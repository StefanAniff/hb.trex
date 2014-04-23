
ALTER TABLE dbo.Invoices
ADD InvoiceID int NULL DEFAULT NULL;
GO

UPDATE dbo.Invoices
SET Invoices.InvoiceID = ID
GO

