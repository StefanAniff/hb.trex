

CREATE PROCEDURE [dbo].[spRoundedTimeSpentPrTaskPrDayPrUserPrPrice] 
@customerId INT, @startdate DATETIME, @enddate DATETIME,@billable BIT
AS

         SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, c.CustomerID AS CustomerID, p.ProjectName AS Project, p.ProjectID, t.TaskName AS Task, t.TaskID,
                                                                   ISNULL(dbo.RoundUpToNextQuarter(SUM(te.BillableTime)),0) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate, te.Price
                                                                    
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
           
           GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, c.CustomerID, p.ProjectName,p.ProjectID, t.TaskName,t.TaskID, t.TimeLeft, t.TimeEstimated, te.Price

           --Then, sum the calculated timeentries, grouped by task
           SELECT     Customer,CustomerID, Project,ProjectID, Task,TaskID, SUM(TimeSpent) AS TimeSpent, Remaining, Estimate, Price
           FROM       #temp
           GROUP BY Customer,CustomerID, Project, ProjectID, Task,TaskID, Remaining, Estimate, Price

           DROP TABLE #temp

