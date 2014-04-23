

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spFinalizeInvoiceDraft]
	@draftId int
AS
BEGIN
	DECLARE @LastInvoiceId int;
	
	SET @LastInvoiceId = (SELECT MAX(i.InvoiceID)
						FROM Invoices i) + 1;
						
	UPDATE Invoices
	SET InvoiceID = @LastInvoiceId
	WHERE ID = @draftId;							

END
GO
