
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
