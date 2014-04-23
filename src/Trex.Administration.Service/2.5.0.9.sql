
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGetCustomersInvoiceView]
	@startDate DATETIME,
	@endDate DATETIME
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF
	
	SELECT 
		c1.CustomerID,
		c1.CustomerName,  
		
		FirstDateNotInvoiced = (
			SELECT TOP 1 te.StartTime
			FROM TimeEntries te
				INNER JOIN Tasks t ON t.TaskID = te.TaskID
				INNER JOIN Projects p ON p.ProjectID = t.ProjectID
				INNER JOIN Customers c ON c.CustomerID = p.CustomerID
			WHERE c.CustomerID = c1.CustomerID
			and te.Billable = 1
			and (te.DocumentType = 1 or te.DocumentType = 3)
			ORDER BY te.StartTime
			),

		DistinctPrices = (
			SELECT COUNT(distinct te.Price) 
			FROM TimeEntries te 
				INNER JOIN Tasks t on t.taskid = te.TaskID
				INNER JOIN Projects p on p.ProjectID = t.ProjectID
				INNER JOIN Customers c on c.CustomerID = p.CustomerID
			WHERE
				te.Billable = 1 
				AND te.StartTime >= @startDate
				AND te.StartTime <= @endDate
				AND c.CustomerID = c1.CustomerID
				AND te.InvoiceId IS NULL
				AND(te.DocumentType = 1 OR te.DocumentType = 3)				
			GROUP BY c.CustomerID
			),

		InventoryValue = (
			SELECT 
				SUM(x.InventoryValue)
			FROM dbo.AggregatedTimeEntriesPrTaskPrDay(c1.CustomerID, @startDate, @endDate, 1) x
			GROUP BY
				x.Customer
			),
		
		NonBillableTime =(
			select SUM(te.BillableTime) from TimeEntries te
				INNER JOIN Tasks t on t.taskid = te.TaskID
				INNER JOIN Projects p on p.ProjectID = t.ProjectID
				INNER JOIN Customers c on c.CustomerID = p.CustomerID
			where te.Billable = 0
				AND te.StartTime >= @startDate
				AND te.StartTime <= @endDate
				AND c.CustomerID = c1.CustomerID
				--AND te.InvoiceId IS NULL
				--AND(te.DocumentType = 1 OR te.DocumentType = 3)
			),
		
		Drafts = (
			CASE
				WHEN(
					SELECT SUM(
						CASE
						WHEN i.InvoiceID is null
							THEN 1
							ELSE 0
						END)
					FROM Invoices i
						INNER JOIN CustomerInvoiceGroup cig
							ON cig.CustomerInvoiceGroupID = i.CustomerInvoiceGroupId
					WHERE c1.CustomerID = cig.CustomerID) IS NULL
					THEN 0
				
					ELSE 
						(SELECT SUM(
							CASE
								WHEN i.InvoiceID IS NULL
									THEN 1
									ELSE 0
							END)
						FROM Invoices i
							INNER JOIN CustomerInvoiceGroup cig
								ON cig.CustomerInvoiceGroupID = i.CustomerInvoiceGroupId
						WHERE c1.CustomerID = cig.CustomerID
						)					
				END)
	INTO #temp
	FROM Customers c1
		INNER JOIN CustomerInvoiceGroup cig ON c1.CustomerId = cig.CustomerId
		INNER JOIN Invoices i ON cig.CustomerInvoiceGroupId = i.CustomerInvoiceGroupId
	GROUP BY 
		c1.CustomerName,
		c1.CustomerID,
		i.ID
	ORDER BY 
		c1.CustomerName
	
	
	SELECT
		t.CustomerID,
		t.CustomerName,
		t.DistinctPrices,
		t.Drafts,
		MIN(t.FirstDateNotInvoiced) AS FirstDateNotInvoiced,
		t.InventoryValue AS InventoryValue,
		t.NonBillableTime
	FROM #temp t
	GROUP BY
		t.CustomerID,
		t.CustomerName,
		t.InventoryValue,
		t.DistinctPrices,
		t.Drafts,
		t.NonBillableTime
	ORDER BY 
		t.CustomerName
	
	DROP TABLE #temp

END
GO

ALTER FUNCTION [dbo].[AggregatedTimeEntriesPrTaskPrDay] 
(
	@customerId INT, 
	@startdate DATETIME, 
	@enddate DATETIME, 
	@billable BIT
)
RETURNS TABLE
AS
RETURN
(
	--First, sum all timeentries,grouped by date, and round time to nearest quarter
	SELECT     
		t.Customer, 
		t.Project, 
		t.Task, 
		SUM(t.TimeSpent * t.Price) AS InventoryValue,
		t.Remaining,
		t.StartDate
	FROM(
		SELECT   
			(te.StartTime) AS StartDate, 
			c.CustomerName AS Customer, 
			p.ProjectName AS Project, 
			t.TaskName AS Task, 
			dbo.RoundUpToNextQuarter(SUM(te.TimeSpent)) AS TimeSpent,
			te.Price, 
			t.TimeLeft AS Remaining                       
		FROM dbo.TimeEntries AS te 
			INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
			INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
			INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
			INNER JOIN dbo.Users AS u ON u.UserID = te.UserID
		WHERE 
			(te.StartTime) >= @startdate 
			AND (te.endtime) <= @enddate
			AND c.CustomerID = @customerId
			AND te.Billable = @billable
			--AND te.InvoiceId IS NULL
			AND (te.DocumentType = 1 OR te.DocumentType = 3)
		GROUP BY 
			(te.StartTime), 
			c.CustomerName, 
			p.ProjectName, 
			t.TaskName, 
			te.Price,
			t.TimeLeft
	) t
	GROUP BY 
		t.Customer, 
		t.Project, 
		t.Task, 
		t.Remaining, 
		t.StartDate
)
GO

CREATE NONCLUSTERED INDEX [TasksTaskID_ix] ON [dbo].[Tasks] 
(
	[TaskID] ASC
)
INCLUDE ( [ProjectID],
[TaskName],
[TimeLeft]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [TasksprojectID_ix] ON [dbo].[Tasks] 
(
	[ProjectID] ASC
)
INCLUDE ( [TaskID],
[TaskName],
[TimeLeft]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [TimeEntriesBillableDocType_ix] ON [dbo].[TimeEntries] 
(
	[Billable] ASC,
	[DocumentType] ASC
)
INCLUDE ( [TaskID],
[StartTime],
[EndTime],
[Price],
[TimeSpent]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [TimeEntriesBillableStartTime_ix] ON [dbo].[TimeEntries] 
(
	[StartTime] ASC,
	[BillableTime] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [TimeEntriesBillableInvoiceIdDocType_ix] ON [dbo].[TimeEntries] 
(
	[Billable] ASC,
	[InvoiceId] ASC,
	[DocumentType] ASC
)
INCLUDE ( [TaskID],
[Price]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE PROCEDURE [dbo].[spDeleteInvoice]
	@invoiceId int
AS
BEGIN
	DELETE FROM InvoiceLines
	WHERE InvoiceID = @invoiceId
	
	DELETE FROM CreditNote
	WHERE InvoiceID = @invoiceId
	
	DELETE FROM InvoiceFiles
	WHERE InvoiceID = @invoiceId
	
	DELETE FROM Invoices
	WHERE ID = @invoiceId
END
GO


INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.5.0.9', GETDATE(), 'RAB', 'Added a lot of indexes to TimeEntries and Tasks to optimize the searchtime, added [deleteInvoice] to get rid of failed credit note Invoices, updated [spGetCustomersInvoiceView] to use Doctype instead of te.InvoiceId')
GO
