SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spUpdateTimeEntriesHourPrice]
(
	@projectId int,
	@invoiceId int,
	@newPrice float
)		
AS
BEGIN
	UPDATE te
	SET te.Price = @newPrice
	FROM TimeEntries te
		INNER JOIN Tasks t ON te.TaskID = t.TaskID
		INNER JOIN Projects p ON t.ProjectID = p.ProjectID
	WHERE 
		p.ProjectID = @projectId
		AND te.InvoiceId = @invoiceId
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
	
	DECLARE @temp3 TABLE ( ProjectID int );
	
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
		t.Project,
		t.ProjectID
	INTO #temp2
	FROM dbo.AggregatedTimeEntriesPrTaskPrDayPrInvoice(@invoiceId, 1) t
	WHERE 
		t.Billable = @billable
	GROUP BY
		t.FixedPrice,
		t.Project,
		t.ProjectID

	INSERT INTO @temp3 
	SELECT t2.ProjectID
	FROM #temp2 t2
	
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
	
	SELECT *
	FROM @temp3
	--SELECT 
	--	t1.*
	--FROM #temp t1
	
	--SELECT *
	--FROM #temp2 t2
	
	DROP TABLE #temp, #temp2
END
GO

ALTER FUNCTION [dbo].[FindVAT]
(
	@invoiceId int
)
RETURNS float
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result float;
	DECLARE @temp TABLE( exclVat float );


	INSERT INTO @temp
	SELECT
		CASE
			WHEN (il.PricePrUnit * il.Units) IS NULL
				THEN 0
				ELSE (
				CASE
					WHEN il.UnitType = 2
						THEN il.PricePrUnit
						ELSE il.PricePrUnit * il.Units
				END)
		END 
	FROM InvoiceLines il
	WHERE il.InvoiceID = @invoiceId

	SET @result = (SELECT SUM(t.exclVat)
	FROM @temp t)
	
	
	RETURN @result;

END
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.6.0.2', GETDATE(), 'LLS & RAB', 'Added [spUpdateTimeEntriesHourPrice] and fixed bug in [spGenerateInvoiceLines] and corrected [FindVAT]')
GO 