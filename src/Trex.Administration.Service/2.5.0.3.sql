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
		cig.Attention,
		cig.SendFormat,
		c.ContactName,
		i.IsCreditNote
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
		cig.Attention,
		cig.SendFormat,
		c.ContactName,
		i.IsCreditNote
END
GO


INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.5.0.3', GETDATE(), 'LLS', 'Added IsCreditNote to view')
GO