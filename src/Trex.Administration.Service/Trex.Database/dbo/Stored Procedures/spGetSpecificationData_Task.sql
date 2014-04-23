
CREATE PROCEDURE [dbo].[spGetSpecificationData_Task]
(
	@invoiceId int,
	@billable bit,
	@fixedProject bit
)
AS
BEGIN		
	SET NOCOUNT ON
	SET FMTONLY OFF

	DECLARE @temp TABLE (TimePrUser float, Task nvarchar(350), ProjectID int, FixedPriceProject bit, FixedPrice float);
	
	DECLARE @isInvoiced BIT = 
		CASE
			WHEN (SELECT TOP 1 i.InvoiceID FROM Invoices i WHERE i.ID = @invoiceId) IS NULL
				THEN 0
				ELSE 1
		END
	
	IF  @isInvoiced = 1
		BEGIN
			INSERT INTO @temp(TimePrUser, Task, ProjectID, FixedPriceProject, FixedPrice)
			SELECT
				dbo.RoundUpToNextQuarter(SUM(x.TimeSpent)) AS TimePrUser,
				x.Task,
				x.ProjectID,
				x.FixedPriceProject,
				x.FixedPrice
			FROM dbo.AggregatedCreditNotesPrTaskPrDayPrInvoice(@invoiceId, @fixedProject) x
			WHERE x.Billable = @billable
			GROUP BY
				x.Task,
				x.ProjectID	,
				x.FixedPriceProject,
				x.FixedPrice		
		END
	ELSE
		BEGIN
			INSERT INTO @temp(TimePrUser, Task, ProjectID, FixedPriceProject, FixedPrice)
			SELECT
				dbo.RoundUpToNextQuarter(SUM(x.TimeSpent)) AS TimePrUser,
				x.Task,
				x.ProjectID,
				x.FixedPriceProject,
				x.FixedPrice
			FROM dbo.AggregatedTimeEntriesPrTaskPrDayPrInvoice(@invoiceId, @fixedProject) x
			WHERE x.Billable = @billable
			GROUP BY
				x.Task,
				x.ProjectID,
				x.FixedPriceProject,
				x.FixedPrice
		END
		
	SELECT 
		t.Task AS TaskName,
		t.ProjectID,
		SUM(t.TimePrUser) AS [TimeUsed],
		t.FixedPriceProject,
		t.FixedPrice
	FROM @temp t
	GROUP BY
		t.Task,
		t.ProjectID,
		t.FixedPriceProject,
		t.FixedPrice
	
END
