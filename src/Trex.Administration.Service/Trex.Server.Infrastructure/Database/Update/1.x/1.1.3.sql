/*
   7. oktober 201013:07:11
   User: 
   Server: localhost
   Database: trex_drift
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
ALTER TABLE dbo.Customers ADD
	Address2 nvarchar(400) NULL
GO
ALTER TABLE dbo.Customers SET (LOCK_ESCALATION = TABLE)

GO
ALTER TABLE dbo.Invoices ADD
	Address2 nvarchar(400) NULL
GO
ALTER TABLE dbo.Invoices SET (LOCK_ESCALATION = TABLE)
GO
COMMIT





           INSERT INTO [Version]
           ([Version]
           ,[Date]
           ,[Creator]
           ,[Description])

     VALUES
           ('1.1.3.0'
           ,GETDATE()
           ,'tga',
           'Address2 field in customer table')
GO