/*
   31. marts 200909:49:06
   User: 
   Server: MICKEYMOUSE\SQL2008
   Database: trex
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Invoices ADD
	DueDate datetime NULL
GO
ALTER TABLE dbo.Invoices SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Invoices', 'Object', 'CONTROL') as Contr_Per 
GO


/****** Object:  StoredProcedure [dbo].[spAggregatedTimeEntriesPrTaskPrDayPrInvoice]    Script Date: 03/31/2009 10:15:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spAggregatedTimeEntriesPrTaskPrDayPrInvoice] 
@InvoiceId INT
AS

	--First, sum all timeentries,grouped by date, and round time to nearest quarter
	SELECT   dbo.ConvertToSmallDate(te.StartTime) AS TaskDate, c.CustomerName AS Customer,c.customerId ,p.ProjectName AS Project, t.TaskName AS Task, 
						  dbo.RoundUpToNextQuarter(SUM(te.TimeSpent-te.PauseTime)) AS TimeSpent,
					i.ID AS InvoiceNumber, i.InvoiceDate, i.StreetAddress, i.ZipCode, i.City, i.Country, i.Attention, i.StartDate, i.EndDate, i.DueDate
	INTO #temp
	FROM         dbo.TimeEntries AS te 
				INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
				INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
				INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
				INNER JOIN dbo.Invoices AS i ON te.InvoiceId = i.ID
	WHERE i.ID = @InvoiceId
	
	GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName,c.customerId ,i.ID, i.InvoiceDate, i.StreetAddress, i.ZipCode, i.City, i.Country, i.Attention, i.StartDate, i.EndDate, i.DueDate

	--Then, sum the calculated timeentries, grouped by task
	SELECT     Customer AS CustomerName, Project, Task, SUM(TimeSpent) AS TimeSpent, InvoiceNumber, InvoiceDate,customerId AS CustomerNumber, StreetAddress, ZipCode, City,Country,Attention AS ContactName,StartDate, EndDate,DueDate
	FROM       #temp
	GROUP BY Customer, Project, Task,InvoiceNumber, InvoiceDate,CustomerId, StreetAddress, ZipCode, City,Country,Attention,StartDate, EndDate,DueDate

	DROP TABLE #temp

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spResetBookedTimeEntries] @invoiceId INT
AS

BEGIN TRANSACTION

BEGIN TRY
	UPDATE TimeEntries
	SET InvoiceID = NULL
	WHERE InvoiceID = @invoiceId

	DELETE FROM InvoiceLines WHERE InvoiceID= @invoiceId AND UnitType = 0
	
	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION;

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

END CATCH

GO



INSERT INTO Version (Version ,Date,Creator,Description) VALUES('0.9.8.0',GetDate(),'tga','Added duedate to invoice table')
	GO