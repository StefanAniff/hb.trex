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
		AND te.endtime <= @enddate
		AND cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
		AND te.Billable = 1
		AND (te.InvoiceId is null OR te.InvoiceId = @invoiceId)
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
		AND te.endtime <= @enddate
		AND cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
		AND (te.InvoiceId is NULL OR te.InvoiceId = @invoiceId)
)
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGenerateInvoiceLines]
@invoiceId int,
@startDate DATETIME,
@endDate DATETIME,
@cigID int
AS
BEGIN
	
	--Find different prices
	SELECT 
		ID_NUM = IDENTITY(int, 1, 1),
		fte.[Price pr Hour],
		SUM(fte.TimeSpent) AS TimeSpent
	INTO #NewTable
	FROM dbo.FindBillableTimeEntries(@invoiceId, @cigID, @startDate, @endDate) fte
	GROUP BY fte.[Price pr Hour];

	--Remove old lines
	DELETE FROM InvoiceLines
	WHERE 
		InvoiceID = @invoiceId
		AND UnitType = '0'; --Auto generated lines
		
		
	DECLARE @MaxID int;
	DECLARE @Counter int;
	
	 
	SET @Counter = 1;
	
	SELECT @MaxID = COUNT(t.ID_NUM) 
	FROM #NewTable t;
	
	WHILE @Counter <= @MaxID 
	BEGIN
	
		DECLARE @f1 float;
		DECLARE @f2 float;
		DECLARE @f3 int;
		DECLARE @f4 nvarchar(50);
		DECLARE @f5 int;
		DECLARE @f6 bit;
		DECLARE @f7 float;
		DECLARE @f8 nvarchar(1000);
		
		SET @f1 = (SELECT t.[Price pr Hour] FROM #NewTable t WHERE ID_NUM = @Counter);
		SET @f2 = (SELECT t.TimeSpent FROM #NewTable t WHERE ID_NUM = @Counter);
		SET @f3 = @invoiceId;
		SET @f4 = 'timer';
		SET @f5 = 0; --Auto generated
		SET @f6 = 0; --Isexpense (udlæg)
		SET @f7 = (SELECT TOP 1 i.VAT FROM Invoices i WHERE i.ID = @invoiceId); --VAT
		SET @f8 = '';
		
		
		--Insert into table
		INSERT INTO InvoiceLines 
			(PricePrUnit, 
			Units, 
			InvoiceID, 
			Unit, 
			UnitType,
			IsExpense, 
			VatPercentage,
			[Text]) 
		VALUES (@f1, @f2, @f3, @f4, @f5, @f6, @f7, @f8);
				     
		-- update counter
		set @Counter = @Counter + 1;
	END;
	
	
	DROP TABLE #NewTable;
	
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[UpdateTimeEntriesInvoiceId]
	@customerInvoiceGroupId INT, 
	@startdate DATETIME, 
	@enddate DATETIME,
	@invoiceId int
AS
BEGIN

	UPDATE TimeEntries
	SET InvoiceId = @invoiceId
	WHERE TimeEntryID = ANY(SELECT [TimeEntry ID] 
						FROM [dbo].FindAllTimeEntries(@invoiceId, @customerInvoiceGroupId, @startdate, @enddate))
END
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.4.8.5', GETDATE(), 'RAB', 'Minor bug in function FindBillableTimeEntries, FindAllTimeEntries, spGenerateInvoicLines and UpdateTimeEntriesInvoiceId')
GO