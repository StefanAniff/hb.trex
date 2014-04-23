
CREATE PROCEDURE spAggregatedTimeEntriesPrTaskPrDay 
@customerId INT, @startdate DATETIME, @enddate DATETIME
AS

	--First, sum all timeentries,grouped by date, and round time to nearest quarter
	SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, p.ProjectName AS Project, t.TaskName AS Task, 
						  dbo.RoundUpToNextQuarter(SUM(te.TimeSpent)) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate 
	INTO #temp
	FROM         dbo.TimeEntries AS te 
				INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
				INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
				INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
	WHERE te.StartTime >= @startdate AND te.endtime <= @enddate
	AND c.CustomerID = @customerId
	GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName, t.TimeLeft, t.TimeEstimated

	--Then, sum the calculated timeentries, grouped by task
	SELECT     Customer, Project, Task, SUM(TimeSpent) AS TimeSpent, Remaining, Estimate
	FROM       #temp
	GROUP BY Customer, Project, Task, Remaining, Estimate

	DROP TABLE #temp



INSERT INTO Version (Version ,Date,Creator,Description) VALUES('0.7.3.0',GetDate(),'tga','Stored proc: spAggregatedTimeEntriesPrTaskPrDay. Til rapportering')