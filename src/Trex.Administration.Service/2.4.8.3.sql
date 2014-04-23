GO
/****** Object:  StoredProcedure [dbo].[spGetCustomersInvoiceView]    Script Date: 10/17/2012 11:55:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGetCustomersInvoiceView]
	@endDate DATETIME
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF
	
	select 
		c1.CustomerID,
		c1.CustomerName,  
		FirstDateNotInvoiced =(

			select top 1 te.StartTime from timeentries te
			inner join Tasks t on t.taskid = te.TaskID
			inner join Projects p on p.ProjectID = t.ProjectID
			inner join Customers c on c.CustomerID = p.CustomerID
			where 
				te.Billable = 1 
				and te.InvoiceId is null
				and c.CustomerID = c1.CustomerID
			Order by te.StartTime asc)

	,DistinctPrices = (
	select count(distinct te.Price) from TimeEntries te 
	inner join Tasks t on t.taskid = te.TaskID
	inner join Projects p on p.ProjectID = t.ProjectID
	inner join Customers c on c.CustomerID = p.CustomerID
	where 
		te.Billable = 1 
		and te.StartTime < @endDate
		and c.CustomerID =c1.CustomerID
	group by c.CustomerID

	)

	,InventoryValue = (
	select sum( te.Price * te.BillableTime) from TimeEntries te 
	inner join Tasks t on t.taskid = te.TaskID
	inner join Projects p on p.ProjectID = t.ProjectID
	inner join Customers c on c.CustomerID = p.CustomerID
	where 
		te.Billable = 1 
		and te.StartTime < @endDate
		and c.CustomerID =c1.CustomerID 
		and te.InvoiceId is null
	group by c.CustomerID
	),
	
	NonBillableTime =(
	  select(
		select SUM(te.TimeSpent - te.BillableTime)
		from TimeEntries te 
			inner join Tasks t on t.taskid = te.TaskID
			inner join Projects p on p.ProjectID = t.ProjectID
			inner join Customers c on c.CustomerID = p.CustomerID
		where te.Billable = 1			
			and te.StartTime < @endDate
			and c.CustomerID = c1.CustomerID 
			and te.InvoiceId is null) + 
		
		(select SUM(te.TimeSpent) 
		from TimeEntries te
			inner join Tasks t on t.taskid = te.TaskID
			inner join Projects p on p.ProjectID = t.ProjectID
			inner join Customers c on c.CustomerID = p.CustomerID 
		where te.Billable = 0
			and te.StartTime < @endDate
			and c.CustomerID = c1.CustomerID 
			and te.InvoiceId is null)
		
		from TimeEntries te
			inner join Tasks t on t.taskid = te.TaskID
			inner join Projects p on p.ProjectID = t.ProjectID
			inner join Customers c on c.CustomerID = p.CustomerID
		where
			te.Billable = 0
			and te.StartTime < @endDate
			and c.CustomerID = c1.CustomerID 
			and te.InvoiceId is null
		group by c.CustomerID
		)
	
	,Drafts = (
	CASE
	WHEN
	(SELECT SUM(
		CASE
		WHEN i.InvoiceID is null
			THEN 1
			ELSE 0
		END)
	FROM Invoices i
		INNER JOIN CustomerInvoiceGroup cig
			ON cig.CustomerInvoiceGroupID = i.CustomerInvoiceGroupId
	WHERE c1.CustomerID = cig.CustomerID
	) is null
		THEN 0
		ELSE (SELECT SUM(
				CASE
				WHEN i.InvoiceID is null
					THEN 1
					ELSE 0
				END)
			FROM Invoices i
				INNER JOIN CustomerInvoiceGroup cig
					ON cig.CustomerInvoiceGroupID = i.CustomerInvoiceGroupId
			WHERE c1.CustomerID = cig.CustomerID
			)
				
	END)

from Customers c1
ORDER BY c1.CustomerName

END
GO


INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.4.8.3', GETDATE(), 'RAB', 'Updated spGetCustomersInvoiceView to get invoices with InvoiceId == null too')
GO
