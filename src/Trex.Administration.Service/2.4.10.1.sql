
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGenerateNewInvoiceDraft]
@customerInvoiceGroupId int,
@createdBy int,
@VAT float,
@startDate DATETIME,
@endDate DATETIME
AS
BEGIN

	DECLARE @Duedate DATETIME;
	DECLARE @invoiceDate DATETIME;
	DECLARE @today DATETIME;
	
	SET @today = GETDATE();
	
	IF @endDate > @today
	BEGIN
		SET @invoiceDate = @today
	END
	ELSE
	BEGIN
		SET @invoiceDate = @endDate
	END
	
	INSERT INTO Invoices (
		CreateDate, 
		InvoiceDate, 
		CreatedBy, 
		VAT, 
		StartDate, 
		EndDate, 
		Closed, 
		DueDate, 
		CustomerInvoiceGroupId)
	VALUES 
		(
		GETDATE(),
		@invoiceDate, 
		@createdBy,
		@VAT,
		@startDate,
		@endDate,
		0, --Not closed
		DATEADD(D, 14, @invoiceDate), 
		@customerInvoiceGroupId
		);
		
	SELECT TOP 1 *
	FROM Invoices i	
	ORDER BY i.ID DESC
	
END
SET ANSI_NULLS ON
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGetSpecificationData_Project]
	@invoiceId int
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF
	
	SELECT 
		te.TimeEntryID,
		te.TaskID,
		te.BillableTime,
		dateadd(dd, datediff(dd,0, te.StartTime), 0) as StartTime,
		te.UserID,
		p.ProjectID,
		t.TaskName,
		p.ProjectName
	INTO #temp
	FROM TimeEntries te
		INNER JOIN Tasks t on te.TaskID = t.TaskID
		INNER JOIN Projects p on t.ProjectID = p.ProjectID
		INNER JOIN CustomerInvoiceGroup cig on p.CustomerInvoiceGroupID = cig.CustomerInvoiceGroupID
		INNER JOIN Customers c on cig.CustomerID = c.CustomerID
		INNER JOIN Users u on te.UserID = u.UserID
	WHERE 
		te.InvoiceId = @InvoiceId
		AND te.Billable = 1
		
	--SELECT *
	--FROM #temp
		
	SELECT
		dbo.RoundUpToNextQuarter(SUM(t.BillableTime)) AS TimePrUser,
		t.TaskName,
		t.ProjectID,
		t.ProjectName
	INTO #temp2
	FROM #temp t
	GROUP BY
		t.TaskID,
		t.UserID,
		StartTime,
		t.TaskName,
		t.ProjectID,
		t.ProjectName
		
	SELECT 
		t2.ProjectID,
		t2.ProjectName,
		SUM(t2.TimePrUser) AS [TimeUsed]
	FROM #temp2 t2
	GROUP BY
		t2.ProjectID,
		t2.ProjectName
		
	DROP TABLE #temp
	DROP TABLE #temp2	
	
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGetSpecificationData_Task]
	@invoiceId int
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF
	
	SELECT 
		te.TimeEntryID,
		te.TaskID,
		te.BillableTime,
		dateadd(dd, datediff(dd,0, te.StartTime), 0) as StartTime,
		te.UserID,
		p.ProjectID,
		t.TaskName
	INTO #temp
	FROM TimeEntries te
		INNER JOIN Tasks t on te.TaskID = t.TaskID
		INNER JOIN Projects p on t.ProjectID = p.ProjectID
		INNER JOIN CustomerInvoiceGroup cig on p.CustomerInvoiceGroupID = cig.CustomerInvoiceGroupID
		INNER JOIN Customers c on cig.CustomerID = c.CustomerID
		INNER JOIN Users u on te.UserID = u.UserID
	WHERE 
		te.InvoiceId = @InvoiceId
		AND te.Billable = 1
		
	--SELECT *
	--FROM #temp
		
	SELECT
		dbo.RoundUpToNextQuarter(SUM(t.BillableTime)) AS TimePrUser,
		t.TaskName,
		t.ProjectID
	INTO #temp2
	FROM #temp t
	GROUP BY
		t.TaskID,
		t.UserID,
		StartTime,
		t.TaskName,
		t.ProjectID
		
	SELECT 
		t2.TaskName,
		t2.ProjectID,
		SUM(t2.TimePrUser) AS [TimeUsed]
	FROM #temp2 t2
	GROUP BY
		t2.TaskName,
		t2.ProjectID
		
	DROP TABLE #temp
	DROP TABLE #temp2	
	
END
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.4.10.1', GETDATE(), 'RAB', 'Removed outcommented lines from [spGenerateNewInvoiceDraft], and changed in spGetSpecificationData_Task and spGetSpecificationData_Project')
GO