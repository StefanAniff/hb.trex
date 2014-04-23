

GO
/****** Object:  StoredProcedure [dbo].[spAggregatedTimeEntriesPrTaskPrDayByCustomerInvoiceGroup]    Script Date: 09/14/2012 12:45:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Stored Procedure to find all relevant data about TimeEntries 
-- - within a certain periode
-- - is billable
-- - belonging to a certain CustomerInvoiceGroup

CREATE PROCEDURE [dbo].[spFindTimeEntiresForInvoice] 
@customerInvoiceGroupId INT, 
@startdate DATETIME, 
@enddate DATETIME
AS
BEGIN
   --First, sum all timeentries,grouped by date, and round time to nearest quarter
   SELECT   
		dbo.RoundUpToNextQuarter(te.BillableTime) AS TimeSpent, 
		dbo.ConvertToSmallDate(te.StartTime) AS StartDate, 
		dbo.ConvertToSmallDate(te.EndTime) AS EndDate,
		p.ProjectName AS Project, 
		p.ProjectID AS [Project ID],
		t.TaskName AS Task, 
		t.TaskID AS [Task ID],
		te.Price AS [Price pr Hour],
		te.InvoiceId AS InvoiceID,
		p.CustomerInvoiceGroupID AS [CIG ID],
		p.CustomerID AS [Customer ID],
		cig.Label AS GroupName,
		te.TimeEntryID AS [TimeEntry ID]
                                                        
   INTO #temp
   FROM 
		dbo.TimeEntries AS te 
        INNER JOIN dbo.Tasks AS t 
			ON t.TaskID = te.TaskID 
        INNER JOIN dbo.Projects AS p 
			ON p.ProjectID = t.ProjectID 
        INNER JOIN dbo.CustomerInvoiceGroup AS cig 
			ON cig.CustomerInvoiceGroupID = p.CustomerInvoiceGroupID 
        INNER JOIN dbo.Users AS u 
			ON u.UserID = te.UserID 
   WHERE 
		te.StartTime >= @startdate 
		AND te.endtime <= @enddate
		AND cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
		AND te.Billable = 1

   --Then, sum the calculated timeentries, grouped by task
   SELECT   
		t.[TimeEntry ID],
		t.GroupName,
		t.[CIG ID],
		t.StartDate, 
		t.EndDate,
		t.Project, 
		t.[Project ID],
		t.Task,
		t.[Task ID],
		t.TimeSpent,
		t.[Price pr Hour],
		t.InvoiceID,
		t.[Customer ID]
   FROM #temp t

   DROP TABLE #temp
   
END

SET ANSI_NULLS ON


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spGetNewInvoiceMetaData]
@CustomerInvoiceGroup int

AS
BEGIN

	SELECT 
		CASE
			WHEN cig.Address1 is null
				THEN c.StreetAddress
				ELSE cig.Address1
		END AS Address1,
		
		CASE
			WHEN cig.Address2 is null
				THEN c.Address2
				ELSE cig.Address2
		END AS Address2,
		
		CASE
			WHEN cig.Attention is null
				THEN c.ContactName
				ELSE cig.Attention
		END AS Attention,
		
		CASE
			WHEN cig.City is null
				THEN c.City
				ELSE cig.City
		END AS City,
		
		CASE
			WHEN cig.Country is null
				THEN c.Country
				ELSE cig.Country
		END AS Country,
		
		cig.CustomerID,
		
		CASE
			WHEN cig.ZipCode is null
				THEN c.ZipCode
				ELSE cig.ZipCode
		END AS ZipCode
		
	FROM CustomerInvoiceGroup cig
		INNER JOIN Customers c	
			ON c.CustomerID = cig.CustomerID
	WHERE cig.CustomerInvoiceGroupID = @CustomerInvoiceGroup

END
