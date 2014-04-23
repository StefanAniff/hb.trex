

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGetSpecificationData_Project]
	@invoiceId int
AS
BEGIN
	
	SELECT 
		p.ProjectID,
		p.ProjectName, 
		sum(dbo.RoundUpToNextQuarter(te.BillableTime)) AS [TimeUsed]
	FROM TimeEntries te  
		INNER JOIN Tasks t on te.taskid = t.TaskID
		INNER JOIN Projects p on p.ProjectID = t.ProjectID
	WHERE 
		te.InvoiceId = @invoiceId
		AND te.Billable = 1
	GROUP BY 
		p.ProjectID,
		p.ProjectName
	
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGetSpecificationData_Task]
	@invoiceId int
AS
BEGIN
	
	SELECT 
		p.ProjectID,
		t.taskname, 
		sum(dbo.RoundUpToNextQuarter(te.BillableTime)) AS [TimeUsed]
	FROM TimeEntries te 
		INNER JOIN Tasks t on te.taskid = t.TaskID
		INNER JOIN Projects p on p.ProjectID = t.ProjectID
	WHERE 
		te.InvoiceId = @invoiceId		
		AND te.Billable = 1
	GROUP BY 
		p.ProjectID,
		t.taskname
END
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.4.8.8', GETDATE(), 'RAB', '[spGetSpecificationData_Project] and [spGetSpecificationData_Task] now only takes billableTime')
GO