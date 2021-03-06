/*
   14. december 201115:00:52
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
CREATE TABLE dbo.InvoiceTemplates
	(
	TemplateId int NOT NULL IDENTITY (1, 1),
	TemplateName nvarchar(250) NOT NULL,
	CreateDate datetime NOT NULL,
	CreatedBy nvarchar(50) NOT NULL,
	FilePath nvarchar(250) NULL,
	Guid uniqueidentifier NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.InvoiceTemplates ADD CONSTRAINT
	PK_InvoiceTemplates PRIMARY KEY CLUSTERED 
	(
	TemplateId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.InvoiceTemplates SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.InvoiceTemplates', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.InvoiceTemplates', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.InvoiceTemplates', 'Object', 'CONTROL') as Contr_Per 