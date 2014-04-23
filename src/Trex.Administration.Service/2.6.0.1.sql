  
use trex_base --To be changed?
GO
insert into Permissions
VALUES('TemplateManagementPermission', 1)

insert into Permissions
VALUES('FinalizeDraftPermission', 1)

insert into Permissions
VALUES('GenerateDeleteDraftPermission', 1)


USE trex --To be changed
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[AggregatedCreditNotesPrTaskPrDayPrInvoice] 
(
	@InvoiceId INT,
	@FixedPrice BIT
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
		t.InvoiceNumber,
		t.FixedPrice,
		t.FixedPriceProject
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
			u.UserID,
			p.FixedPriceProject,
			p.FixedPrice
		FROM dbo.CreditNote AS cn 
			INNER JOIN dbo.Tasks AS t ON t.TaskID = cn.TaskID 
			INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
			INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
			INNER JOIN dbo.Invoices AS i ON cn.InvoiceId = i.ID
			INNER JOIN dbo.Users AS u ON u.UserID = cn.UserID
		WHERE 
			i.ID = @InvoiceId
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
			u.UserID,
			p.FixedPriceProject,
			p.FixedPrice
		) t
	WHERE t.FixedPriceProject = @FixedPrice
	GROUP BY 
		t.Customer, 
		t.ProjectID,
		t.Project, 
		t.TaskID,
		t.Task,
		t.Price,
		t.Billable,
		t.InvoiceNumber,
		t.CustomerId,
		t.FixedPrice,
		t.FixedPriceProject
)
GO

