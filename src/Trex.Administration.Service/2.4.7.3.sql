
--Bad mapping on Projects' customerInvoiceGroupId fix
UPDATE p
SET CustomerInvoiceGroupID = cig.CustomerInvoiceGroupID
FROM CustomerInvoiceGroup cig
	INNER JOIN Customers c ON cig.CustomerID = c.CustomerID
	INNER JOIN Projects p ON c.CustomerID = p.CustomerID
WHERE 
	c.CustomerID = p.CustomerID


  
  ALTER TABLE CustomerInvoiceGroup
  DROP COLUMN InvoiceIdPrint, InvoiceIdMail, SpecificationIdMail
  GO
  


  INSERT INTO Version ([Version], [Date], [Creator], [Description])
  VALUES ('2.4.7.3', GETDATE(), 'RAB', 'Updates a bad mapping in CustomerUnvoiceGroup and remove 3 columns from it as well')
  GO