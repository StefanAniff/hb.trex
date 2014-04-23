
USE [trex]
GO
/****** Object:  StoredProcedure [dbo].[spGetCustomerInvoiceView]    Script Date: 10/02/2012 15:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetCustomersInvoiceView]
	@endDate DATETIME
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF
	
	SELECT 
		te.*,
		c.CustomerID,
		c.CustomerName
	INTO #temp
	FROM Customers c
		INNER JOIN CustomerInvoiceGroup cig ON cig.CustomerID = c.CustomerID
		INNER JOIN Invoices i ON i.CustomerInvoiceGroupId = cig.CustomerInvoiceGroupID
		INNER JOIN Projects p ON p.CustomerInvoiceGroupID = cig.CustomerInvoiceGroupID
		INNER JOIN Tasks t ON t.ProjectID = p.ProjectID
		INNER JOIN TimeEntries te ON te.TaskID = t.TaskID
	WHERE
		te.Billable = 1
		AND te.InvoiceId is null
		AND te.StartTime < @endDate
	
	SELECT
		temp.CustomerID,
		temp.CustomerName,
		FirstDateNotInvoiced = (select top 1 te.StartTime 
								from timeentries te
									inner join Tasks t on t.taskid = te.TaskID
									inner join Projects p on p.ProjectID = t.ProjectID
									inner join Customers c1 on c1.CustomerID = p.CustomerID
								where 
									te.Billable = 1 
									and te.InvoiceId is null
									and c1.CustomerID = temp.CustomerID
								Order by te.StartTime asc),
		DistinctPrices = (SELECT COUNT(distinct temp.Price)),
		InventoryValue = (SELECT SUM(temp.Price * temp.BillableTime)),
		CASE
			WHEN (SELECT COUNT(temp.InvoiceId) 
					WHERE temp.InvoiceId is null) > 0
				THEN 1
				ELSE 0
		END AS Drafts
	FROM #temp temp
	GROUP BY
		temp.CustomerID,
		temp.CustomerName,
		temp.InvoiceId
	ORDER BY
		temp.CustomerID
	
	DROP TABLE #temp
END
GO