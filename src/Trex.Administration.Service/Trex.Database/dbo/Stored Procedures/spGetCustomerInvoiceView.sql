

CREATE PROCEDURE [dbo].[spGetCustomerInvoiceView] @endDate DATETIME
AS

select c1.customerId,c1.CustomerName,  FirstDateNotInvoiced =(

select top 1 te.StartTime from timeentries te
inner join Tasks t on t.taskid = te.TaskID
inner join Projects p on p.ProjectID = t.ProjectID
inner join Customers c on c.CustomerID = p.CustomerID
where te.Billable = 1 and te.InvoiceId is null
and c.CustomerID = c1.CustomerID
Order by te.StartTime asc)

,DistinctPrice = (
select count(distinct te.Price) from TimeEntries te 
inner join Tasks t on t.taskid = te.TaskID
inner join Projects p on p.ProjectID = t.ProjectID
inner join Customers c on c.CustomerID = p.CustomerID
where te.Billable = 1 and te.StartTime < @endDate
and c.CustomerID =c1.CustomerID
group by c.CustomerID

)

,InventoryValue = (
select sum( te.Price * te.BillableTime) from TimeEntries te 
inner join Tasks t on t.taskid = te.TaskID
inner join Projects p on p.ProjectID = t.ProjectID
inner join Customers c on c.CustomerID = p.CustomerID
where te.Billable = 1 and te.StartTime < @endDate
and c.CustomerID =c1.CustomerID and te.InvoiceId is null
group by c.CustomerID

)


from Customers c1


