/*
   25. november 201117:42:45
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
ALTER TABLE dbo.Projects
	DROP CONSTRAINT FK_Projects_Customers
GO
ALTER TABLE dbo.Customers SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Customers', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Customers', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Customers', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Projects
	DROP CONSTRAINT FK_Projects_Users
GO
ALTER TABLE dbo.Projects
	DROP CONSTRAINT FK_Projects_Users1
GO
ALTER TABLE dbo.Users SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Users', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Projects
	DROP CONSTRAINT DF_Projects_Guid
GO
ALTER TABLE dbo.Projects
	DROP CONSTRAINT DF_Projects_CreateDate
GO
ALTER TABLE dbo.Projects
	DROP CONSTRAINT DF_Projects_Inactive
GO
ALTER TABLE dbo.Projects
	DROP CONSTRAINT DF_Projects_IsEstimatesEnabled
GO
CREATE TABLE dbo.Tmp_Projects
	(
	ProjectID int NOT NULL IDENTITY (1, 1),
	Guid uniqueidentifier NOT NULL ROWGUIDCOL,
	CustomerID int NOT NULL,
	ProjectName nvarchar(50) NOT NULL,
	CreatedBy int NOT NULL,
	CreateDate datetime NOT NULL,
	Inactive bit NOT NULL,
	IsEstimatesEnabled bit NOT NULL,
	ChangeDate datetime NULL,
	ChangedBy int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Projects SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Projects ADD CONSTRAINT
	DF_Projects_Guid DEFAULT (newid()) FOR Guid
GO
ALTER TABLE dbo.Tmp_Projects ADD CONSTRAINT
	DF_Projects_CreateDate DEFAULT (getdate()) FOR CreateDate
GO
ALTER TABLE dbo.Tmp_Projects ADD CONSTRAINT
	DF_Projects_Inactive DEFAULT ((0)) FOR Inactive
GO
ALTER TABLE dbo.Tmp_Projects ADD CONSTRAINT
	DF_Projects_IsEstimatesEnabled DEFAULT ((0)) FOR IsEstimatesEnabled
GO
SET IDENTITY_INSERT dbo.Tmp_Projects ON
GO
IF EXISTS(SELECT * FROM dbo.Projects)
	 EXEC('INSERT INTO dbo.Tmp_Projects (ProjectID, Guid, CustomerID, ProjectName, CreatedBy, CreateDate, Inactive, IsEstimatesEnabled, ChangeDate, ChangedBy)
		SELECT ProjectID, Guid, CustomerID, ProjectName, CreatedBy, CreateDate, Inactive, IsEstimatesEnabled, ChangeDate, ChangedBy FROM dbo.Projects WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Projects OFF
GO
ALTER TABLE dbo.UsersProjects
	DROP CONSTRAINT FK_UsersProjects_Projects
GO
DROP TABLE dbo.Projects
GO
EXECUTE sp_rename N'dbo.Tmp_Projects', N'Projects', 'OBJECT' 
GO
ALTER TABLE dbo.Projects ADD CONSTRAINT
	PK_Projects PRIMARY KEY CLUSTERED 
	(
	ProjectID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

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
ALTER TABLE dbo.Projects ADD CONSTRAINT
	FK_Projects_Customers FOREIGN KEY
	(
	CustomerID
	) REFERENCES dbo.Customers
	(
	CustomerID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Projects', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Projects', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Projects', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.UsersProjects ADD CONSTRAINT
	FK_UsersProjects_Projects FOREIGN KEY
	(
	ProjectID
	) REFERENCES dbo.Projects
	(
	ProjectID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.UsersProjects SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.UsersProjects', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.UsersProjects', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.UsersProjects', 'Object', 'CONTROL') as Contr_Per 