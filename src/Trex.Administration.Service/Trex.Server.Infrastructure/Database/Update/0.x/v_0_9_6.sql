/*
   8. december 200822:47:59
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
ALTER TABLE dbo.TimeEntries ADD
	Guid uniqueidentifier NOT NULL ROWGUIDCOL CONSTRAINT DF_TimeEntries_Guid DEFAULT (newid())
GO
ALTER TABLE dbo.TimeEntries SET (LOCK_ESCALATION = TABLE)
GO


INSERT INTO Version (Version ,Date,Creator,Description) VALUES('0.9.6.0',GetDate(),'tga','Row guid in timeentry table')
	GO


COMMIT

