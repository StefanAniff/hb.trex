
/****** Object:  StoredProcedure [dbo].[spGetCustomerInvoiceGroupByInvoiceId]    Script Date: 09/28/2012 10:46:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGetCustomerInvoiceGroupByInvoiceId]
	@invoiceId int
AS
BEGIN
	SELECT
		cig.CustomerID,
		cig.CustomerInvoiceGroupID,
		CASE	
			WHEN cig.Label is null
				THEN 'Label'
				ELSE cig.Label 
		END AS [Label],
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
GO

