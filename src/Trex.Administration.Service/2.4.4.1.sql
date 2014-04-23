

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FindTimeEntries]
(	
	@customerInvoiceGroupId INT, 
	@startdate DATETIME, 
	@enddate DATETIME
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT   
		te.TimeEntryID AS [TimeEntry ID],
		te.InvoiceId AS InvoiceID,
		p.ProjectName AS Project, 
		p.ProjectID AS [Project ID],
		t.TaskName AS Task, 
		t.TaskID AS [Task ID],
		dbo.ConvertToSmallDate(te.StartTime) AS StartDate, 
		dbo.ConvertToSmallDate(te.EndTime) AS EndDate,
		dbo.RoundUpToNextQuarter(te.BillableTime) AS TimeSpent, 
		te.Price AS [Price pr Hour],
		p.CustomerInvoiceGroupID AS [CIG ID],
		cig.Label AS GroupName,
		p.CustomerID AS [Customer ID]

	FROM 
		dbo.TimeEntries AS te 
        INNER JOIN dbo.Tasks AS t 
			ON t.TaskID = te.TaskID 
        INNER JOIN dbo.Projects AS p 
			ON p.ProjectID = t.ProjectID 
        INNER JOIN dbo.CustomerInvoiceGroup AS cig 
			ON cig.CustomerInvoiceGroupID = p.CustomerInvoiceGroupID 
        INNER JOIN dbo.Users AS u 
			ON u.UserID = te.UserID 
	WHERE 
		te.StartTime >= @startdate 
		AND te.endtime <= @enddate
		AND cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
		AND te.Billable = 1
)
GO



GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateTimeEntriesInvoiceId]
	@customerInvoiceGroupId INT, 
	@startdate DATETIME, 
	@enddate DATETIME,
	@invoiceId int
AS
BEGIN

	UPDATE TimeEntries
	SET InvoiceId = @invoiceId
	WHERE TimeEntryID = ANY(SELECT [TimeEntry ID] 
						FROM [dbo].FindTimeEntries(@customerInvoiceGroupId, @startdate, @enddate))
END
GO
