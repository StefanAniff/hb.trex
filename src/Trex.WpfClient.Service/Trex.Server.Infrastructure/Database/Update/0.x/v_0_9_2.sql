/****** Object:  StoredProcedure [dbo].[spAggregatedTimeEntriesPrTaskPrDayPrInvoice]    Script Date: 09/24/2008 14:31:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spAggregatedTimeEntriesPrTaskPrDayPrInvoice] 
@InvoiceId INT
AS

	--First, sum all timeentries,grouped by date, and round time to nearest quarter
	SELECT   dbo.ConvertToSmallDate(te.StartTime) AS TaskDate, c.CustomerName AS Customer,c.customerId ,p.ProjectName AS Project, t.TaskName AS Task, 
						  dbo.RoundUpToNextQuarter(SUM(te.TimeSpent)) AS TimeSpent,
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
	SELECT     Customer AS CustomerName, Project, Task, SUM(TimeSpent) AS TimeSpent, InvoiceNumber, InvoiceDate,customerId AS CustomerNumber, StreetAddress, ZipCode, City,Country,Attention AS ContactName,StartDate, EndDate
	FROM       #temp
	GROUP BY Customer, Project, Task,InvoiceNumber, InvoiceDate,CustomerId, StreetAddress, ZipCode, City,Country,Attention,StartDate, EndDate

	DROP TABLE #temp
	
	GO
	INSERT INTO Version (Version ,Date,Creator,Description) VALUES('0.9.2.0',GetDate(),'tga','Invoice specification')
	GO



