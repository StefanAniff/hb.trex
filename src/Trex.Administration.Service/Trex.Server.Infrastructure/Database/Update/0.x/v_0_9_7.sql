/*
   17. marts 200914:12:14
   User: 
   Server: MICKEYMOUSE\SQL2008
   Database: trex
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
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT FK_Tasks_Tags
GO
ALTER TABLE dbo.Tags SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Tags', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Tags', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Tags', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT FK_Tasks_Projects
GO
ALTER TABLE dbo.Projects SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Projects', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Projects', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Projects', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT FK_Tasks_Users
GO
ALTER TABLE dbo.Users SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Users', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT DF_Tasks_Guid
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT DF_Tasks_ModifyDate
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT DF_Tasks_CreateDate
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT DF_Tasks_TimeEstimated
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT DF_Tasks_Closed
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT DF_Tasks_WorstCaseEstimate
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT DF_Tasks_BestCaseEstimate
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT DF_Tasks_RealisticEstimate
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT DF_Tasks_Inactive
GO
CREATE TABLE dbo.Tmp_Tasks
	(
	TaskID int NOT NULL IDENTITY (1, 1),
	ParentID nchar(10) NULL,
	Guid uniqueidentifier NOT NULL ROWGUIDCOL,
	ProjectID int NOT NULL,
	CreatedBy int NOT NULL,
	ModifyDate datetime NOT NULL,
	CreateDate datetime NOT NULL,
	TaskName nvarchar(350) NOT NULL,
	Description nvarchar(500) NULL,
	TimeEstimated float(53) NOT NULL,
	TimeLeft int NOT NULL,
	Closed bit NOT NULL,
	WorstCaseEstimate float(53) NOT NULL,
	BestCaseEstimate float(53) NOT NULL,
	TagID int NULL,
	RealisticEstimate float(53) NOT NULL,
	Inactive bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Tasks SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Tasks ADD CONSTRAINT
	DF_Tasks_Guid DEFAULT (newid()) FOR Guid
GO
ALTER TABLE dbo.Tmp_Tasks ADD CONSTRAINT
	DF_Tasks_ModifyDate DEFAULT (getdate()) FOR ModifyDate
GO
ALTER TABLE dbo.Tmp_Tasks ADD CONSTRAINT
	DF_Tasks_CreateDate DEFAULT (getdate()) FOR CreateDate
GO
ALTER TABLE dbo.Tmp_Tasks ADD CONSTRAINT
	DF_Tasks_TimeEstimated DEFAULT ((0)) FOR TimeEstimated
GO
ALTER TABLE dbo.Tmp_Tasks ADD CONSTRAINT
	DF_Tasks_Closed DEFAULT ((0)) FOR Closed
GO
ALTER TABLE dbo.Tmp_Tasks ADD CONSTRAINT
	DF_Tasks_WorstCaseEstimate DEFAULT ((0)) FOR WorstCaseEstimate
GO
ALTER TABLE dbo.Tmp_Tasks ADD CONSTRAINT
	DF_Tasks_BestCaseEstimate DEFAULT ((0)) FOR BestCaseEstimate
GO
ALTER TABLE dbo.Tmp_Tasks ADD CONSTRAINT
	DF_Tasks_RealisticEstimate DEFAULT ((0)) FOR RealisticEstimate
GO
ALTER TABLE dbo.Tmp_Tasks ADD CONSTRAINT
	DF_Tasks_Inactive DEFAULT ((0)) FOR Inactive
GO
SET IDENTITY_INSERT dbo.Tmp_Tasks ON
GO
IF EXISTS(SELECT * FROM dbo.Tasks)
	 EXEC('INSERT INTO dbo.Tmp_Tasks (TaskID, ParentID, Guid, ProjectID, CreatedBy, ModifyDate, CreateDate, TaskName, Description, TimeEstimated, TimeLeft, Closed, WorstCaseEstimate, BestCaseEstimate, TagID, RealisticEstimate, Inactive)
		SELECT TaskID, ParentID, Guid, ProjectID, CreatedBy, ModifyDate, CreateDate, TaskName, Description, TimeEstimated, TimeLeft, Closed, WorstCaseEstimate, BestCaseEstimate, TagID, RealisticEstimate, Inactive FROM dbo.Tasks WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Tasks OFF
GO
ALTER TABLE dbo.TimeEntries
	DROP CONSTRAINT FK_Trex.Servers_Tasks
GO
DROP TABLE dbo.Tasks
GO
EXECUTE sp_rename N'dbo.Tmp_Tasks', N'Tasks', 'OBJECT' 
GO
ALTER TABLE dbo.Tasks ADD CONSTRAINT
	PK_Tasks PRIMARY KEY CLUSTERED 
	(
	TaskID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

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
ALTER TABLE dbo.Tasks ADD CONSTRAINT
	FK_Tasks_Projects FOREIGN KEY
	(
	ProjectID
	) REFERENCES dbo.Projects
	(
	ProjectID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Tasks ADD CONSTRAINT
	FK_Tasks_Tags FOREIGN KEY
	(
	TagID
	) REFERENCES dbo.Tags
	(
	TagID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Tasks', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Tasks', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Tasks', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.TimeEntries ADD CONSTRAINT
	FK_Trex.Servers_Tasks FOREIGN KEY
	(
	TaskID
	) REFERENCES dbo.Tasks
	(
	TaskID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.TimeEntries SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.TimeEntries', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TimeEntries', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TimeEntries', 'Object', 'CONTROL') as Contr_Per 


INSERT INTO Version (Version ,Date,Creator,Description) VALUES('0.9.7.0',GetDate(),'tga','Expanded task description to nvarchar(350)')
	GO