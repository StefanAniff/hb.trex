
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
