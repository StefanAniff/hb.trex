--Set projects's CustomerInvoiceGroupID to each customers default project
UPDATE Projects
SET Projects.CustomerInvoiceGroupID = 
   (SELECT cig.CustomerInvoiceGroupID 
	FROM CustomerInvoiceGroup cig
	WHERE cig.CustomerID = Projects.CustomerID);


--Change Stored Procedure '[spAggregatedTimeEntriesPrTaskPrDay]'

GO
/****** Object:  StoredProcedure [dbo].[spAggregatedTimeEntriesPrTaskPrDay]    Script Date: 09/13/2012 08:51:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spAggregatedTimeEntriesPrTaskPrDay]  

@customerInvoiceGroupId INT, 
@startdate DATETIME, 
@enddate DATETIME, 
@billable BIT
AS

           --First, sum all timeentries,grouped by date, and round time to nearest quarter
           SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate, 
					--c.CustomerName AS Customer, 
					p.ProjectName AS Project, 
					t.TaskName AS Task, 
					dbo.RoundUpToNextQuarter(SUM(te.BillableTime)) AS TimeSpent, 
					t.TimeLeft AS Remaining, 
					t.TimeEstimated AS Estimate
                                                                    
           INTO #temp
           FROM dbo.TimeEntries AS te 
                INNER JOIN dbo.Tasks AS t 
					ON t.TaskID = te.TaskID  -- TEs with correct TaskID
                INNER JOIN dbo.Projects AS p 
					ON p.ProjectID = t.ProjectID -- Tasks with correct ProjectID
                INNER JOIN dbo.CustomerInvoiceGroup AS cig 
					ON cig.CustomerInvoiceGroupID = p.CustomerInvoiceGroupID -- Projects with correct CustomerInvoiceGroupId
                INNER JOIN dbo.Users AS u 
					ON u.UserID = te.UserID -- TimeEntry pr consulent
           WHERE te.StartTime >= @startdate 
			AND te.endtime <= @enddate
			AND cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
			AND te.Billable = @billable
           GROUP BY dbo.ConvertToSmallDate(te.StartTime), 
					--c.CustomerName, 
					p.ProjectName, 
					t.TaskName, 
					t.TimeLeft, 
					t.TimeEstimated

           --Then, sum the calculated timeentries, grouped by task
           SELECT   --Customer, 
					Project, 
					Task, 
					SUM(TimeSpent) AS TimeSpent, 
					Remaining, 
					Estimate
           FROM #temp
           GROUP BY --Customer, 
					Project, 
					Task, 
					Remaining, 
					Estimate

           DROP TABLE #temp


/****** Object:  StoredProcedure [dbo].[spAggregatedTimeEntriesPrTaskPrDayPrInvoice]    Script Date: 09/13/2012 09:46:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAggregatedTimeEntriesPrTaskPrDayPrInvoiceByCustomerInvoiceGroup] 
@InvoiceId INT
AS

	--First, sum all timeentries,grouped by date, and round time to nearest quarter
	SELECT  dbo.ConvertToSmallDate(te.StartTime) AS TaskDate, 
			--c.CustomerName AS Customer,
			cig.customerId,
			cig.CustomerInvoiceGroupID,
			p.ProjectName AS Project, 
			t.TaskName AS Task, 
		dbo.RoundUpToNextQuarter(SUM(te.BillableTime)) AS TimeSpent,
			i.ID AS InvoiceNumber, 
			i.InvoiceDate, 
			i.StreetAddress, 
			i.ZipCode, 
			i.City, 
			i.Country, 
			i.Attention, 
			i.StartDate, 
			i.EndDate, 
			i.DueDate,
			u.UserID
	INTO #temp
	FROM dbo.TimeEntries AS te 
		INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
		INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
		INNER JOIN dbo.CustomerInvoiceGroup AS cig ON cig.CustomerInvoiceGroupID = p.CustomerInvoiceGroupID
		INNER JOIN dbo.Invoices AS i ON te.InvoiceId = i.ID
		INNER JOIN dbo.Users AS u ON u.UserID = te.UserID
	WHERE i.ID = @InvoiceId
	
	GROUP BY dbo.ConvertToSmallDate(te.StartTime), 
			--c.CustomerName, 
			p.ProjectName, 
			t.TaskName,
			cig.customerId,
			cig.CustomerInvoiceGroupID,
			i.ID, 
			i.InvoiceDate, 
			i.StreetAddress, 
			i.ZipCode, 
			i.City, 
			i.Country, 
			i.Attention, 
			i.StartDate, 
			i.EndDate, 
			i.DueDate,
			u.UserID

	--Then, sum the calculated timeentries, grouped by task
	SELECT  --Customer AS CustomerName, 
			customerId AS CustomerNumber, 
			Project, 
			Task, 
			SUM(TimeSpent) AS TimeSpent, 
			InvoiceNumber, 
			InvoiceDate,
			StreetAddress, 
			ZipCode, 
			City,
			Country,Attention AS ContactName,
			StartDate, 
			EndDate,DueDate
	FROM       #temp
	GROUP BY --Customer, 
		CustomerId, 
		Project, 
		Task,
		InvoiceNumber, 
		InvoiceDate,
		StreetAddress, 
		ZipCode, 
		City,
		Country,
		Attention,
		StartDate, 
		EndDate,DueDate

	DROP TABLE #temp
	

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGetCustomerInvoiceView] @endDate DATETIME
AS

select c1.customerId,c1.CustomerName,  
		FirstDateNotInvoiced =(

	select top 1 te.StartTime 
	from timeentries te
		inner join Tasks t on t.taskid = te.TaskID
		inner join Projects p on p.ProjectID = t.ProjectID
		inner join Customers c on c.CustomerID = p.CustomerID
	where te.Billable = 1 
		and te.InvoiceId is null
		and c.CustomerID = c1.CustomerID
	Order by te.StartTime asc)

	,DistinctPrice = (
	select count(distinct te.Price) 
	from TimeEntries te 
		inner join Tasks t on t.taskid = te.TaskID
		inner join Projects p on p.ProjectID = t.ProjectID
		inner join Customers c on c.CustomerID = p.CustomerID
	where te.Billable = 1 
		and te.StartTime < @endDate
		and c.CustomerID =c1.CustomerID
	group by c.CustomerID
)

,InventoryValue = (
	select sum( te.Price * te.BillableTime) 
	from TimeEntries te 
		inner join Tasks t on t.taskid = te.TaskID
		inner join Projects p on p.ProjectID = t.ProjectID
		inner join Customers c on c.CustomerID = p.CustomerID
	where te.Billable = 1 
		and te.StartTime < @endDate
		and c.CustomerID = c1.CustomerID 
		and te.InvoiceId is null
	group by c.CustomerID

)

from Customers c1

SET QUOTED_IDENTIFIER OFF
