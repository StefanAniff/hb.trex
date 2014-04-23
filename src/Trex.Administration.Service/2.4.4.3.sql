  
  
  ALTER TABLE Tasks
  ADD FOREIGN KEY (ProjectID)
  REFERENCES Projects(ProjectID)
  
  ALTER TABLE TimeEntries
  ADD FOREIGN KEY (InvoiceId)
  REFERENCES Invoices(ID);
  
  EXEC sp_rename 'UsersCustomers', 'UserCustomerInfo';

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Projects
	DROP CONSTRAINT FK_Projects_Users1
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT FK_Tasks_Projects
GO
ALTER TABLE dbo.Customers
	DROP CONSTRAINT FK_Customers_Users1
GO
ALTER TABLE dbo.Users SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
