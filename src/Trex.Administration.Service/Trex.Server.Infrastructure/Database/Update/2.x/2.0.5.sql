/*
   12. december 201109:00:25
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
ALTER TABLE dbo.TimeEntries ADD
	CreateDate DATETIME NULL,
	ClientSourceId int NOT NULL CONSTRAINT DF_TimeEntries_ClientSourceId DEFAULT 0
GO
ALTER TABLE dbo.TimeEntries SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.TimeEntries', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TimeEntries', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TimeEntries', 'Object', 'CONTROL') as Contr_Per 