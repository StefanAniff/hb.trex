
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--This may have to be CREATE
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
		SUM(t.TimeSpent) AS TimeSpent, 
		t.Price,
		t.Remaining,
		t.StartDate
	FROM(
		SELECT   
			dbo.ConvertToSmallDate(te.StartTime) AS StartDate, 
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
			dbo.ConvertToSmallDate(te.StartTime) >= @startdate 
			AND dbo.ConvertToSmallDate(te.endtime) <= @enddate
			AND c.CustomerID = @customerId
			AND te.Billable = @billable
			AND te.InvoiceId IS NULL
			--AND (te.DocumentType = 1 OR te.DocumentType = 3)
		GROUP BY 
			dbo.ConvertToSmallDate(te.StartTime), 
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
		t.Price,
		t.StartDate
)
GO

--This may have to be CREATE
ALTER FUNCTION [dbo].[AggregatedTimeEntriesPrTaskPrDayPrInvoice] 
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
			u.UserID
		FROM dbo.TimeEntries AS te 
			INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
			INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
			INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
			INNER JOIN dbo.Invoices AS i ON te.InvoiceId = i.ID
			INNER JOIN dbo.Users AS u ON u.UserID = te.UserID
		WHERE i.ID = @InvoiceId
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

ALTER PROCEDURE [dbo].[spGetSpecificationData_Project]
	@invoiceId int,
	@billable bit
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF

	DECLARE @temp TABLE (TimePrUser float, Task nvarchar(200), ProjectID int, Project nvarchar(200));
	
	--DECLARE @isInvoiced BIT = 
	--	CASE
	--		WHEN (SELECT TOP 1 i.InvoiceID FROM Invoices i WHERE i.ID = @invoiceId) IS NULL
	--			THEN 0
	--			ELSE 1
	--	END
	
	--IF  @isInvoiced = 1
	--	BEGIN
	--		INSERT INTO @temp(TimePrUser, Task, ProjectID, Project)
	--		SELECT
	--			dbo.RoundUpToNextQuarter(SUM(t.TimeSpent)) AS TimePrUser,
	--			t.Task,
	--			t.ProjectID,
	--			t.Project
	--		FROM dbo.AggregatedCreditNotesPrTaskPrDayPrInvoice(@invoiceId) t
	--		WHERE t.Billable = @billable
	--		GROUP BY
	--			t.Task,
	--			t.ProjectID,
	--			t.Project			
	--	END
	--ELSE
	--	BEGIN
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
		--END
	
		
		
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
	
	--DECLARE @isInvoiced BIT = 
	--	CASE
	--		WHEN (SELECT TOP 1 i.InvoiceID FROM Invoices i WHERE i.ID = @invoiceId) IS NULL
	--			THEN 0
	--			ELSE 1
	--	END
	
	--IF  @isInvoiced = 1
	--	BEGIN
	--		INSERT INTO @temp(TimePrUser, Task, ProjectID)
	--		SELECT
	--			dbo.RoundUpToNextQuarter(SUM(x.TimeSpent)) AS TimePrUser,
	--			x.Task,
	--			x.ProjectID
	--		FROM dbo.AggregatedCreditNotesPrTaskPrDayPrInvoice(@invoiceId) x
	--		WHERE x.Billable = @billable
	--		GROUP BY
	--			x.Task,
	--			x.ProjectID			
	--	END
	--ELSE
	--	BEGIN
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
		--END
		
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
			SELECT TOP 1 t.StartDate --te.StartTime
			FROM dbo.AggregatedTimeEntriesPrTaskPrDay(c1.CustomerID, @startDate, @endDate, 1) t
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
				--AND(te.DocumentType = 1 OR te.DocumentType = 3)				
			GROUP BY c.CustomerID
			),

		InventoryValue = (
			SELECT 
				SUM(x.Price * x.TimeSpent)
			FROM dbo.AggregatedTimeEntriesPrTaskPrDay(c1.CustomerID, @startDate, @endDate, 1) x
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
				AND te.InvoiceId IS NULL
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
		
	--SELECT *
	--FROM #temp t
	--ORDER BY t.CustomerName
	
	
	SELECT
		t.CustomerID,
		t.CustomerName,
		t.DistinctPrices,
		t.Drafts,
		MIN(t.FirstDateNotInvoiced) AS FirstDateNotInvoiced,
		SUM(t.InventoryValue) AS InventoryValue,
		t.NonBillableTime
	FROM #temp t
	GROUP BY
		t.CustomerID,
		t.CustomerName,
		t.DistinctPrices,
		t.Drafts,
		t.NonBillableTime
	ORDER BY 
		t.CustomerName
	
	DROP TABLE #temp

END
GO


INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.4.10.4', GETDATE(), 'RAB', 'Added the Stored Procedure [AggregatedTimeEntriesPrTaskPrDay] and [AggregatedTimeEntriesPrTaskPrDay], edited [spGetSpecificationData_Project], [spGetSpecificationData_Task], [spGenerateInvoiceLines] and [spGetCustomersInvoiceView]')
GO