ALTER FUNCTION [dbo].[AggregatedTimeEntriesPrTaskPrDayPrInvoice] 
(
	@InvoiceId INT,
	@FixedPrice BIT
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
		t.InvoiceNumber,
		t.FixedPrice,
		t.FixedPriceProject
	FROM (	
		SELECT   
			dbo.ConvertToSmallDate(te.StartTime) AS TaskDate, 
			c.CustomerName AS Customer,
			c.customerId,
			p.ProjectName AS Project, 
			p.ProjectID,
			t.TaskName AS Task,
			t.TaskID,
			dbo.RoundUpToNextQuarter(SUM(te.TimeSpent)) AS TimeSpent,
			te.Price,
			te.Billable,
			i.ID AS InvoiceNumber,
			u.UserID,
			p.FixedPriceProject,
			p.FixedPrice
		FROM dbo.TimeEntries AS te 
			INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
			INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
			INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
			INNER JOIN dbo.Invoices AS i ON te.InvoiceId = i.ID
			INNER JOIN dbo.Users AS u ON u.UserID = te.UserID
		WHERE 
			i.ID = @InvoiceId
			AND p.FixedPriceProject = @FixedPrice
		GROUP BY 
			dbo.ConvertToSmallDate(te.StartTime), 
			c.CustomerName, 
			p.ProjectName, 
			p.ProjectID,
			t.TaskName,
			t.TaskID,
			te.Price,
			c.customerId,
			te.Billable,
			i.ID, 
			u.UserID,
			p.FixedPriceProject,
			p.FixedPrice
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
		t.CustomerId,
		t.FixedPrice,
		t.FixedPriceProject
)
GO

ALTER PROCEDURE [dbo].[spGetSpecificationData_Project]
	@invoiceId int,
	@billable bit,
	@fixedPrice bit
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF

	DECLARE @temp TABLE (TimePrUser float, Task nvarchar(200), ProjectID int, Project nvarchar(200), FixedPriceProject bit, FixedPrice float);
	
	DECLARE @isInvoiced BIT = 
		CASE
			WHEN (SELECT TOP 1 i.InvoiceID FROM Invoices i WHERE i.ID = @invoiceId) IS NULL
				THEN 0
				ELSE 1
		END
	
	IF  @isInvoiced = 1
		BEGIN
			INSERT INTO @temp(TimePrUser, Task, ProjectID, Project, FixedPriceProject, FixedPrice)
			SELECT
				dbo.RoundUpToNextQuarter(SUM(t.TimeSpent)) AS TimePrUser,
				t.Task,
				t.ProjectID,
				t.Project,
				t.FixedPriceProject,
				t.FixedPrice
			FROM dbo.AggregatedCreditNotesPrTaskPrDayPrInvoice(@invoiceId, @fixedPrice) t --From CreditNote
			WHERE t.Billable = @billable
			GROUP BY
				t.Task,
				t.ProjectID,
				t.Project,
				t.FixedPriceProject,
				t.FixedPrice			
		END
	ELSE
		BEGIN
			INSERT INTO @temp(TimePrUser, Task, ProjectID, Project, FixedPriceProject, FixedPrice)
			SELECT
				dbo.RoundUpToNextQuarter(SUM(t.TimeSpent)) AS TimePrUser,
				t.Task,
				t.ProjectID,
				t.Project,
				t.FixedPriceProject,
				t.FixedPrice
			FROM dbo.AggregatedTimeEntriesPrTaskPrDayPrInvoice(@invoiceId, @fixedPrice) t --From TimeEntries
			WHERE t.Billable = @billable
			GROUP BY
				t.Task,
				t.ProjectID,
				t.Project,
				t.FixedPriceProject,
				t.FixedPrice
		END		
		
	SELECT 
		t.ProjectID,
		t.Project AS ProjectName,
		SUM(t.TimePrUser) AS [TimeUsed],
		t.FixedPriceProject,
		t.FixedPrice
	FROM @temp t
	GROUP BY
		t.ProjectID,
		t.Project,
		t.FixedPriceProject,
		t.FixedPrice
END
GO

ALTER PROCEDURE [dbo].[spGetSpecificationData_Task]
(
	@invoiceId int,
	@billable bit,
	@fixedProject bit
)
AS
BEGIN		
	SET NOCOUNT ON
	SET FMTONLY OFF

	DECLARE @temp TABLE (TimePrUser float, Task nvarchar(200), ProjectID int, FixedPriceProject bit, FixedPrice float);
	
	DECLARE @isInvoiced BIT = 
		CASE
			WHEN (SELECT TOP 1 i.InvoiceID FROM Invoices i WHERE i.ID = @invoiceId) IS NULL
				THEN 0
				ELSE 1
		END
	
	IF  @isInvoiced = 1
		BEGIN
			INSERT INTO @temp(TimePrUser, Task, ProjectID, FixedPriceProject, FixedPrice)
			SELECT
				dbo.RoundUpToNextQuarter(SUM(x.TimeSpent)) AS TimePrUser,
				x.Task,
				x.ProjectID,
				x.FixedPriceProject,
				x.FixedPrice
			FROM dbo.AggregatedCreditNotesPrTaskPrDayPrInvoice(@invoiceId, @fixedProject) x
			WHERE x.Billable = @billable
			GROUP BY
				x.Task,
				x.ProjectID	,
				x.FixedPriceProject,
				x.FixedPrice		
		END
	ELSE
		BEGIN
			INSERT INTO @temp(TimePrUser, Task, ProjectID, FixedPriceProject, FixedPrice)
			SELECT
				dbo.RoundUpToNextQuarter(SUM(x.TimeSpent)) AS TimePrUser,
				x.Task,
				x.ProjectID,
				x.FixedPriceProject,
				x.FixedPrice
			FROM dbo.AggregatedTimeEntriesPrTaskPrDayPrInvoice(@invoiceId, @fixedProject) x
			WHERE x.Billable = @billable
			GROUP BY
				x.Task,
				x.ProjectID,
				x.FixedPriceProject,
				x.FixedPrice
		END
		
	SELECT 
		t.Task AS TaskName,
		t.ProjectID,
		SUM(t.TimePrUser) AS [TimeUsed],
		t.FixedPriceProject,
		t.FixedPrice
	FROM @temp t
	GROUP BY
		t.Task,
		t.ProjectID,
		t.FixedPriceProject,
		t.FixedPrice
	
END
GO

ALTER PROCEDURE [dbo].[spGenerateInvoiceLines]
	@invoiceId int,
	@billable bit,
	@Unit nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF
	
	SELECT 
		t.Price,
		SUM(t.TimeSpent) AS TimeSpent
	INTO #temp
	FROM dbo.AggregatedTimeEntriesPrTaskPrDayPrInvoice(@invoiceId, 0) t
	WHERE t.Billable = @billable
	GROUP BY
		t.Price
		
	SELECT 
		t.FixedPrice,
		SUM(t.TimeSpent) AS TimeSpent,
		t.Project
	INTO #temp2
	FROM dbo.AggregatedTimeEntriesPrTaskPrDayPrInvoice(@invoiceId, 1) t
	WHERE 
		t.Billable = @billable
	GROUP BY
		t.FixedPrice,
		t.Project

		
	
	DELETE FROM InvoiceLines
	WHERE 
		(UnitType = 0
		OR UnitType = 2)
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
		t.FixedPrice,
		t.TimeSpent,
		@invoiceId,
		@Unit,
		2,
		0,
		(SELECT TOP 1 i.VAT FROM Invoices i WHERE i.ID = @invoiceId), --VAT
		t.Project
	FROM #temp2 t
	
	--SELECT 
	--	t1.*
	--FROM #temp t1
	
	--SELECT *
	--FROM #temp2 t2
	
	DROP TABLE #temp, #temp2
END
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.6.0.1', GETDATE(), 'LLS & RAB', 
'Added 3 new Permissions, to permissionTable,
Edited [AggregatedTimeEntriesPrTaskPrDayPrInvoice], [spGetSpecificationData_Project], [spGetSpecificationData_Task], [AggregatedCreditNotesPrTaskPrDayPrInvoice] and [spGenerateInvoiceLines]')
GO