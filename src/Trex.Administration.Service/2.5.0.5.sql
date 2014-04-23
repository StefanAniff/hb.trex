SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[FindBillableTimeEntries]
(	
	@invoiceId INT,
	@customerInvoiceGroupId INT, 
	@startdate DATETIME, 
	@enddate DATETIME
)
RETURNS TABLE 
AS
RETURN 
(
   SELECT   
		dbo.RoundUpToNextQuarter(te.BillableTime) AS TimeSpent, 
		dbo.ConvertToSmallDate(te.StartTime) AS StartDate, 
		dbo.ConvertToSmallDate(te.EndTime) AS EndDate,
		p.ProjectName AS Project, 
		p.ProjectID AS [Project ID],
		t.TaskName AS Task, 
		t.TaskID AS [Task ID],
		te.Price AS [Price pr Hour],
		te.InvoiceId AS InvoiceID,
		p.CustomerInvoiceGroupID AS [CIG ID],
		p.CustomerID AS [Customer ID],
		cig.Label AS GroupName,
		te.TimeEntryID AS [TimeEntry ID]
                    
   FROM 
		dbo.TimeEntries AS te 
        INNER JOIN dbo.Tasks AS t 
			ON t.TaskID = te.TaskID 
        INNER JOIN dbo.Projects AS p 
			ON p.ProjectID = t.ProjectID 
        INNER JOIN dbo.CustomerInvoiceGroup AS cig 
			ON cig.CustomerInvoiceGroupID = p.CustomerInvoiceGroupID 
        INNER JOIN dbo.Users AS u 
			ON u.UserID = te.UserID 
   WHERE 
		te.StartTime >= @startdate 
		AND te.endtime <= DATEADD(S, -1, DATEADD(D, 1, @enddate))
		AND cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
		AND te.Billable = 1
		AND (te.DocumentType = 1 OR te.DocumentType = 3)
)
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[FindAllTimeEntries]
(	
	@invoiceId INT,
	@customerInvoiceGroupId INT, 
	@startdate DATETIME, 
	@enddate DATETIME
)
RETURNS TABLE 
AS
RETURN 
(
   SELECT   
		dbo.RoundUpToNextQuarter(te.BillableTime) AS TimeSpent, 
		dbo.ConvertToSmallDate(te.StartTime) AS StartDate, 
		dbo.ConvertToSmallDate(te.EndTime) AS EndDate,
		p.ProjectName AS Project, 
		p.ProjectID AS [Project ID],
		t.TaskName AS Task, 
		t.TaskID AS [Task ID],
		te.Price AS [Price pr Hour],
		te.InvoiceId AS InvoiceID,
		p.CustomerInvoiceGroupID AS [CIG ID],
		p.CustomerID AS [Customer ID],
		cig.Label AS GroupName,
		te.TimeEntryID AS [TimeEntry ID]
                    
   FROM 
		dbo.TimeEntries AS te 
        INNER JOIN dbo.Tasks AS t 
			ON t.TaskID = te.TaskID 
        INNER JOIN dbo.Projects AS p 
			ON p.ProjectID = t.ProjectID 
        INNER JOIN dbo.CustomerInvoiceGroup AS cig 
			ON cig.CustomerInvoiceGroupID = p.CustomerInvoiceGroupID 
        INNER JOIN dbo.Users AS u 
			ON u.UserID = te.UserID 
   WHERE 
		te.StartTime >= @startdate 
		AND te.endtime <= DATEADD(S, -1, DATEADD(D, 1, @enddate)) 
		AND	cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
		AND (te.DocumentType = 1 OR te.DocumentType = 3)
)
GO




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spCopyTimeEntriesToCreditNote]
	@invoiceId int
