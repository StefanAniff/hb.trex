
CREATE PROCEDURE [dbo].[spGetCustomersInvoiceView]
	@startDate DATETIME,
	@endDate DATETIME
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF
	
	SELECT 
		c1.CustomerID,
		c1.CustomerName, 
		c1.internal, 
		
		FirstDateNotInvoiced = (
			SELECT TOP 1 te.StartTime
			FROM TimeEntries te
				INNER JOIN Tasks t ON t.TaskID = te.TaskID
				INNER JOIN Projects p ON p.ProjectID = t.ProjectID
				INNER JOIN Customers c ON c.CustomerID = p.CustomerID
			WHERE c.CustomerID = c1.CustomerID
			and te.Billable = 1
			and (te.DocumentType = 1 or te.DocumentType = 3)
			ORDER BY te.StartTime
			),

		--DistinctPrices = (
		--	SELECT COUNT(distinct te.Price) 
		--	FROM TimeEntries te 
		--		INNER JOIN Tasks t on t.taskid = te.TaskID
		--		INNER JOIN Projects p on p.ProjectID = t.ProjectID
		--		INNER JOIN Customers c on c.CustomerID = p.CustomerID
		--	WHERE
		--		te.Billable = 1 
		--		AND te.StartTime >= @startDate
		--		AND te.StartTime <= @endDate
		--		AND c.CustomerID = c1.CustomerID
		--		AND te.InvoiceId IS NULL
		--		AND(te.DocumentType = 1 OR te.DocumentType = 3)				
		--	GROUP BY c.CustomerID
		--	),

		InventoryValue = (
			SELECT 
				SUM(x.InventoryValue)
			FROM dbo.AggregatedTimeEntriesPrTaskPrDay(c1.CustomerID, @startDate, @endDate, 1) x
			GROUP BY
				x.Customer
			),
			
		OverduePrice = (
			SELECT 
				SUM(x.Price)
			FROM dbo.GetDebitPrice(c1.CustomerID, @startDate, @endDate, 1) x
			),
		
		NonBillableTime =(
			select SUM(te.BillableTime) from TimeEntries te
				INNER JOIN Tasks t on t.taskid = te.TaskID
				INNER JOIN Projects p on p.ProjectID = t.ProjectID
				INNER JOIN Customers c on c.CustomerID = p.CustomerID
			where te.Billable = 0
				AND te.StartTime >= @startDate
				AND te.StartTime <= @endDate
				AND c.CustomerID = c1.CustomerID
			),
		
		Drafts = (
			CASE
				WHEN(
					SELECT SUM(
						CASE
						WHEN i.InvoiceID is null
							THEN 1
							ELSE 0
						END)
					FROM Invoices i
						INNER JOIN CustomerInvoiceGroup cig
							ON cig.CustomerInvoiceGroupID = i.CustomerInvoiceGroupId
					WHERE c1.CustomerID = cig.CustomerID) IS NULL
					THEN 0
				
					ELSE 
						(SELECT SUM(
							CASE
								WHEN i.InvoiceID IS NULL
									THEN 1
									ELSE 0
							END)
						FROM Invoices i
							INNER JOIN CustomerInvoiceGroup cig
								ON cig.CustomerInvoiceGroupID = i.CustomerInvoiceGroupId
						WHERE c1.CustomerID = cig.CustomerID
						)					
				END)
		
	INTO #temp
	FROM Customers c1
		INNER JOIN CustomerInvoiceGroup cig ON c1.CustomerId = cig.CustomerId
	GROUP BY 
		c1.CustomerName,
		c1.CustomerID,
		c1.internal--,
		--i.ID
	ORDER BY 
		c1.CustomerName
			
	SELECT
		t.CustomerID,
		t.CustomerName,
		t.internal,
		--t.DistinctPrices,
		t.OverduePrice,
		t.Drafts,
		MIN(t.FirstDateNotInvoiced) AS FirstDateNotInvoiced,
		t.InventoryValue AS InventoryValue,
		t.NonBillableTime
	FROM #temp t
	GROUP BY
		t.CustomerID,
		t.CustomerName,
		t.internal,
		t.InventoryValue,
		--t.DistinctPrices,
		t.OverduePrice,
		t.Drafts,
		t.NonBillableTime
	ORDER BY 
		t.CustomerName
	
	DROP TABLE #temp

END
