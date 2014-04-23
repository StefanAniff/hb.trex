
CREATE FUNCTION [dbo].[AggregatedCreditNotesPrTaskPrDayPrInvoice] 
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
			dbo.ConvertToSmallDate(cn.StartTime) AS TaskDate, 
			c.CustomerName AS Customer,
			c.customerId,
			p.ProjectName AS Project, 
			p.ProjectID,
			t.TaskName AS Task,
			t.TaskID,
			dbo.RoundUpToNextQuarter(SUM(cn.TimeSpent)) AS TimeSpent,
			cn.Price,
			cn.Billable,
			i.ID AS InvoiceNumber,
			u.UserID,
			p.FixedPriceProject,
			p.FixedPrice
		FROM dbo.CreditNote AS cn 
			INNER JOIN dbo.Tasks AS t ON t.TaskID = cn.TaskID 
			INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
			INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
			INNER JOIN dbo.Invoices AS i ON cn.InvoiceId = i.ID
			INNER JOIN dbo.Users AS u ON u.UserID = cn.UserID
		WHERE 
			i.ID = @InvoiceId
		GROUP BY 
			dbo.ConvertToSmallDate(cn.StartTime), 
			c.CustomerName, 
			p.ProjectName, 
			p.ProjectID,
			t.TaskName,
			t.TaskID,
			cn.Price,
			c.customerId,
			cn.Billable,
			i.ID, 
			u.UserID,
			p.FixedPriceProject,
			p.FixedPrice
		) t
	WHERE t.FixedPriceProject = @FixedPrice
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
