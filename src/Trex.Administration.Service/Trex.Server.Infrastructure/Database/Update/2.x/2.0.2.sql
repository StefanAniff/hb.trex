/*
   25. november 201119:06:18
   User: 
   Server: THOMAC
   Database: Trex
   Application: 
*/

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
ALTER TABLE dbo.Users
	DROP CONSTRAINT DF_Users_Inactive
GO
CREATE TABLE dbo.Tmp_Users
	(
	UserID int NOT NULL IDENTITY (1, 1),
	UserName nvarchar(255) NOT NULL,
	Name nvarchar(200) NULL,
	Email nvarchar(100) NULL,
	Price float(53) NOT NULL,
	Inactive bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Users SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Users ADD CONSTRAINT
	DF_Users_Inactive DEFAULT ((0)) FOR Inactive
GO
SET IDENTITY_INSERT dbo.Tmp_Users ON
GO
IF EXISTS(SELECT * FROM dbo.Users)
	 EXEC('INSERT INTO dbo.Tmp_Users (UserID, UserName, Name, Email, Price, Inactive)
		SELECT UserID, UserName, Name, Email, Price, Inactive FROM dbo.Users WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Users OFF
GO
ALTER TABLE dbo.Customers
	DROP CONSTRAINT FK_Customers_Users1
GO
ALTER TABLE dbo.TimeEntries
	DROP CONSTRAINT FK_TimeEntries_Users
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT FK_Tasks_Projects
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT FK_Tasks_Users
GO
ALTER TABLE dbo.UsersProjects
	DROP CONSTRAINT FK_UsersProjects_Users
GO
ALTER TABLE dbo.TimeEntries
	DROP CONSTRAINT FK_TimeRegs_Users
GO
ALTER TABLE dbo.UsersCustomers
	DROP CONSTRAINT FK_UsersCustomers_Users
GO
ALTER TABLE dbo.Projects
	DROP CONSTRAINT FK_Projects_Users
GO
ALTER TABLE dbo.Projects
	DROP CONSTRAINT FK_Projects_Users1
GO
ALTER TABLE dbo.Invoices
	DROP CONSTRAINT FK_Invoices_Users
GO
ALTER TABLE dbo.Customers
	DROP CONSTRAINT FK_Customers_Users
GO
DROP TABLE dbo.Users
GO
EXECUTE sp_rename N'dbo.Tmp_Users', N'Users', 'OBJECT' 
GO
ALTER TABLE dbo.Users ADD CONSTRAINT
	PK_Users PRIMARY KEY CLUSTERED 
	(
	UserID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.Users', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Invoices ADD CONSTRAINT
	FK_Invoices_Users FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Invoices SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Projects ADD CONSTRAINT
	FK_Projects_Users FOREIGN KEY
	(
	ChangedBy
	) REFERENCES dbo.Users
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Projects ADD CONSTRAINT
	FK_Projects_Users1 FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Projects SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Projects', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Projects', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Projects', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.UsersCustomers ADD CONSTRAINT
	FK_UsersCustomers_Users FOREIGN KEY
	(
	UserID
	) REFERENCES dbo.Users
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.UsersCustomers SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.UsersCustomers', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.UsersCustomers', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.UsersCustomers', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.UsersProjects ADD CONSTRAINT
	FK_UsersProjects_Users FOREIGN KEY
	(
	UserID
	) REFERENCES dbo.Users
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.UsersProjects SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.UsersProjects', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.UsersProjects', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.UsersProjects', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Tasks ADD CONSTRAINT
	FK_Tasks_Projects FOREIGN KEY
	(
	ChangedBy
	) REFERENCES dbo.Users
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Tasks ADD CONSTRAINT
	FK_Tasks_Users FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Tasks SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Tasks', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Tasks', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Tasks', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.TimeEntries ADD CONSTRAINT
	FK_TimeEntries_Users FOREIGN KEY
	(
	ChangedBy
	) REFERENCES dbo.Users
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.TimeEntries ADD CONSTRAINT
	FK_TimeRegs_Users FOREIGN KEY
	(
	UserID
	) REFERENCES dbo.Users
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.TimeEntries SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.TimeEntries', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TimeEntries', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TimeEntries', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Customers ADD CONSTRAINT
	FK_Customers_Users1 FOREIGN KEY
	(
	ChangedBy
	) REFERENCES dbo.Users
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Customers ADD CONSTRAINT
	FK_Customers_Users FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Customers SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Customers', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Customers', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Customers', 'Object', 'CONTROL') as Contr_Per 