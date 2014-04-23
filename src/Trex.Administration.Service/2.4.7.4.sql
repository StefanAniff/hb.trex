
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

	
	SET @Duedate = (
		SELECT 
			CASE
				WHEN c.PaymentTermsIncludeCurrentMonth = 0 --Month not included
					THEN DATEADD(D, c.PaymentTermsNumberOfDays, @invoiceDate) -- Add paymentTermsNumbersOfDays to InvoiceDate
					ELSE DATEADD(D, c.PaymentTermsNumberOfDays, @FirstDayOfNextMonth) --Add paymentTermsNumbersOfDays from the next month's 1th to InvoiceDate
			END
		FROM Customers c
			INNER JOIN CustomerInvoiceGroup cig ON
				cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
		WHERE cig.CustomerID = c.CustomerID);

			
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
		@endDate, --InvoiceDate - what is it?
		@createdBy,
		@VAT,
		@startDate,
		@endDate,
		0, --Not closed
		@DueDate, 
		@customerInvoiceGroupId
		);
		
	SELECT TOP 1 *
	FROM Invoices i	
	ORDER BY i.ID DESC
	
END
GO

  INSERT INTO Version ([Version], [Date], [Creator], [Description])
  VALUES ('2.4.7.1', GETDATE(), 'RAB', 'Updated [spGenerateNewInvoiceDraft]')
  GO
