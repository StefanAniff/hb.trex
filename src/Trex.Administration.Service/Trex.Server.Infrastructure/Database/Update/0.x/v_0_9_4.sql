set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spAggregatedTimeEntriesPrTaskPrDay] 
@customerId INT, @startdate DATETIME, @enddate DATETIME
AS

	--First, sum all timeentries,grouped by date, and round time to nearest quarter
	SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, p.ProjectName AS Project, t.TaskName AS Task, 
						  dbo.RoundUpToNextQuarter(SUM(te.TimeSpent-te.PauseTime)) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate 
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

GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spAggregatedTimeEntriesPrTaskPrDayPrInvoice] 
@InvoiceId INT
AS

	--First, sum all timeentries,grouped by date, and round time to nearest quarter
	SELECT   dbo.ConvertToSmallDate(te.StartTime) AS TaskDate, c.CustomerName AS Customer,c.customerId ,p.ProjectName AS Project, t.TaskName AS Task, 
						  dbo.RoundUpToNextQuarter(SUM(te.TimeSpent-te.PauseTime)) AS TimeSpent,
					i.ID AS InvoiceNumber, i.InvoiceDate, i.StreetAddress, i.ZipCode, i.City, i.Country, i.Attention, i.StartDate, i.EndDate
	INTO #temp
	FROM         dbo.TimeEntries AS te 
				INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
				INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
				INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
				INNER JOIN dbo.Invoices AS i ON te.InvoiceId = i.ID
	WHERE i.ID = @InvoiceId
	
	GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName,c.customerId ,i.ID, i.InvoiceDate, i.StreetAddress, i.ZipCode, i.City, i.Country, i.Attention, i.StartDate, i.EndDate

	--Then, sum the calculated timeentries, grouped by task
	SELECT     Customer AS CustomerName, Project, Task, SUM(TimeSpent) AS TimeSpent, InvoiceNumber, InvoiceDate,customerId AS CustomerNumber, StreetAddress, ZipCode, City,Country,Attention AS ContactName,StartDate, EndDate,DATEADD(dd,14,InvoiceDate) AS DeadLine
	FROM       #temp
	GROUP BY Customer, Project, Task,InvoiceNumber, InvoiceDate,CustomerId, StreetAddress, ZipCode, City,Country,Attention,StartDate, EndDate

	DROP TABLE #temp

GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spGetGeneratedInvoiceLines] @InvoiceId INT

AS

--First, sum all timeentries,grouped by date, and round time to nearest quarter
SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate,  
						  dbo.RoundUpToNextQuarter(SUM(te.TimeSpent-te.PauseTime)) AS TimeSpent, te.price ,t.taskid
INTO #temp
	FROM         dbo.TimeEntries AS te 
				INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
				INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
				INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
	WHERE  te.InvoiceID = @InvoiceId
	GROUP BY dbo.ConvertToSmallDate(te.StartTime),  te.price, t.taskid
	
--	--Then, sum the calculated timeentries, grouped by price
	SELECT     price, SUM(TimeSpent) AS TimeSpent
	FROM       #temp
	GROUP BY price

	DROP TABLE #temp



GO


INSERT INTO Version (Version ,Date,Creator,Description) VALUES('0.9.4.0',GetDate(),'tga','Bugfix. Pausetime were not subtracted from timespent')
	GO


