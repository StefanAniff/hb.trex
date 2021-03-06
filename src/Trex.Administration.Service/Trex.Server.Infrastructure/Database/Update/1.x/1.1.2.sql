/*
   12. maj 201013:31:35
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
	PaymentTermsNumberOfDays int NOT NULL CONSTRAINT DF_Customers_PaymentTermsNumberOfDays DEFAULT 0,
	PaymentTermsIncludeCurrentMonth bit NOT NULL CONSTRAINT DF_Customers_PaymentTermsIncludeCurrentMonth DEFAULT 1
GO
ALTER TABLE dbo.Customers SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


           INSERT INTO [Version]
           ([Version]
           ,[Date]
           ,[Creator]
           ,[Description])

     VALUES
           ('1.2.0.0'
           ,GETDATE()
           ,'tga',
           'Payment terms in customer table')
GO
           