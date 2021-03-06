
/****** Object:  StoredProcedure [dbo].[spRoundedTimeSpentPrTaskPrDayPrUser]    Script Date: 05/11/2010 00:00:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[spRoundedTimeSpentPrTaskPrDayPrUser] 
@customerId INT, @startdate DATETIME, @enddate DATETIME,@billable BIT
AS

         SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, c.CustomerID AS CustomerID, p.ProjectName AS Project, p.ProjectID, t.TaskName AS Task, t.TaskID,
                                                                   ISNULL(dbo.RoundUpToNextQuarter(SUM(te.BillableTime)),0) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate
                                                                    
           INTO #temp
           FROM   dbo.Customers AS c
           INNER JOIN dbo.Projects AS p ON p.CustomerID = c.CustomerID 
           INNER JOIN dbo.Tasks AS t ON t.ProjectID = p.ProjectID
           LEFT JOIN dbo.TimeEntries AS te ON te.TaskID = t.TaskID
           LEFT JOIN dbo.Users AS u ON u.UserID = te.UserID
                     
           WHERE COALESCE(te.StartTime,@startdate) >= @startdate 
           AND COALESCE(te.endtime,@enddate) <= @enddate
           AND c.CustomerID = @customerId
           AND COALESCE(te.Billable ,1) = @billable
           
           GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, c.CustomerID, p.ProjectName,p.ProjectID, t.TaskName,t.TaskID, t.TimeLeft, t.TimeEstimated

           --Then, sum the calculated timeentries, grouped by task
           SELECT     Customer,CustomerID, Project,ProjectID, Task,TaskID, SUM(TimeSpent) AS TimeSpent, Remaining, Estimate
           FROM       #temp
           GROUP BY Customer,CustomerID, Project, ProjectID, Task,TaskID, Remaining, Estimate

           DROP TABLE #temp

GO





           INSERT INTO [Version]
           ([Version]
           ,[Date]
           ,[Creator]
           ,[Description])

     VALUES
           ('1.1.0.0'
           ,GETDATE()
           ,'tga',
           'Changed [spRoundedTimeSpentPrTaskPrDayPrUser]')
GO
           