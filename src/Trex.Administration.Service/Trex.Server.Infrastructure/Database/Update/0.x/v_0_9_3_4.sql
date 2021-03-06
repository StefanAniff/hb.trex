/*
   24. september 200815:57:30
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
ALTER TABLE dbo.Customers
	DROP CONSTRAINT FK_Customers_Users
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Users', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Customers
	DROP CONSTRAINT DF_Customers_Guid
GO
ALTER TABLE dbo.Customers
	DROP CONSTRAINT DF_Customers_CreateDate
GO
ALTER TABLE dbo.Customers
	DROP CONSTRAINT DF_Customers_Inactive
GO
CREATE TABLE dbo.Tmp_Customers
	(
	CustomerID int NOT NULL IDENTITY (1, 1),
	Guid uniqueidentifier NOT NULL ROWGUIDCOL,
	CreateDate datetime NOT NULL,
	CreatedBy int NOT NULL,
	CustomerName nvarchar(250) NOT NULL,
	PhoneNumber nvarchar(50) NULL,
	Email nvarchar(255) NULL,
	Inactive bit NOT NULL,
	StreetAddress nvarchar(400) NULL,
	ZipCode nvarchar(50) NULL,
	City nvarchar(100) NULL,
	Country nvarchar(200) NULL,
	ContactName nvarchar(200) NULL,
	ContactPhone nvarchar(100) NULL,
	CustomerNumber int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Customers ADD CONSTRAINT
	DF_Customers_Guid DEFAULT (newid()) FOR Guid
GO
ALTER TABLE dbo.Tmp_Customers ADD CONSTRAINT
	DF_Customers_CreateDate DEFAULT (getdate()) FOR CreateDate
GO
ALTER TABLE dbo.Tmp_Customers ADD CONSTRAINT
	DF_Customers_Inactive DEFAULT ((0)) FOR Inactive
GO
SET IDENTITY_INSERT dbo.Tmp_Customers ON
GO
IF EXISTS(SELECT * FROM dbo.Customers)
	 EXEC('INSERT INTO dbo.Tmp_Customers (CustomerID, Guid, CreateDate, CreatedBy, CustomerName, PhoneNumber, Email, Inactive, StreetAddress, ZipCode, City, Country, ContactName, ContactPhone, CustomerNumber)
		SELECT CustomerID, Guid, CreateDate, CreatedBy, CustomerName, PhoneNumber, Email, Inactive, StreetAddress, ZipCode, City, Country, ContactName, ContactPhone, CustomerNumber FROM dbo.Customers WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Customers OFF
GO
ALTER TABLE dbo.Tags
	DROP CONSTRAINT FK_Tags_Customers
GO
DROP TABLE dbo.Customers
GO
EXECUTE sp_rename N'dbo.Tmp_Customers', N'Customers', 'OBJECT' 
GO
ALTER TABLE dbo.Customers ADD CONSTRAINT
	PK_Customers PRIMARY KEY CLUSTERED 
	(
	CustomerID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Customers ADD CONSTRAINT
	FK_Customers_Users FOREIGN KEY
	(
	CreatedBy
	) REFERENCES dbo.Users
	(
	UserID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Customers', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Customers', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Customers', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Tags ADD CONSTRAINT
	FK_Tags_Customers FOREIGN KEY
	(
	CustomerID
	) REFERENCES dbo.Customers
	(
	CustomerID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Tags', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Tags', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Tags', 'Object', 'CONTROL') as Contr_Per 