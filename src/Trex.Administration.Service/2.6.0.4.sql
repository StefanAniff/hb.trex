
ALTER PROCEDURE [dbo].[spGetCustomersInvoiceView]
	@startDate DATETIME,
	@endDate DATETIME
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF
	
	SELECT 
		c1.CustomerID,
		c1.CustomerName,  
		
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

		DistinctPrices = (
			SELECT COUNT(distinct te.Price) 
			FROM TimeEntries te 
				INNER JOIN Tasks t on t.taskid = te.TaskID
				INNER JOIN Projects p on p.ProjectID = t.ProjectID
				INNER JOIN Customers c on c.CustomerID = p.CustomerID
			WHERE
				te.Billable = 1 
				AND te.StartTime >= @startDate
				AND te.StartTime <= @endDate
				AND c.CustomerID = c1.CustomerID
				AND te.InvoiceId IS NULL
				AND(te.DocumentType = 1 OR te.DocumentType = 3)				
			GROUP BY c.CustomerID
			),

		InventoryValue = (
			SELECT 
				SUM(x.InventoryValue)
			FROM dbo.AggregatedTimeEntriesPrTaskPrDay(c1.CustomerID, @startDate, @endDate, 1) x
			GROUP BY
				x.Customer
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
		c1.CustomerID--,
		--i.ID
	ORDER BY 
		c1.CustomerName
	
	
	SELECT
		t.CustomerID,
		t.CustomerName,
		t.DistinctPrices,
		t.Drafts,
		MIN(t.FirstDateNotInvoiced) AS FirstDateNotInvoiced,
		t.InventoryValue AS InventoryValue,
		t.NonBillableTime
	FROM #temp t
	GROUP BY
		t.CustomerID,
		t.CustomerName,
		t.InventoryValue,
		t.DistinctPrices,
		t.Drafts,
		t.NonBillableTime
	ORDER BY 
		t.CustomerName
	
	DROP TABLE #temp

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGenerateNewInvoiceDraft]
@customerInvoiceGroupId int,
@createdBy int,
@VAT float,
@startDate DATETIME,
@endDate DATETIME
AS
BEGIN

	DECLARE @Duedate DATETIME;
	DECLARE @invoiceDate DATETIME;
	DECLARE @today DATETIME;
	
	SET @today = GETDATE();
	
	IF @endDate > @today
	BEGIN
		SET @invoiceDate = @today
	END
	ELSE
	BEGIN
		SET @invoiceDate = @endDate
	END
	
	INSERT INTO Invoices (
		CreateDate, 
		InvoiceDate, 
		CreatedBy, 
		VAT, 
		StartDate, 
		EndDate, 
		Closed, 
		DueDate, 
		CustomerInvoiceGroupId,
		Guid,
		Attention)
	VALUES 
		(
		GETDATE(),
		@invoiceDate, 
		@createdBy,
		@VAT,
		@startDate,
		@endDate,
		0, --Not closed
		DATEADD(D, 14, @invoiceDate), 
		@customerInvoiceGroupId,
		NEWID(),
		(SELECT cig.Attention FROM CustomerInvoiceGroup cig
			WHERE cig.CustomerInvoiceGroupID = @customerInvoiceGroupId));
		
	SELECT TOP 1 *
	FROM Invoices i	
	ORDER BY i.ID DESC
	
END
SET ANSI_NULLS ON
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[spGetInvoices]
	@OrderList varchar(1000)
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF

	CREATE TABLE #TempList
	(
		CustomerId int
	)

	DECLARE @OrderID varchar(10), @Pos int

	SET @OrderList = LTRIM(RTRIM(@OrderList))+ ','
	SET @Pos = CHARINDEX(',', @OrderList, 1)

	IF REPLACE(@OrderList, ',', '') <> ''
	BEGIN
		WHILE @Pos > 0
		BEGIN
			SET @OrderID = LTRIM(RTRIM(LEFT(@OrderList, @Pos - 1)))
			IF @OrderID <> ''
			BEGIN
				INSERT INTO #TempList (CustomerId) VALUES (CAST(@OrderID AS int)) --Use Appropriate conversion
			END
			SET @OrderList = RIGHT(@OrderList, LEN(@OrderList) - @Pos)
			SET @Pos = CHARINDEX(',', @OrderList, 1)

		END
	END	

	SELECT 
		i.ID,
		i.InvoiceID,
		c.CustomerName,
		i.[Guid],
		i.InvoiceDate AS [InvoiceDate],
		DATEDIFF(day,i.StartDate,i.EndDate) AS [InvoicePeriode],
		i.DueDate AS [DueDate],
		cig.Label,
		dbo.FindVAT(i.ID) AS [ExclVAT],
		i.Regarding,
		i.Closed,
		i.CustomerInvoiceGroupId,
		i.StartDate,
		i.EndDate,
		i.Delivered,
		i.DeliveredDate,
		i.attention,
		cig.Attention as CigAttention,
		CASE 
			WHEN cig.SendFormat = 0
				THEN c.SendFormat
				ELSE cig.SendFormat
		END AS [SendFormat],
		c.ContactName,
		i.IsCreditNote,
		i.InvoiceLinkId
	FROM InvoiceLines il, #TempList t
		INNER JOIN Customers c
			ON c.CustomerID = t.CustomerId		
		INNER JOIN CustomerInvoiceGroup cig
			ON c.CustomerID = cig.CustomerID
		INNER JOIN Invoices i
			ON i.CustomerInvoiceGroupId = cig.CustomerInvoiceGroupID
	GROUP BY
		c.CustomerName,
		i.InvoiceID,
		i.ID,
		i.[Guid],
		InvoiceDate,	
		cig.Label,
		i.Regarding,
		i.Closed,
		DATEDIFF(day,i.StartDate,i.EndDate),
		DueDate,
		i.CustomerInvoiceGroupId,
		i.StartDate,
		i.EndDate,
		i.Delivered,
		i.DeliveredDate,
		i.attention,
		c.SendFormat,
		cig.SendFormat,
		c.ContactName,
		i.IsCreditNote,
		i.InvoiceLinkId,
		cig.Attention
END
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.6.0.4', GETDATE(), 'LLS', 'Altered [GetCustomersInvoiceView], [GenerateNewInvoiceDraft] and [spGetInvoices]"')
GO 

