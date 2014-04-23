ALTER TABLE dbo.Invoices
ADD InvoiceID int NULL DEFAULT NULL;
GO

UPDATE dbo.Invoices
SET Invoices.InvoiceID = ID
GO

--New table
CREATE TABLE CustomerInvoiceGroup
(
	CustomerInvoiceGroupID int NOT NULL IDENTITY(1, 1),
	CustomerID	int NOT NULL,
	Label		nvarchar(250) DEFAULT '' NOT NULL,
	Attention	nvarchar(250) DEFAULT NULL,
	City		nvarchar(100) DEFAULT NULL,
	Country		nvarchar(200) DEFAULT NULL,
	Address1	nvarchar(400) DEFAULT NULL,
	Address2	nvarchar(400) DEFAULT NULL,
	ZipCode		nvarchar(50)  DEFAULT NULL,
	Email		nvarchar(200) DEFAULT NULL,
	InvoiceTemplateIdPrint int DEFAULT NULL,
	InvoiceTemplateIdMail int DEFAULT NULL,
	SpecificationTemplateIdMail int DEFAULT NULL,

	PRIMARY KEY(CustomerInvoiceGroupID),
	FOREIGN KEY(CustomerID) REFERENCES Customers(CustomerID)
)

--Create a default group for each customer
INSERT INTO CustomerInvoiceGroup (CustomerID)
	SELECT c.CustomerID 
	FROM Customers c;
GO

--Add a foreign key from Projects to CustomerInvoiceGroup
ALTER TABLE Projects
ADD CustomerInvoiceGroupID int NOT NULL DEFAULT 0
GO

--Add foreign key value
UPDATE Projects
SET CustomerInvoiceGroupID = (SELECT TOP 1 cig.CustomerInvoiceGroupID
							FROM CustomerInvoiceGroup cig 
							WHERE cig.CustomerID = CustomerID)

--Add foreign key
ALTER TABLE Projects
ADD FOREIGN KEY (CustomerInvoiceGroupID) 
REFERENCES CustomerInvoiceGroup(CustomerInvoiceGroupID)

-- Add CustomerInvoiceGroupId to Invoice
ALTER TABLE Invoices
ADD CustomerInvoiceGroupId int

GO
  
-- Set all CustomerInvoiceGroupId to the default cig from CustomerInvoiceGroup in Invoices
UPDATE i
SET i.CustomerInvoiceGroupId = cig.CustomerInvoiceGroupID
FROM Invoices i 
  INNER JOIN CustomerInvoiceGroup cig 
    ON i.CustomerId = cig.CustomerID
    
GO


	--Move invoice from deleted customer to real customer
	UPDATE Invoices
	SET CustomerInvoiceGroupId = (select top 1 cig.CustomerInvoiceGroupID
									FROM CustomerInvoiceGroup cig
									WHERE cig.CustomerID = 5055)
	WHERE CustomerId = 5057
	GO
   
  --Alter Invoices to drop CustomerId (Shall not know of it anymore)
  ALTER TABLE Invoices
  DROP COLUMN CustomerId 
  GO
    
  -- Link new row to CustomerInvoiceGroupId
  ALTER TABLE Invoices
  ADD FOREIGN KEY (CustomerInvoiceGroupId)
  REFERENCES CustomerInvoiceGroup(CustomerInvoiceGroupId);
  GO
  
  --Set FK to NOT NULL
  ALTER TABLE Invoices
  ALTER COLUMN CustomerInvoiceGroupId int NOT null;
  GO


  INSERT INTO Version ([Version], [Date], [Creator], [Description])
  VALUES ('2.4.7.1', GETDATE(), 'RAB', 'Create coloumn in Invoices to use for actual invoiceId, Create CustomerInvoiceGroup and set FK from Customers and FK to Customers and Invoices, removed old FK from Invoice to Customer')
  GO