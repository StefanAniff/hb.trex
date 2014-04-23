

CREATE PROCEDURE [dbo].[spGetGeneratedInvoiceLines] @InvoiceId INT

AS

--First, sum all timeentries,grouped by date, and round time to nearest quarter
SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate,  
						  dbo.RoundUpToNextQuarter(SUM(te.TimeSpent)) AS TimeSpent, te.price ,t.TaskID, u.UserID
INTO #temp
	FROM         dbo.TimeEntries AS te 
				INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
				INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
				INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
				INNER JOIN dbo.Users AS u ON u.UserID = te.UserID
	WHERE  te.InvoiceID = @InvoiceId and billable = 1
	GROUP BY dbo.ConvertToSmallDate(te.StartTime),  te.price, t.taskid,u.UserID
	
--	--Then, sum the calculated timeentries, grouped by price
	SELECT     price, SUM(TimeSpent) AS TimeSpent
	FROM       #temp
	GROUP BY price

	DROP TABLE #temp

