/*
   19. september 200809:40:31
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
ALTER TABLE dbo.Invoices
	DROP COLUMN InvoiceDeadline
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'CONTROL') as Contr_Per 

GO
INSERT INTO Version (Version ,Date,Creator,Description) VALUES('0.9.1.1',GetDate(),'tga','Invoice modul')

GO