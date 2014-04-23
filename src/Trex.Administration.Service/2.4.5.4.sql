

CREATE TABLE CustomersInvoiceTemplates
(	
	CustomerId int NOT NULL FOREIGN KEY REFERENCES Customers(CustomerId),
	InvoiceTemplateId int NOT NULL FOREIGN KEY REFERENCES InvoiceTemplates(TemplateId),
	SpecificationTemplateId int NOT NULL FOREIGN KEY REFERENCES InvoiceTemplates(TemplateId)	
)
GO


ALTER TABLE InvoiceTemplates
ADD StandardInvoice BIT NOT NULL DEFAULT 0

ALTER TABLE InvoiceTemplates
ADD StandardSpecification BIT NOT NULL DEFAULT 0
GO

-- Lav "d60InvoiceTemplate.dotx"
-- Lav "d60SpecificationTemplate.dotx"
-- Upload til server MANUELT

DECLARE @invoice varchar(50);
DECLARE @specification varchar(50);
SET @invoice = 'd60 Standard Invoice Template';
SET @specification = 'd60 Standard Specification Template';



INSERT INTO InvoiceTemplates( TemplateName, CreateDate, CreatedBy, FilePath, [Guid], [StandardInvoice])
VALUES(@invoice, GETDATE(), 'systemUser', 'd60InvoiceTemplate.dotx', NEWID(), 1)
INSERT INTO InvoiceTemplates( TemplateName, CreateDate, CreatedBy, FilePath, [Guid], [StandardSpecification])
VALUES(@specification, GETDATE(), 'systemUser', 'd60SpecificationTemplate.dotx', NEWID(), 1)

DECLARE @invoiceID int;
DECLARE @specID int;

SET @invoiceID = (SELECT TOP 1 it.TemplateId
				FROM InvoiceTemplates it
				WHERE it.StandardInvoice = 1)

SET @specID = (SELECT TOP 1 it.TemplateId
				FROM InvoiceTemplates it
				WHERE it.StandardSpecification = 1)
				
INSERT INTO CustomersInvoiceTemplates (CustomerId, InvoiceTemplateId, SpecificationTemplateId)
SELECT
	c.CustomerID,
	@invoiceID,
	@specID	
FROM Customers c





--InvoiceStandard update

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spUpdateInvoiceTemplateStandard] 
	@templateId int
AS 
BEGIN
	UPDATE InvoiceTemplates
	SET StandardInvoice = 0
	WHERE StandardInvoice = 1;
	
	UPDATE InvoiceTemplates
	SET StandardInvoice = 1
	WHERE TemplateId = @templateId;
END
GO

--SpecificationStandard update

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spUpdateSpecificationTemplateStandard] 
	@templateId int
AS 
BEGIN
	UPDATE InvoiceTemplates
	SET StandardSpecification = 0
	WHERE StandardSpecification = 1;
	
	UPDATE InvoiceTemplates
	SET StandardSpecification = 1
	WHERE TemplateId = @templateId;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetCustomerInvoiceGroupById]
	@customerInvoiceGroup int
AS
BEGIN
	SELECT *
	FROM CustomerInvoiceGroup cig
	WHERE cig.CustomerInvoiceGroupID = @customerInvoiceGroup
END
GO



UPDATE CustomerInvoiceGroup
SET Label = 'Label'
WHERE Label is null

ALTER TABLE CustomerInvoiceGroup
ALTER COLUMN Label nvarchar(200) NOT NULL 



ALTER TABLE CustomerInvoiceGroup
ADD DEFAULT 'Label' for  Label 



ALTER TABLE CustomerInvoiceGroup
ADD InvoiceTemplateId int

ALTER TABLE CustomerInvoiceGroup
ADD SpecificationTemplateID int


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetCustomerInvoiceGroupByInvoiceId]
	@invoiceId int
AS
BEGIN
	SELECT
		cig.CustomerID,
		cig.CustomerInvoiceGroupID,
		cig.Label,
		CASE
			WHEN cig.InvoiceTemplateId is null	
				THEN cit.InvoiceTemplateId
				ELSE cig.InvoiceTemplateId
		END AS [InvoiceTemplateId],
		CASE
			WHEN cig.SpecificationTemplateID is null	
				THEN cit.SpecificationTemplateId
				ELSE cig.SpecificationTemplateID
		END AS [SpecificationTemplateId]
	FROM CustomerInvoiceGroup cig
		INNER JOIN CustomersInvoiceTemplates cit
			ON cit.CustomerID = cig.CustomerID
		INNER JOIN Invoices i
			ON i.CustomerInvoiceGroupId = cig.CustomerInvoiceGroupID
	WHERE i.ID = @invoiceId
	
END
