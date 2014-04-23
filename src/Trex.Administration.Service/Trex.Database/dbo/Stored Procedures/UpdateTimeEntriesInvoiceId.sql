
CREATE PROCEDURE [dbo].[UpdateTimeEntriesInvoiceId]
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
