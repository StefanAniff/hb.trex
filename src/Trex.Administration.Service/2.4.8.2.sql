
--Added 14 days as duedate from enddate (invoicedate)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGenerateNewInvoiceDraft]
@customerInvoiceGroupId int,
@createdBy int,
@VAT float,
@startDate DATETIME,
@endDate DATETIME
AS
BEGIN

	DECLARE @Duedate DATETIME;
	DECLARE @invoiceDate DATETIME;
	DECLARE @today DATETIME;
	SET @today = GETDATE();

	--DECLARE @LastDayOfLastMonth DATETIME;
	--DECLARE @LastDayOfCurrentMonth DATETIME;
	--DECLARE @FirstDayOfCurrentMonth DATETIME;
	DECLARE @FirstDayOfNextMonth DATETIME;

	--SET @LastDayOfLastMonth = DATEADD(dd,-(DAY(@today)),@today);
	-- @invoiceDate = @LastDayOfLastMonth;
	--SET @FirstDayOfCurrentMonth = DATEADD(dd,-(DAY(@today)-1),@today);
	--SET @LastDayOfCurrentMonth = DATEADD(dd,-(DAY(DATEADD(mm,1,@today))),DATEADD(mm,1,@today));
	SET @FirstDayOfNextMonth = DATEADD(dd,-(DAY(DATEADD(mm,1,@invoiceDate))-1),DATEADD(mm,1,@invoiceDate));

	--SELECT
	--	@LastDayOfCurrentMonth AS [Last of current],
	--	@LastDayOfLastMonth AS [Last of last],
	--	@FirstDayOfCurrentMonth AS [First of current],
	--	@FirstDayOfNextMonth AS [First of next]

	
	--SET @Duedate = (
	--	SELECT 
	--		CASE
	--			WHEN c.PaymentTermsIncludeCurrentMonth = 0 --Month not included
	--				THEN DATEADD(D, c.PaymentTermsNumberOfDays, @invoiceDate) -- Add paymentTermsNumbersOfDays to InvoiceDate
	--				ELSE DATEADD(D, c.PaymentTermsNumberOfDays, @FirstDayOfNextMonth) --Add paymentTermsNumbersOfDays from the next month's 1th to InvoiceDate
	--		END
	--	FROM Customers c
	--		INNER JOIN CustomerInvoiceGroup cig ON
	--			cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
	--	WHERE cig.CustomerID = c.CustomerID);
			
	--Insert data for a draft
	INSERT INTO Invoices (
		CreateDate, 
		InvoiceDate, 
		CreatedBy, 
		VAT, 
		StartDate, 
		EndDate, 
		Closed, 
		DueDate, 
		CustomerInvoiceGroupId)
	VALUES 
		(
		GETDATE(),
		@endDate, 
		@createdBy,
		@VAT,
		@startDate,
		@endDate,
		0, --Not closed
		DATEADD(D, 14, @endDate), 
		@customerInvoiceGroupId
		);
		
	SELECT TOP 1 *
	FROM Invoices i	
	ORDER BY i.ID DESC
	
END
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
	FROM dbo.FindBillableTimeEntries(@cigID, @startDate, @endDate) fte
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

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.4.8.2', GETDATE(), 'RAB', 'Updatede spGenerateNewInvoiceDraft to add 14 days as standard to InvoiceDate, spGenerateInvoiceLines now uses the correct function')
GO