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
