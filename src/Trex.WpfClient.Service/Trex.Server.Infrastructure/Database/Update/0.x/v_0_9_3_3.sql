ALTER TABLE dbo.Projects
	DROP CONSTRAINT FK_Projects_Customers

GO
ALTER TABLE dbo.UsersCustomers
	DROP CONSTRAINT FK_UsersCustomers_Customers
GO

ALTER TABLE dbo.Invoices
	DROP CONSTRAINT FK_Invoices_Customers


UPDATE Projects SET CustomerID = CustomerID + 5000
UPDATE UsersCustomers SET CustomerID = CustomerID + 5000
UPDATE Invoices SET CustomerID = CustomerID + 5000
UPDATE Customers SET CustomerID = CustomerID +5000

GO

