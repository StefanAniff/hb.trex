/*
   24. september 200816:04:22
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
ALTER TABLE dbo.Customers ADD CONSTRAINT
	FK_Customers_Customers FOREIGN KEY
	(
	CustomerID
	) REFERENCES dbo.Customers
	(
	CustomerID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Customers', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Customers', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Customers', 'Object', 'CONTROL') as Contr_Per 