
CREATE FUNCTION [dbo].[AggregatedTimeEntriesPrTaskPrDayPrInvoice] 
(
	@InvoiceId INT,
	@FixedPrice BIT
)
RETURNS TABLE
AS
RETURN
(
	--First, sum all timeentries,grouped by date, and round time to nearest quarter
		SELECT  
		t.customerId AS CustomerId,   
		t.Customer AS CustomerName, 
		t.ProjectID,
		t.Project, 
		t.TaskID,
		t.Task, 
		SUM(t.TimeSpent) AS TimeSpent, 
		t.Price,
		t.Billable,
		t.InvoiceNumber,
		t.FixedPrice,
		t.FixedPriceProject
	FROM (	
		SELECT   
			dbo.ConvertToSmallDate(te.StartTime) AS TaskDate, 
			c.CustomerName AS Customer,
			c.customerId,
			p.ProjectName AS Project, 
			p.ProjectID,
			t.TaskName AS Task,
			t.TaskID,
			dbo.RoundUpToNextQuarter(SUM(te.TimeSpent)) AS TimeSpent,
			te.Price,
			te.Billable,
			i.ID AS InvoiceNumber,
			u.UserID,
			p.FixedPriceProject,
			p.FixedPrice
		FROM dbo.TimeEntries AS te 
			INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
			INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
			INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
			INNER JOIN dbo.Invoices AS i ON te.InvoiceId = i.ID
			INNER JOIN dbo.Users AS u ON u.UserID = te.UserID
		WHERE 
			i.ID = @InvoiceId
			AND p.FixedPriceProject = @FixedPrice
		GROUP BY 
			dbo.ConvertToSmallDate(te.StartTime), 
			c.CustomerName, 
			p.ProjectName, 
			p.ProjectID,
			t.TaskName,
			t.TaskID,
			te.Price,
			c.customerId,
			te.Billable,
			i.ID, 
			u.UserID,
			p.FixedPriceProject,
			p.FixedPrice
		) t
	GROUP BY 
		t.Customer, 
		t.ProjectID,
		t.Project, 
		t.TaskID,
		t.Task,
		t.Price,
		t.Billable,
		t.InvoiceNumber,
		t.CustomerId,
		t.FixedPrice,
		t.FixedPriceProject
)
