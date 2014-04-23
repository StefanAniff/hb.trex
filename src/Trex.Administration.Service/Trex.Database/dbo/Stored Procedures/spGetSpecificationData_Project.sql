
CREATE PROCEDURE [dbo].[spGetSpecificationData_Project]
	@invoiceId int,
	@billable bit,
	@fixedPrice bit
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF

	DECLARE @temp TABLE (TimePrUser float, Task nvarchar(350), ProjectID int, Project nvarchar(200), FixedPriceProject bit, FixedPrice float);
	
	DECLARE @isInvoiced BIT = 
		CASE
			WHEN (SELECT TOP 1 i.InvoiceID FROM Invoices i WHERE i.ID = @invoiceId) IS NULL
				THEN 0
				ELSE 1
		END
	
	IF  @isInvoiced = 1
		BEGIN
			INSERT INTO @temp(TimePrUser, Task, ProjectID, Project, FixedPriceProject, FixedPrice)
			SELECT
				dbo.RoundUpToNextQuarter(SUM(t.TimeSpent)) AS TimePrUser,
				t.Task,
				t.ProjectID,
				t.Project,
				t.FixedPriceProject,
				t.FixedPrice
			FROM dbo.AggregatedCreditNotesPrTaskPrDayPrInvoice(@invoiceId, @fixedPrice) t --From CreditNote
			WHERE t.Billable = @billable
			GROUP BY
				t.Task,
				t.ProjectID,
				t.Project,
				t.FixedPriceProject,
				t.FixedPrice			
		END
	ELSE
		BEGIN
			INSERT INTO @temp(TimePrUser, Task, ProjectID, Project, FixedPriceProject, FixedPrice)
			SELECT
				dbo.RoundUpToNextQuarter(SUM(t.TimeSpent)) AS TimePrUser,
				t.Task,
				t.ProjectID,
				t.Project,
				t.FixedPriceProject,
				t.FixedPrice
			FROM dbo.AggregatedTimeEntriesPrTaskPrDayPrInvoice(@invoiceId, @fixedPrice) t --From TimeEntries
			WHERE t.Billable = @billable
			GROUP BY
				t.Task,
				t.ProjectID,
				t.Project,
				t.FixedPriceProject,
				t.FixedPrice
		END		
		
	SELECT 
		t.ProjectID,
		t.Project AS ProjectName,
		SUM(t.TimePrUser) AS [TimeUsed],
		t.FixedPriceProject,
		t.FixedPrice
	FROM @temp t
	GROUP BY
		t.ProjectID,
		t.Project,
		t.FixedPriceProject,
		t.FixedPrice
END
