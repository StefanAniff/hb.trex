
    
  -- Add CustomerInvoiceGroupId to Invoice
  ALTER TABLE Invoices
  ADD CustomerInvoiceGroupId int
  
  GO

  -- Set all CustomerInvoiceGroupId to the default cig from CustomerInvoiceGroup in Invoices
  UPDATE i
	SET i.CustomerInvoiceGroupId = cig.CustomerInvoiceGroupID
	FROM Invoices i 
      inner join CustomerInvoiceGroup cig 
        on i.CustomerId = cig.CustomerID
        
  GO

  --Find all invoices without customerID
  DECLARE @InvoicesToErase TABLE (VALUE int);
  INSERT INTO @InvoicesToErase 
  SELECT i.InvoiceID
  FROM Invoices i
	  LEFT JOIN Customers c
		ON i.CustomerId = c.CustomerID
		WHERE c.CustomerID is null;
	  
  --Find all tasks about to be erased
  DECLARE @TasksToDelete Table (VALUE int)
  INSERT INTO @TasksToDelete 
	SELECT te.TaskID 
	FROM TimeEntries te
	WHERE InvoiceId = ANY(SELECT * FROM @InvoicesToErase);  

  --Find all projects about to be erased	
  DECLARE @ProjectsToDelete Table (VALUE int)
  INSERT INTO @ProjectsToDelete 
	SELECT t.ProjectID
	FROM Tasks t
	WHERE t.ProjectID = ANY(SELECT * FROM @TasksToDelete);  
  
  --Delete invoiceLines
  DELETE FROM InvoiceLines
  WHERE InvoiceID = ANY(SELECT * FROM @InvoicesToErase);  
  
  --Delete TimeEntries
  DELETE FROM TimeEntries
  WHERE InvoiceId = ANY(SELECT * FROM @InvoicesToErase);  
  
  --Delete Invoices
  DELETE FROM Invoices
  WHERE InvoiceID = ANY(SELECT * FROM @InvoicesToErase);  
  
  --Delete Tasks  
  DELETE FROM Tasks
  WHERE TaskID = ANY(SELECT * FROM @TasksToDelete);  
  
  --Delete Projects
  DELETE FROM Projects
  WHERE ProjectID = ANY(SELECT * FROM @ProjectsToDelete);  
   
   
  --Alter Invoices to drop CustomerId (Shall not know of it anymore)
  ALTER TABLE Invoices
  DROP COLUMN CustomerId 
  
  --Alter Invoices to drop ProjectdId (Was never used)
  ALTER TABLE Invoices
  DROP COLUMN ProjectId
  
  -- Link new row to CustomerInvoiceGroupId
  ALTER TABLE Invoices
  ADD FOREIGN KEY (CustomerInvoiceGroupId)
  REFERENCES CustomerInvoiceGroup(CustomerInvoiceGroupId);
  
  --Set FK to NOT NULL
  ALTER TABLE Invoices
  ALTER COLUMN CustomerInvoiceGroupId int NOT null;
  