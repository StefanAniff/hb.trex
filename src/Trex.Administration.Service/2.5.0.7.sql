ALTER TABLE Invoices
ADD InvoiceParentId int NULL DEFAULT NULL
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[AggregatedCreditNotesPrTaskPrDayPrInvoice] 
(
	@InvoiceId INT
)
RETURNS TABLE
AS
RETURN
(
	--First, sum all timeentries,grouped by date, and round time to nearest quarter
		SELECT  
		t.customerId AS CustomerId,   
		t.Customer AS CustomerName, 
		t.ProjectID,
		t.Project, 
		t.TaskID,
		t.Task, 
		SUM(t.TimeSpent) AS TimeSpent, 
		t.Price,
		t.Billable,
		t.InvoiceNumber 
	FROM (	
		SELECT   
			dbo.ConvertToSmallDate(cn.StartTime) AS TaskDate, 
			c.CustomerName AS Customer,
			c.customerId,
			p.ProjectName AS Project, 
			p.ProjectID,
			t.TaskName AS Task,
			t.TaskID,
			dbo.RoundUpToNextQuarter(SUM(cn.TimeSpent)) AS TimeSpent,
			cn.Price,
			cn.Billable,
			i.ID AS InvoiceNumber,
			u.UserID
		FROM dbo.CreditNote AS cn 
			INNER JOIN dbo.Tasks AS t ON t.TaskID = cn.TaskID 
			INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
			INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
			INNER JOIN dbo.Invoices AS i ON cn.InvoiceId = i.ID
			INNER JOIN dbo.Users AS u ON u.UserID = cn.UserID
		WHERE i.ID = @InvoiceId
		GROUP BY 
			dbo.ConvertToSmallDate(cn.StartTime), 
			c.CustomerName, 
			p.ProjectName, 
			p.ProjectID,
			t.TaskName,
			t.TaskID,
			cn.Price,
			c.customerId,
			cn.Billable,
			i.ID, 
			u.UserID
		) t
	GROUP BY 
		t.Customer, 
		t.ProjectID,
		t.Project, 
		t.TaskID,
		t.Task,
		t.Price,
		t.Billable,
		t.InvoiceNumber,
		t.CustomerId
)
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
		dbo.RoundUpToNextQuarter(te.TimeSpent) AS TimeSpent, 
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
        INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
        INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
        INNER JOIN dbo.CustomerInvoiceGroup AS cig ON cig.CustomerInvoiceGroupID = p.CustomerInvoiceGroupID 
        INNER JOIN dbo.Users AS u ON u.UserID = te.UserID 
   WHERE 
		dbo.ConvertToSmallDate(te.StartTime) >= @startdate 
		AND dbo.ConvertToSmallDate(te.endtime) <= @enddate
		AND cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
		AND te.Billable = 1
		AND (te.DocumentType = 1 OR te.DocumentType = 3)
)
GO

ALTER PROCEDURE [dbo].[spGetSpecificationData_Task]
(
	@invoiceId int,
	@billable bit
)
AS
BEGIN		
	SET NOCOUNT ON
	SET FMTONLY OFF

	DECLARE @temp TABLE (TimePrUser float, Task nvarchar(200), ProjectID int);
	
	DECLARE @isInvoiced BIT = 
		CASE
			WHEN (SELECT TOP 1 i.InvoiceID FROM Invoices i WHERE i.ID = @invoiceId) IS NULL
				THEN 0
				ELSE 1
		END
	
	IF  @isInvoiced = 1
		BEGIN
			INSERT INTO @temp(TimePrUser, Task, ProjectID)
			SELECT
				dbo.RoundUpToNextQuarter(SUM(x.TimeSpent)) AS TimePrUser,
				x.Task,
				x.ProjectID
			FROM dbo.AggregatedCreditNotesPrTaskPrDayPrInvoice(@invoiceId) x
			WHERE x.Billable = @billable
			GROUP BY
				x.Task,
				x.ProjectID			
		END
	ELSE
		BEGIN
			INSERT INTO @temp(TimePrUser, Task, ProjectID)
			SELECT
				dbo.RoundUpToNextQuarter(SUM(x.TimeSpent)) AS TimePrUser,
				x.Task,
				x.ProjectID
			FROM dbo.AggregatedTimeEntriesPrTaskPrDayPrInvoice(@invoiceId) x
			WHERE x.Billable = @billable
			GROUP BY
				x.Task,
				x.ProjectID	
		END
		
	SELECT 
		t.Task AS TaskName,
		t.ProjectID,
		SUM(t.TimePrUser) AS [TimeUsed]
	FROM @temp t
	GROUP BY
		t.Task,
		t.ProjectID
	
