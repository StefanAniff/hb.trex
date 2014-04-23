/*Add TimeEntryID column to InvoiceLines table*/
alter table invoiceLines add TimeEntryId int

GO

/****** Add TimeEntryID to result ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[spGetGeneratedInvoiceLines] @InvoiceId INT

AS

SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate,  
						  dbo.RoundUpToNextQuarter(SUM(te.BillableTime)) AS TimeSpent, te.price ,t.TaskID,u.UserID,te.TimeEntryID
INTO #temp
	FROM         dbo.TimeEntries AS te 
				INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
				INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
				INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
				INNER JOIN dbo.Users AS u ON u.UserID = te.UserID
	WHERE  te.InvoiceID = @InvoiceId
	
	GROUP BY dbo.ConvertToSmallDate(te.StartTime),  te.price, t.taskid,u.UserID,te.TimeEntryID
	

	--Then, sum the calculated timeentries, grouped by price
	SELECT     price, SUM(TimeSpent) AS TimeSpent,TimeEntryID
	FROM       #temp
	GROUP BY price,TimeEntryID
	drop table #temp


GO


/****** Object:  StoredProcedure [dbo].[spUnbookTimeEntries]    Script Date: 08/24/2011 09:22:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[spUnbookTimeEntries] @timeentryID INT
AS

UPDATE timeentries
SET InvoiceID = NULL
WHERE TimeEntryID = @timeentryID


GO




  INSERT INTO [Version]
           ([Version]
           ,[Date]
           ,[Creator]
           ,[Description])

     VALUES
           ('x.x.x'
           ,GETDATE()
           ,'djo',
           'New column TimeEntryID in InvoiceLines, modifiend sp: [spGetGeneratedInvoiceLines] to return TimeEntryID and new sp[spUnbookTimeEntries]')
GO
           