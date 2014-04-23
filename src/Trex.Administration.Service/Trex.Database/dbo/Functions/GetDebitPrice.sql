
CREATE FUNCTION [dbo].[GetDebitPrice] 
(
	@customerId INT, 
	@startdate DATETIME, 
	@enddate DATETIME, 
	@billable BIT
	
)
RETURNS TABLE 
AS
RETURN
(
	SELECT
		CASE
			WHEN (t.PricePrUnit * t.Units) IS NULL
				THEN 0
				ELSE (
				CASE
					WHEN t.UnitType = 2
						THEN SUM(t.PricePrUnit)
						ELSE SUM(t.PricePrUnit * t.Units)
				END)
		END AS Price
	FROM(
		SELECT
			il.Units,
			il.PricePrUnit,
			il.UnitType                   
			  FROM Customers c
			  INNER JOIN CustomerInvoiceGroup cig on cig.CustomerID = c.CustomerID
			  INNER JOIN Invoices i on i.CustomerInvoiceGroupId = cig.CustomerInvoiceGroupID
			  INNER JOIN InvoiceLines il on il.InvoiceID = i.ID
			  
	WHERE
	c.CustomerID = @customerId 
	AND i.InvoiceID IS NOT NULL 
	AND i.Delivered = 1 
	AND i.DueDate < GETDATE() 
	AND i.Closed = 0
		GROUP BY
		c.CustomerID,
		i.InvoiceID,
		i.Delivered,
		i.DueDate,
		il.PricePrUnit,
		il.Units,
		il.UnitType
	) t
	GROUP BY
	t.PricePrUnit,
	t.UnitType,
	t.Units
)
