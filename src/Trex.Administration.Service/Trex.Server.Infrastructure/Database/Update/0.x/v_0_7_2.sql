/*
   10. september 200812:15:43
   User: 
   Server: MICKEYMOUSE\SQLEXPRESS
   Database: Trex.Server
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
	TimeSpent float(53) NOT NULL CONSTRAINT DF_TimeEntries_TimeSpent DEFAULT 0
GO
COMMIT


UPDATE TimeEntries SET TimeSpent = CONVERT(float,DATEDIFF(ss, StartTime,EndTime))/3600


INSERT INTO Version (Version ,Date,Creator,Description) VALUES('0.7.2.0',GetDate(),'tga','TimeSpent felt i timeentries tabellen')