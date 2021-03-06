
DELETE from InvoiceLines
 
  where InvoiceID is null
  GO

/*
   25. november 201116:46:00
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
ALTER TABLE dbo.InvoiceLines
	DROP CONSTRAINT FK_InvoiceLines_Invoices
GO
ALTER TABLE dbo.Invoices SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.InvoiceLines
	DROP CONSTRAINT DF_InvoiceLines_UnitType
GO
ALTER TABLE dbo.InvoiceLines
	DROP CONSTRAINT DF_InvoiceLines_SortIndex
GO
ALTER TABLE dbo.InvoiceLines
	DROP CONSTRAINT DF_InvoiceLines_IsExpense
GO
ALTER TABLE dbo.InvoiceLines
	DROP CONSTRAINT DF_InvoiceLines_VatPercentage
GO
CREATE TABLE dbo.Tmp_InvoiceLines
	(
	ID int NOT NULL IDENTITY (1, 1),
	Text nvarchar(1000) NOT NULL,
	PricePrUnit float(53) NOT NULL,
	InvoiceID int NOT NULL,
	Units float(53) NOT NULL,
	Unit nvarchar(50) NULL,
	UnitType int NOT NULL,
	SortIndex int NOT NULL,
	IsExpense bit NOT NULL,
	VatPercentage float(53) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_InvoiceLines SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_InvoiceLines ADD CONSTRAINT
	DF_InvoiceLines_UnitType DEFAULT ((1)) FOR UnitType
GO
ALTER TABLE dbo.Tmp_InvoiceLines ADD CONSTRAINT
	DF_InvoiceLines_SortIndex DEFAULT ((0)) FOR SortIndex
GO
ALTER TABLE dbo.Tmp_InvoiceLines ADD CONSTRAINT
	DF_InvoiceLines_IsExpense DEFAULT ((0)) FOR IsExpense
GO
ALTER TABLE dbo.Tmp_InvoiceLines ADD CONSTRAINT
	DF_InvoiceLines_VatPercentage DEFAULT ((0.25)) FOR VatPercentage
GO
SET IDENTITY_INSERT dbo.Tmp_InvoiceLines ON
GO
IF EXISTS(SELECT * FROM dbo.InvoiceLines)
	 EXEC('INSERT INTO dbo.Tmp_InvoiceLines (ID, Text, PricePrUnit, InvoiceID, Units, Unit, UnitType, SortIndex, IsExpense, VatPercentage)
		SELECT ID, Text, PricePrUnit, InvoiceID, Units, Unit, UnitType, SortIndex, IsExpense, VatPercentage FROM dbo.InvoiceLines WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_InvoiceLines OFF
GO
DROP TABLE dbo.InvoiceLines
GO
EXECUTE sp_rename N'dbo.Tmp_InvoiceLines', N'InvoiceLines', 'OBJECT' 
GO
ALTER TABLE dbo.InvoiceLines ADD CONSTRAINT
	PK_InvoiceLines PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.InvoiceLines ADD CONSTRAINT
	FK_InvoiceLines_Invoices FOREIGN KEY
	(
	InvoiceID
	) REFERENCES dbo.Invoices
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.InvoiceLines', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.InvoiceLines', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.InvoiceLines', 'Object', 'CONTROL') as Contr_Per 