/*
   19. september 200809:38:05
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
	DROP CONSTRAINT FK_Invoices_Customers
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Customers', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Customers', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Customers', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Invoices
	DROP CONSTRAINT FK_Invoices_Users
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Users', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Invoices
	(
	ID int NOT NULL IDENTITY (1, 1),
	CreateDate datetime NOT NULL,
	InvoiceDate datetime NOT NULL,
	CustomerId int NOT NULL,
	CreatedBy int NOT NULL,
	CustomerName nvarchar(200) NULL,
	StreetAddress nvarchar(400) NULL,
	ZipCode nvarchar(50) NULL,
	City nvarchar(100) NULL,
	Country nvarchar(200) NULL,
	InvoiceDeadline datetime NULL,
	Attention nvarchar(300) NULL,
	VAT float(53) NOT NULL,
	FooterText nvarchar(1000) NULL,
	StartDate datetime NOT NULL,
	EndDate datetime NOT NULL,
	Closed bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Invoices ADD CONSTRAINT
	DF_Invoices_InvoiceDate DEFAULT GetDate() FOR InvoiceDate
GO
SET IDENTITY_INSERT dbo.Tmp_Invoices ON
GO
IF EXISTS(SELECT * FROM dbo.Invoices)
	 EXEC('INSERT INTO dbo.Tmp_Invoices (ID, CreateDate, CustomerId, CreatedBy, CustomerName, StreetAddress, ZipCode, City, Country, InvoiceDeadline, Attention, VAT, FooterText, StartDate, EndDate, Closed)
		SELECT ID, CreateDate, CustomerId, CreatedBy, CustomerName, StreetAddress, ZipCode, City, Country, InvoiceDeadline, Attention, VAT, FooterText, StartDate, EndDate, Closed FROM dbo.Invoices WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Invoices OFF
GO
ALTER TABLE dbo.InvoiceLines
	DROP CONSTRAINT FK_InvoiceLines_Invoices
GO
DROP TABLE dbo.Invoices
GO
EXECUTE sp_rename N'dbo.Tmp_Invoices', N'Invoices', 'OBJECT' 
GO
ALTER TABLE dbo.Invoices ADD CONSTRAINT
	PK_Invoices PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Invoices ADD CONSTRAINT
	FK_Invoices_Users FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Invoices ADD CONSTRAINT
	FK_Invoices_Customers FOREIGN KEY
	(
	CustomerId
	) REFERENCES dbo.Customers
	(
	CustomerID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
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