

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
	
	DECLARE @wasCreditNote bit = (SELECT TOP 1 i.IsCreditNote FROM Invoices i WHERE i.ID = @invoiceId)
	
	UPDATE TimeEntries
	SET InvoiceId = NULL, 
	DocumentType = CASE
						WHEN @wasCreditNote = 1
							THEN 3
							ELSE 1
					END
	WHERE InvoiceId = @invoiceId
	
	DECLARE @linkedId int = (SELECT TOP 1 i.InvoiceLinkId FROM Invoices i WHERE i.ID = @invoiceId)
	
	UPDATE Invoices
	SET InvoiceLinkId = null
	WHERE ID = @linkedId
	
	DELETE FROM Invoices
	WHERE ID = @invoiceId
END
