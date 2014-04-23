

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.4.8.6', GETDATE(), 'RAB', 'Created SP to release TimeEntries')
GO