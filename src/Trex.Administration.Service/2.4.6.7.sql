DROP PROCEDURE [dbo].[spDeleteInvoiceLine]
GO

DROP PROCEDURE [dbo].[spFinalizeInvoiceDraft]
GO

DELETE FROM CustomerInvoiceGroup
WHERE CustomerID = 5000
GO

DELETE FROM CustomersInvoiceTemplates
WHERE CustomerId = 5000
GO

DELETE FROM UserCustomerInfo
WHERE CustomerID = 5000
GO

DELETE FROM Customers
WHERE CustomerID = 5000
GO