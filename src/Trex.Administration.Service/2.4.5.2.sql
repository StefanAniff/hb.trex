
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spDeleteInvoiceLine]
	@InvoiceLineId int
AS
BEGIN
	DELETE InvoiceLines
	WHERE ID = @InvoiceLineId
		AND UnitType != 0;
END
GO


GO

CREATE PROCEDURE [dbo].[spGetSpecificationData_Project]
	@invoiceId int
AS
BEGIN
	
	SELECT 
		p.ProjectID,
		p.ProjectName, 
		sum(dbo.RoundUpToNextQuarter(te.BillableTime)) AS [TimeUsed]
	FROM TimeEntries te 
		join Tasks t on te.TaskID = t.TaskID
		join Projects p on p.ProjectID = t.ProjectID
	WHERE te.InvoiceId = @invoiceId
	GROUP BY 
		p.ProjectID,
		p.ProjectName
	
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetSpecificationData_Task]
	@invoiceId int
AS
BEGIN
	
	SELECT 
		p.ProjectID,
		t.taskname, 
		sum(dbo.RoundUpToNextQuarter(te.BillableTime)) AS [TimeUsed]
	FROM TimeEntries te 
		join Tasks t on te.taskid = t.TaskID
		join Projects p on p.ProjectID= t.ProjectID
	WHERE te.InvoiceId = @invoiceId
	GROUP BY 
		p.ProjectID,
		t.taskname
	
	
END
GO




ALTER TABLE CustomerInvoiceGroup
ADD Email nvarchar(200)

GO