AS
BEGIN
	SELECT *
	INTO #temp
	FROM TimeEntries te
	WHERE te.InvoiceId = @invoiceId
	
	INSERT INTO CreditNote
		(
		 Billable,
		 BillableTime,
		 ChangeDate,
		 ChangedBy,
		 ClientSourceId,
		 CreateDate,
		 CreditNoteDate,
		 Description,
		 EndTime, 
		 Guid, 
		 InvoiceID, 
		 PauseTime, 
		 Price, 
		 StartTime, 
		 TaskID, 
		 TimeEntryID, 
		 TimeEntryTypeId, 
		 TimeSpent, 
		 UserID
		)
	SELECT 
		 t.Billable,
		 t.BillableTime,
		 t.ChangeDate,
		 t.ChangedBy,
		 t.ClientSourceId,
		 t.CreateDate,
		 GETDATE(),
		 t.Description,
		 t.EndTime,
		 t.Guid,
		 t.InvoiceId,
		 t.PauseTime,
		 t.Price,
		 t.StartTime,
		 t.TaskID,
		 t.TimeEntryID,
		 t.TimeEntryTypeId,
		 t.TimeSpent,
		 t.UserID
	FROM #temp t
	
	UPDATE TimeEntries
	SET DocumentType = 2
	WHERE InvoiceId = @invoiceId
	
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGetCustomersInvoiceView]
	@endDate DATETIME
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF
	
	select 
		c1.CustomerID,
		c1.CustomerName,  
		FirstDateNotInvoiced =(

			select top 1 te.StartTime from timeentries te
			inner join Tasks t on t.taskid = te.TaskID
			inner join Projects p on p.ProjectID = t.ProjectID
			inner join Customers c on c.CustomerID = p.CustomerID
			where 
				te.Billable = 1 
				AND (te.DocumentType = 1 OR te.DocumentType = 3)
				and c.CustomerID = c1.CustomerID
			Order by te.StartTime asc)

	,DistinctPrices = (
	select count(distinct te.Price) from TimeEntries te 
	inner join Tasks t on t.taskid = te.TaskID
	inner join Projects p on p.ProjectID = t.ProjectID
	inner join Customers c on c.CustomerID = p.CustomerID
	where 
		te.Billable = 1 
		and te.StartTime < @endDate
		and c.CustomerID =c1.CustomerID
	group by c.CustomerID

	)

	,InventoryValue = (
	select sum( te.Price * te.BillableTime) from TimeEntries te 
	inner join Tasks t on t.taskid = te.TaskID
	inner join Projects p on p.ProjectID = t.ProjectID
	inner join Customers c on c.CustomerID = p.CustomerID
	where 
		te.Billable = 1 
		and te.StartTime < @endDate
		and c.CustomerID =c1.CustomerID 
		AND (te.DocumentType = 1 OR te.DocumentType = 3)
	group by c.CustomerID
	),
	
	NonBillableTime =(
	
	select SUM(te.BillableTime) from TimeEntries te
			inner join Tasks t on t.taskid = te.TaskID
			inner join Projects p on p.ProjectID = t.ProjectID
			inner join Customers c on c.CustomerID = p.CustomerID
			where te.Billable = 0
			and te.StartTime < @endDate
			and c.CustomerID = c1.CustomerID
			AND (te.DocumentType = 1 OR te.DocumentType = 3))

	
	,Drafts = (
	CASE
	WHEN
	(SELECT SUM(
		CASE
		WHEN i.InvoiceID is null
			THEN 1
			ELSE 0
		END)
	FROM Invoices i
		INNER JOIN CustomerInvoiceGroup cig
			ON cig.CustomerInvoiceGroupID = i.CustomerInvoiceGroupId
	WHERE c1.CustomerID = cig.CustomerID
	) is null
		THEN 0
		ELSE (SELECT SUM(
				CASE
				WHEN i.InvoiceID is null
					THEN 1
					ELSE 0
				END)
			FROM Invoices i
				INNER JOIN CustomerInvoiceGroup cig
					ON cig.CustomerInvoiceGroupID = i.CustomerInvoiceGroupId
			WHERE c1.CustomerID = cig.CustomerID
			)
				
	END)

from Customers c1
ORDER BY c1.CustomerName

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spFindTimeEntiresForInvoice] 
@customerInvoiceGroupId INT, 
@startdate DATETIME, 
@enddate DATETIME
AS
BEGIN
   
   SELECT   
		dbo.RoundUpToNextQuarter(te.BillableTime) AS TimeSpent, 
		dbo.ConvertToSmallDate(te.StartTime) AS StartDate, 
		dbo.ConvertToSmallDate(te.EndTime) AS EndDate,
		p.ProjectName AS Project, 
		p.ProjectID AS [Project ID],
		t.TaskName AS Task, 
		t.TaskID AS [Task ID],
		te.Price AS [Price pr Hour],
		te.InvoiceId AS InvoiceID,
		p.CustomerInvoiceGroupID AS [CIG ID],
		p.CustomerID AS [Customer ID],
		cig.Label AS GroupName,
		te.TimeEntryID AS [TimeEntry ID]
                    
   FROM 
		dbo.TimeEntries AS te 
        INNER JOIN dbo.Tasks AS t 
			ON t.TaskID = te.TaskID 
        INNER JOIN dbo.Projects AS p 
			ON p.ProjectID = t.ProjectID 
        INNER JOIN dbo.CustomerInvoiceGroup AS cig 
			ON cig.CustomerInvoiceGroupID = p.CustomerInvoiceGroupID 
        INNER JOIN dbo.Users AS u 
			ON u.UserID = te.UserID 
   WHERE 
		te.StartTime >= @startdate 
		AND te.endtime <= @enddate
		AND cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
		AND te.Billable = 1
		AND (te.DocumentType = 1 OR te.DocumentType = 3)
  
END
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.5.0.5', GETDATE(), 'RAB', 'Added [spCopyTimeEntriesToCreditNote] and edited [FindBillableTimeEntries], [FindAllTimeEntries] and [spGetCustomersInvoiceView] to find entries by DocumentType')
GO