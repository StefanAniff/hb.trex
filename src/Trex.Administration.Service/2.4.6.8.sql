DROP PROCEDURE [dbo].[spGetCustomerInvoiceGroupById]
GO

DROP PROCEDURE [dbo].[spGetCustomerInvoiceGroupIdByCustomerId]
GO

DROP PROCEDURE [dbo].[spGetInvoiceLineExclVAT]
GO

DROP PROCEDURE [dbo].[spGetNewInvoiceMetaData]
GO

DROP PROCEDURE [dbo].[spUpdateInvoiceTemplateStandard]
GO

DROP PROCEDURE [dbo].[spUpdateSpecificationTemplateStandard]
GO

DROP PROCEDURE [dbo].[spGetCustomerInvoiceGroupByInvoiceId]
GO

ALTER TABLE Customers
ADD SendFormat int NOT NULL DEFAULT 1
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spInvoiceDraftValidationView]
(	
	@invoiceId int
)
AS
BEGIN
	SELECT
		CASE
			WHEN cig.Address1 is null
				THEN c.StreetAddress
				ELSE cig.Address1
		END AS Address1,
		CASE
			WHEN cig.Attention is null
				THEN c.ContactName
				ELSE cig.Attention
		END AS Attention,
		CASE
			WHEN cig.City is null
				THEN c.City
				ELSE cig.City
		END AS City,
		CASE
			WHEN cig.Country is null
				THEN c.Country
				ELSE cig.Country
		END AS Country,
		CASE
			WHEN cig.Email is null
				THEN c.Email
				ELSE cig.Email
		END AS mail,
		cig.label AS Label,
		CASE
			WHEN cig.ZipCode is null
				THEN c.ZipCode
				ELSE cig.ZipCode
		END AS ZIP,
		c.CustomerName,
		c.SendFormat,
		cit.InvoiceTemplateId,
		cit.SpecificationTemplateId,
		c.CustomerID
	FROM CustomerInvoiceGroup cig
		INNER JOIN Customers c
			ON c.CustomerID = cig.CustomerID
		INNER JOIN CustomersInvoiceTemplates cit
			ON cit.CustomerId = c.CustomerID
		INNER JOIN Invoices i
			ON i.CustomerInvoiceGroupId = cig.CustomerInvoiceGroupID
	WHERE i.ID = @invoiceId 

END

--Template for mail renaming
Update InvoiceTemplates
SET FilePath = 'd60InvoiceTemplateMail.dotx', TemplateName = 'd60 Invoice Mail Template'
WHERE FilePath LIKE '%invoicetempl%'

--template for print insert
INSERT INTO InvoiceTemplates( TemplateName, CreateDate, CreatedBy, FilePath, [Guid], [StandardInvoice])
VALUES('d60 Invoice Print Template', GETDATE(), 'systemUser', 'd60InvoiceTemplatePrint.dotx', NEWID(), 0)

