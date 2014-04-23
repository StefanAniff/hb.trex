

--New table
CREATE TABLE CustomerInvoiceGroup
(
	CustomerInvoiceGroupID	int NOT NULL  IDENTITY(1, 1),
	CustomerID	int NOT NULL,
	Attention	nvarchar(250) DEFAULT NULL,
	PONumber	nvarchar(100) DEFAULT NULL,
	City		nvarchar(100) DEFAULT NULL,
	Country		nvarchar(200) DEFAULT NULL,
	Address1	nvarchar(400) DEFAULT NULL,
	Address2	nvarchar(400) DEFAULT NULL,
	ZipCode		nvarchar(50) DEFAULT NULL,

	PRIMARY KEY(CustomerInvoiceGroupID),
	FOREIGN KEY(CustomerID) REFERENCES Customers(CustomerID)
)

GO
--New user for later use - no price and inactive
INSERT INTO Users (UserName, Price, Inactive)
	VALUES ('systemUser', 0, 1)
GO

--Find the new users ID
DECLARE @userId int;

SET @userId = 
(	SELECT TOP 1 u.UserID 
	FROM Users u
	WHERE u.UserName = 'systemUser');


--Insert a new customer called 'EmptyCustomer' linked to 'systemUser' - also inactive
--ID '5000' is available
SET IDENTITY_INSERT Customers ON
INSERT INTO Customers (CustomerID, CustomerName, Inactive, CreatedBy)
	VALUES (5000, 'EmptyCustomer', 1, @userId);
SET IDENTITY_INSERT Customers OFF

GO
--Insert link between user and customer
INSERT INTO UsersCustomers
	VALUES (@userId, 5000, 0);

GO
--Insert a group for all previous projects for 'EmptyCustomer'
SET IDENTITY_INSERT CustomerInvoiceGroup ON
INSERT INTO CustomerInvoiceGroup (CustomerInvoiceGroupID, CustomerID)
	VALUES (0, 5000);
SET IDENTITY_INSERT CustomerInvoiceGroup OFF;
GO

--Add a foreign key from Projects to CustomerInvoiceGroup
ALTER TABLE Projects
ADD CustomerInvoiceGroupID int NOT NULL default 0 REFERENCES CustomerInvoiceGroup(CustomerInvoiceGroupID)

GO
--Create a default group for each customer
INSERT INTO CustomerInvoiceGroup (CustomerID)
	SELECT c.CustomerID 
	FROM Customers c 
	WHERE c.CustomerID >= 5001;
GO