

ALTER TABLE Invoices
DROP COLUMN Address2
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spCreateNewInvoiceLine]
	@invoiceId int,
	@VATPercentage float
AS
BEGIN
	INSERT INTO InvoiceLines ([Text], PricePrUnit, InvoiceID, Units, UnitType, SortIndex, IsExpense, VatPercentage)
	VALUES('', 0, @invoiceId, 0, 1, 0, 0, @VATPercentage);
END
GO
