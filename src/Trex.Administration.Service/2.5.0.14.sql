

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[AggregatedTimeEntriesPrTaskPrDay] 
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
	--First, sum all timeentries,grouped by date, and round time to nearest quarter
	SELECT     
		t.Customer, 
		t.Project, 
		t.Task, 
		SUM(t.TimeSpent * t.Price) AS InventoryValue,
		t.Remaining,
		t.StartDate
	FROM(
		SELECT   
			dbo.ConvertToSmallDate(te.StartTime) AS StartDate, 
			c.CustomerName AS Customer, 
			p.ProjectName AS Project, 
			t.TaskName AS Task, 
			dbo.RoundUpToNextQuarter(SUM(te.TimeSpent)) AS TimeSpent,
			te.Price, 
			t.TimeLeft AS Remaining                       
		FROM dbo.TimeEntries AS te 
			INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
			INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
			INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
			INNER JOIN dbo.Users AS u ON u.UserID = te.UserID
		WHERE 
			(te.StartTime) >= @startdate 
			AND (te.endtime) <= @enddate
			AND c.CustomerID = @customerId
			AND te.Billable = @billable
			--AND te.InvoiceId IS NULL
			AND (te.DocumentType = 1 OR te.DocumentType = 3)
		GROUP BY 
			dbo.ConvertToSmallDate(te.StartTime), 
			c.CustomerName, 
			p.ProjectName, 
			t.TaskName, 
			te.UserID,
			te.Price,
			t.TimeLeft
	) t
	GROUP BY 
		t.Customer, 
		t.Project, 
		t.Task, 
		t.Remaining, 
		t.StartDate
)
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.5.0.14', GETDATE(), 'RAB', 'Corrected a bug in [AggregatedTimeEntriesPrTaskPrDay]')
GO 
