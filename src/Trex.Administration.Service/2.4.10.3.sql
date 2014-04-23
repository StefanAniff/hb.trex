
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
	
	IF @endDate > @today
	BEGIN
		SET @invoiceDate = @today
	END
	ELSE
	BEGIN
		SET @invoiceDate = @endDate
	END
	
	INSERT INTO Invoices (
		CreateDate, 
		InvoiceDate, 
		CreatedBy, 
		VAT, 
		StartDate, 
		EndDate, 
		Closed, 
		DueDate, 
		CustomerInvoiceGroupId,
		Guid)
	VALUES 
		(
		GETDATE(),
		@invoiceDate, 
		@createdBy,
		@VAT,
		@startDate,
		@endDate,
		0, --Not closed
		DATEADD(D, 14, @invoiceDate), 
		@customerInvoiceGroupId,
		NEWID());
		
	SELECT TOP 1 *
	FROM Invoices i	
	ORDER BY i.ID DESC
	
END
SET ANSI_NULLS ON
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.4.10.3', GETDATE(), 'RAB', 'Added new Guid to generation of drafts')
GO