SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spSetDocumentType] 
@invoiceId int
AS
BEGIN

	SET NOCOUNT ON;
	SET FMTONLY OFF
	
	SELECT te.TimeEntryID into #temp1
	FROM TimeEntries te
	LEFT JOIN CreditNote cn
	ON te.TimeEntryID = cn.TimeEntryID
	WHERE cn.TimeEntryID IS NULL
	AND cn.InvoiceId = @invoiceId
	
	SELECT te.TimeEntryID into #temp2
	FROM TimeEntries te
	INNER JOIN CreditNote cn
	ON te.TimeEntryID = cn.TimeEntryID
	WHERE cn.InvoiceId = @invoiceId
	
	update TimeEntries
	set DocumentType = 1
	where TimeEntryID = any(select t.TimeEntryID from #temp1 t)
	
	update TimeEntries
	set DocumentType = 3
	where TimeEntryID = any(select t.TimeEntryID from #temp2 t)


	drop table #temp2
	drop table #temp1

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[UpdateTimeEntriesInvoiceId]
	@customerInvoiceGroupId INT, 
	@startdate DATETIME, 
	@enddate DATETIME,
	@invoiceId int
AS
BEGIN

	UPDATE TimeEntries
	SET InvoiceId = @invoiceId, DocumentType = 2
	WHERE TimeEntryID = ANY(SELECT [TimeEntry ID] 
						FROM [dbo].FindAllTimeEntries(@invoiceId, @customerInvoiceGroupId, @startdate, @enddate))
END
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.5.0.6', GETDATE(), 'LLS', 'Added [spSetDocumentType] to set the relevant documenttype when deleting a draft and altered [UpdateTimeEntriesInvoiceId]')
GO