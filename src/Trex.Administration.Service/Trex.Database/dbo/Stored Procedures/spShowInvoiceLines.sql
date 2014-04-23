CREATE PROCEDURE [dbo].[spShowInvoiceLines]
(
	@invoiceId int
)
AS
BEGIN
	
	SELECT
		il.PricePrUnit,
		il.Units,
		il.Unit,
		il.IsExpense,
		il.[Text],
		il.VatPercentage		
	FROM  InvoiceLines il
	WHERE il.InvoiceID = @invoiceId
	ORDER BY il.PricePrUnit	

END
