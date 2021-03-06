/****** Object:  StoredProcedure [dbo].[spAggregatedTimeEntriesPrTaskPrDay]    Script Date: 05/10/2010 23:57:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spAggregatedTimeEntriesPrTaskPrDay] 
@customerId INT, @startdate DATETIME, @enddate DATETIME, @billable BIT
AS

           --First, sum all timeentries,grouped by date, and round time to nearest quarter
           SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, p.ProjectName AS Project, t.TaskName AS Task, 
                                                                   dbo.RoundUpToNextQuarter(SUM(te.BillableTime)) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate
                                                                    
           INTO #temp
           FROM         dbo.TimeEntries AS te 
                                            INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
                                            INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
                                            INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
                                            INNER JOIN dbo.Users AS u ON u.UserID = te.UserID
           WHERE te.StartTime >= @startdate AND te.endtime <= @enddate
           AND c.CustomerID = @customerId
           AND te.Billable = @billable
           GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName, t.TimeLeft, t.TimeEstimated

           --Then, sum the calculated timeentries, grouped by task
           SELECT     Customer, Project, Task, SUM(TimeSpent) AS TimeSpent, Remaining, Estimate
           FROM       #temp
           GROUP BY Customer, Project, Task, Remaining, Estimate

           DROP TABLE #temp

GO

/****** Object:  StoredProcedure [dbo].[spRoundedTimeSpentPrTaskPrDayPrUser]    Script Date: 05/11/2010 00:00:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spRoundedTimeSpentPrTaskPrDayPrUser] 
@customerId INT, @startdate DATETIME, @enddate DATETIME,@billable BIT
AS

           --First, sum all timeentries,grouped by date, and round time to nearest quarter
           SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, c.CustomerID AS CustomerID, p.ProjectName AS Project, p.ProjectID, t.TaskName AS Task, t.TaskID,
                                                                   dbo.RoundUpToNextQuarter(SUM(te.BillableTime)) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate
                                                                    
           INTO #temp
           FROM         dbo.TimeEntries AS te 
                                            INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
                                            INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
                                            INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
                                            INNER JOIN dbo.Users AS u ON u.UserID = te.UserID
           WHERE te.StartTime >= @startdate AND te.endtime <= @enddate
           AND c.CustomerID = @customerId
           AND te.Billable = @billable
           
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
           'Error in spAggregatedTimeEntriesPrTaskPrDay, and new sp: [spRoundedTimeSpentPrTaskPrDayPrUser]')
GO
           