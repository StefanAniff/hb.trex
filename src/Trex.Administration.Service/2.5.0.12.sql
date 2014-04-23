

sp_RENAME 'CreditNote.[CreditNoteDate]', 'DocumentDate', 'COLUMN'



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spCopyTimeEntriesToCreditNote]
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
GO

CREATE PROCEDURE [dbo].[spSaveProject]
@ProjectId int,
@Name nvarchar(100),
@cigId int,
@changedate DATETIME
AS
BEGIN
	update Projects
	set ProjectName = @Name,
		CustomerInvoiceGroupID = @cigId,
		ChangeDate = @changedate
	where
		ProjectID = @ProjectId
END
GO


INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.5.0.12', GETDATE(), 'RAB', 'Renamed column CreditNoteDate to DocumentDate, altered [spCopyTimeEntriesToCreditNote] to use this column, added [spSaveProject]')
GO