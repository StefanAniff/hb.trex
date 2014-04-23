
CREATE PROCEDURE [dbo].[spCopyTimeEntriesToCreditNote]
	@invoiceId int
AS
BEGIN
	SELECT *
	INTO #temp
	FROM TimeEntries te
	WHERE te.InvoiceId = @invoiceId
	
	INSERT INTO CreditNote
		(
		 Billable,
		 BillableTime,
		 ChangeDate,
		 ChangedBy,
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
		 UserID
		)
	SELECT 
		 t.Billable,
		 t.BillableTime,
		 t.ChangeDate,
		 t.ChangedBy,
		 t.ClientSourceId,
		 t.CreateDate,
		 GETDATE(),
		 t.Description,
		 t.EndTime,
		 t.Guid,
		 t.InvoiceId,
		 t.PauseTime,
		 t.Price,
		 t.StartTime,
		 t.TaskID,
		 t.TimeEntryID,
		 t.TimeEntryTypeId,
		 t.TimeSpent,
		 t.UserID
	FROM #temp t
	
	UPDATE TimeEntries
	SET DocumentType = 2
	WHERE InvoiceId = @invoiceId
END
