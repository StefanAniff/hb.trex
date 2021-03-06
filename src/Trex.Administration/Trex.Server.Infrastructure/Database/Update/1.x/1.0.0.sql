
/****** Object:  StoredProcedure [dbo].[spAggregatedTimeEntriesPrTaskPrDay]    Script Date: 05/10/2010 17:19:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spAggregatedTimeEntriesPrTaskPrDay] 
@customerId INT, @startdate DATETIME, @enddate DATETIME, @billable BIT
AS

           --First, sum all timeentries,grouped by date, and round time to nearest quarter
           SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, p.ProjectName AS Project, t.TaskName AS Task, 
                                                                   dbo.RoundUpToNextQuarter(SUM(te.BillableTime)) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate, StartTime as PeriodStart,EndTime as PeriodEnd
                                                                    
           INTO #temp
           FROM         dbo.TimeEntries AS te 
                                            INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
                                            INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
                                            INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
                                            INNER JOIN dbo.Users AS u ON u.UserID = te.UserID
           WHERE te.StartTime >= @startdate AND te.endtime <= @enddate
           AND c.CustomerID = @customerId
           AND te.Billable = @billable
           GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName, t.TimeLeft, t.TimeEstimated,StartTime,EndTime

           --Then, sum the calculated timeentries, grouped by task
           SELECT     Customer, Project, Task, SUM(TimeSpent) AS TimeSpent, Remaining, Estimate,PeriodStart,PeriodEnd
           FROM       #temp
           GROUP BY Customer, Project, Task, Remaining, Estimate,PeriodStart,PeriodEnd

           DROP TABLE #temp

           
           
           GO
           

		   

/****** Object:  View [dbo].[RoundedTimeSpentPrDayPrUser]    Script Date: 05/10/2010 17:22:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[RoundedTimeSpentPrDayPrUser]
AS
SELECT     dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, p.ProjectName AS Project, t.TaskName AS Task, 
                      dbo.RoundUpToNextQuarter(SUM(te.BillableTime)) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate, u.UserName, c.CustomerID, p.ProjectID, 
                      u.UserID, u.Name, te.Billable
FROM         dbo.TimeEntries AS te INNER JOIN
                      dbo.Tasks AS t ON t.TaskID = te.TaskID INNER JOIN
                      dbo.Projects AS p ON p.ProjectID = t.ProjectID INNER JOIN
                      dbo.Customers AS c ON c.CustomerID = p.CustomerID INNER JOIN
                      dbo.Users AS u ON u.UserID = te.UserID
GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName, t.TimeLeft, t.TimeEstimated, u.UserName, c.CustomerID, p.ProjectID, 
                      u.UserID, u.Name, te.Billable

GO


/****** Object:  View [dbo].[RoundedTimeSpentPrTaskPrDay]    Script Date: 05/10/2010 17:22:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[RoundedTimeSpentPrTaskPrDay]
AS
SELECT     Customer, Project, Task, SUM(TimeSpent) AS TimeSpent, Remaining, Estimate, CustomerID, ProjectID, StartDate
FROM         dbo.RoundedTimeSpentPrDayPrUser
GROUP BY Customer, Project, Task, Remaining, Estimate, CustomerID, ProjectID, StartDate

GO







           INSERT INTO [Version]
           ([Version]
           ,[Date]
           ,[Creator]
           ,[Description])

     VALUES
           ('1.0.0.0'
           ,GETDATE()
           ,'tga',
           'Error in spAggregatedTimeEntriesPrTaskPrDay')
GO
           
           
