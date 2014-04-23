

UPDATE Invoices
SET DeliveredDate = InvoiceDate
WHERE 
	DeliveredDate is null
	AND Delivered = 1
GO


sp_RENAME 'Invoices.InvoiceParentId', 'InvoiceLinkId', 'COLUMN'
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
		cig.Attention,
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
		cig.Attention,
		c.SendFormat,
		cig.SendFormat,
		c.ContactName,
		i.IsCreditNote,
		i.InvoiceLinkId
END
GO

CREATE PROCEDURE [dbo].[spReplicateCreditNoteLines]
	@oldInvoiceId int
AS
BEGIN
	SELECT 
		cn.TimeEntryID
	INTO #temp1
	FROM CreditNote cn
	WHERE cn.InvoiceID = @oldInvoiceId
	
	SELECT 
		te.DocumentType,
		te.TimeEntryID
	INTO #temp2
	FROM TimeEntries te
		INNER JOIN #temp1 t ON t.TimeEntryID = te.TimeEntryID
		
	UPDATE TimeEntries
	SET DocumentType = 3
	FROM #temp2 t2
		INNER JOIN TimeEntries te ON te.TimeEntryID = t2.TimeEntryID
END
GO


ALTER PROCEDURE [dbo].[spDeleteInvoice]
	@invoiceId int
AS
BEGIN
	DELETE FROM InvoiceLines
	WHERE InvoiceID = @invoiceId
	
	DELETE FROM CreditNote
	WHERE InvoiceID = @invoiceId
	
	DELETE FROM InvoiceFiles
	WHERE InvoiceID = @invoiceId
	
	DECLARE @wasCreditNote bit = (SELECT TOP 1 i.IsCreditNote FROM Invoices i WHERE i.ID = @invoiceId)
	
	UPDATE TimeEntries
	SET InvoiceId = NULL, 
	DocumentType = CASE
						WHEN @wasCreditNote = 1
							THEN 3
							ELSE 1
					END
	WHERE InvoiceId = @invoiceId
	
	DECLARE @linkedId int = (SELECT TOP 1 i.InvoiceLinkId FROM Invoices i WHERE i.ID = @invoiceId)
	
	UPDATE Invoices
	SET InvoiceLinkId = null
	WHERE ID = @linkedId
	
	DELETE FROM Invoices
	WHERE ID = @invoiceId
END
GO

ALTER PROCEDURE [dbo].[spGenerateCreditNoteLines] 
	@OriginalInvoiceId int,
	@NewInvoiceId int
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF
	
	--Take a copy
	INSERT INTO CreditNote(
		Billable, 
		BillableTime, 
		ChangedBy, 
		ChangeDate, 
		ClientSourceId, 
		CreateDate,
		DocumentDate,
		Description, 
		EndTime, 
		Guid, 
		InvoiceID, 
		PauseTime, 
		Price, 
		StartTime, 
		TaskID, 
		TimeEntryID, 
		TimeEntryTypeId, 
		TimeSpent, 
		UserID)
	SELECT 
		cn.Billable,
		cn.BillableTime,
		cn.ChangedBy,
		cn.ChangeDate,
		cn.ClientSourceId,
		cn.CreateDate,
		GETDATE(),
		cn.Description,
		cn.EndTime,
		cn.Guid,
		@NewInvoiceId,
		cn.PauseTime,
		cn.Price * (-1),
		cn.StartTime,
		cn.TaskID,
		cn.TimeEntryID,
		cn.TimeEntryTypeId,
		cn.TimeSpent,
		cn.UserID
	FROM CreditNote cn
	WHERE cn.InvoiceID = @OriginalInvoiceId

	--Insert new lines
	INSERT INTO InvoiceLines 
		(PricePrUnit, 
		 Units, 
		 InvoiceID, 
		 Unit, 
		 UnitType,
		 IsExpense, 
		 VatPercentage,
		 [Text])
	SELECT
		cn.PricePrUnit * (-1),
		cn.Units,
		@NewInvoiceId,
		cn.Unit,
		cn.UnitType,
		cn.IsExpense,
		cn.VatPercentage,
		cn.[Text]
	FROM InvoiceLines cn
	WHERE cn.InvoiceID = @OriginalInvoiceId

END
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.5.0.13', GETDATE(), 'RAB', 'Update allready closed invoices"s DeliveredDate to InvoiceDate')
GO
