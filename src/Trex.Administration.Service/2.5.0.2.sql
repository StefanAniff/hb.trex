ALTER TABLE InvoiceTemplates
ADD StandardCreditNotePrint bit NOT NULL DEFAULT 0
GO

ALTER TABLE InvoiceTemplates
ADD StandardCreditNoteMail bit NOT NULL DEFAULT 0
GO

ALTER TABLE Invoices
ADD IsCreditNote bit DEFAULT 0 NOT NULL
GO

ALTER TABLE CustomerInvoiceGroup
ADD CreditNoteTemplateIdMail int
GO

ALTER TABLE CustomerInvoiceGroup
ADD CreditNoteTemplateIdPrint int
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGenerateCreditNoteLines] 
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
		CreditNoteDate,
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

	SELECT 
		cn.BillableTime,
		cn.Price,
		cn.InvoiceID,
		dateadd(dd, datediff(dd,0, cn.StartTime), 0) as StartTime,
		cn.TaskID,
		cn.UserID,
		p.ProjectID,
		t.TaskName
	INTO #temp
	FROM CreditNote cn
		INNER JOIN Tasks t ON cn.TaskID = t.TaskID
		INNER JOIN Projects p ON t.ProjectID = p.ProjectID
	WHERE 
		cn.InvoiceID = @NewInvoiceId
		AND cn.Billable = 1
		
	--SELECT *
	--FROM #temp
		
	SELECT
		dbo.RoundUpToNextQuarter(SUM(t.BillableTime)) AS TimePrUser,
		t.TaskName,
		t.ProjectID,
		t.Price
	INTO #temp2
	FROM #temp t
	GROUP BY
		t.TaskID,
		t.UserID,
		StartTime,
		t.TaskName,
		t.ProjectID,
		t.Price
		
	--SELECT *
	--FROM #temp2
	
	SELECT
		SUM(t2.TimePrUser) AS TotalTimePrPriceUnit,
		t2.Price
	INTO #temp3
	FROM #temp2 t2
	GROUP BY t2.Price
	
	--SELECT *
	--FROM #temp3
		
	--Insert new Invoiceslines
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
		t3.Price,
		t3.TotalTimePrPriceUnit,
		@NewInvoiceId,
		'timer',
		0, --Auto generated
		0, --isExpense
		(SELECT TOP 1 i.VAT FROM Invoices i WHERE i.ID = @OriginalInvoiceId),
		''
	FROM #temp3 t3
	
	--Copy custom added lines with new id
	SELECT *
	INTO #temp4
	FROM InvoiceLines il
	WHERE 
		il.InvoiceID = @OriginalInvoiceId
		AND il.UnitType != 0
	
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
		t4.PricePrUnit * (-1),
		t4.Units, 
		@NewInvoiceId, 
		t4.Unit, 
		t4.UnitType,
		t4.IsExpense, 
		t4.VatPercentage,
		t4.[Text]
	FROM #temp4 t4
	
	--Dispose temporary tables
	DROP TABLE #temp
	DROP TABLE #temp2
	DROP TABLE #temp3
	DROP TABLE #temp4
END
GO


INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.5.0.2', GETDATE(), 'RAB', 'Added columns for credit note templates in InvoicesTemplates and CustomerInvoiceGroup, a bit in Invoices to see if an invoices IS an invoice or not, a SP to calculate invoiceLines from CreditNote')
GO