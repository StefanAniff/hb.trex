USE [trex.dk]
GO

ALTER TABLE [dbo].[Invoices]
ADD [AmountPaid] [float] NOT NULL
CONSTRAINT AmountPaid_DefaultValue DEFAULT 0

GO

/****** Object:  StoredProcedure [dbo].[spGetInvoices]    Script Date: 10/25/2013 08:31:37 ******/
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
		(SELECT TOP 1 ic.Comment FROM InvoiceComments ic
		where ic.InvoiceID = i.ID
		ORDER BY ic.InvoiceCommentID desc) AS Comment,
		cig.Attention as CigAttention,
		CASE 
			WHEN cig.SendFormat = 0
				THEN c.SendFormat
				ELSE cig.SendFormat
		END AS [SendFormat],
		(SELECT u.Name from Users u
		where u.UserID = c.UserID) AS CustomerManager,
		c.ContactName,
		i.IsCreditNote,
		i.InvoiceLinkId,
		I.AmountPaid
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
		c.UserId,
		cig.Attention,
		I.AmountPaid
END

GO




