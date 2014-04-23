

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [dbo].[spInvoiceDraftValidationView]    Script Date: 10/01/2012 15:45:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spInvoiceDraftValidationView]
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
		--CASE
		--	WHEN cig.Address2 is null
		--		THEN c.Address2
		--		ELSE cig.Address2
		--END AS Address2,
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

