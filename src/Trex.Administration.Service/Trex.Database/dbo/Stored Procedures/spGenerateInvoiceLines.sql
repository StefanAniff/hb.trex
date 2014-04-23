
CREATE PROCEDURE [dbo].[spGenerateInvoiceLines]
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
	
	SELECT 
		t3.ProjectID,
		il.ID as InvoiceLineId
	FROM @temp3 t3,
		InvoiceLines il
	WHERE il.InvoiceID = @invoiceId
	
	--SELECT 
	--	t1.*
	--FROM #temp t1
	
	--SELECT *
	--FROM #temp2 t2
	
	DROP TABLE #temp, #temp2
END