END
GO

ALTER PROCEDURE [dbo].[spGenerateInvoiceLines]
	@invoiceId int,
	@billable bit,
	@Unit nvarchar(50) --Size of InvoiceLine.Unit
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF
	
	SELECT 
		t.Price,
		SUM(t.TimeSpent) AS TimeSpent
	INTO #temp
	FROM dbo.AggregatedTimeEntriesPrTaskPrDayPrInvoice(@invoiceId) t
	WHERE t.Billable = @billable
	GROUP BY
		t.Price

		
	
	DELETE FROM InvoiceLines
	WHERE 
		UnitType = 0
		AND InvoiceID = @invoiceId	
	
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
	SELECT
		t.Price,
		t.TimeSpent,
		@invoiceId,
		@Unit,
		0,
		0,
		(SELECT TOP 1 i.VAT FROM Invoices i WHERE i.ID = @invoiceId), --VAT
		''
	FROM #temp t
	
	DROP TABLE #temp
END
GO

ALTER PROCEDURE [dbo].[spGetSpecificationData_Project]
	@invoiceId int,
	@billable bit
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF

	DECLARE @temp TABLE (TimePrUser float, Task nvarchar(200), ProjectID int, Project nvarchar(200));
	
	DECLARE @isInvoiced BIT = 
		CASE
			WHEN (SELECT TOP 1 i.InvoiceID FROM Invoices i WHERE i.ID = @invoiceId) IS NULL
				THEN 0
				ELSE 1
		END
	
	IF  @isInvoiced = 1
		BEGIN
			INSERT INTO @temp(TimePrUser, Task, ProjectID, Project)
			SELECT
				dbo.RoundUpToNextQuarter(SUM(t.TimeSpent)) AS TimePrUser,
				t.Task,
				t.ProjectID,
				t.Project
			FROM dbo.AggregatedCreditNotesPrTaskPrDayPrInvoice(@invoiceId) t
			WHERE t.Billable = @billable
			GROUP BY
				t.Task,
				t.ProjectID,
				t.Project			
		END
	ELSE
		BEGIN
			INSERT INTO @temp(TimePrUser, Task, ProjectID, Project)
			SELECT
				dbo.RoundUpToNextQuarter(SUM(t.TimeSpent)) AS TimePrUser,
				t.Task,
				t.ProjectID,
				t.Project
			FROM dbo.AggregatedTimeEntriesPrTaskPrDayPrInvoice(@invoiceId) t
			WHERE t.Billable = @billable
			GROUP BY
				t.Task,
				t.ProjectID,
				t.Project
		END
	
		
		
	SELECT 
		t.ProjectID,
		t.Project AS ProjectName,
		SUM(t.TimePrUser) AS [TimeUsed]
	FROM @temp t
	GROUP BY
		t.ProjectID,
		t.Project
END
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.5.0.7', GETDATE(), 'RAB', 'Added functions to aggregate TimeEntries for each invoice, 
	altered [spGetCustomersInvoiceView], [spGetSpecificationData_Task], 
		[spGetSpecificationData_Project] and [spGenerateInvoiceLines], 
	Added "Default" as default name for all default CustomerInvoiceGroups')
GO