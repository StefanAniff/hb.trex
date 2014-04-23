
CREATE PROCEDURE [dbo].[UpdateTimeEntriesInvoiceIdToNull]
	@customerInvoiceGroupId INT, 
	@startdate DATETIME, 
	@enddate DATETIME,
	@invoiceId int
AS
BEGIN

	UPDATE TimeEntries
	SET InvoiceId = null
	WHERE TimeEntryID = ANY(SELECT [TimeEntry ID] 
						FROM [dbo].FindAllTimeEntries(@invoiceId, @customerInvoiceGroupId, @startdate, @enddate))
END
