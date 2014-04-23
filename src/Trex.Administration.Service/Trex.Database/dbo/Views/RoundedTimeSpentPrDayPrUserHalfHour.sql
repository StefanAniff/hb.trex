

CREATE VIEW [dbo].[RoundedTimeSpentPrDayPrUserHalfHour]
AS
SELECT     dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, p.ProjectName AS Project, t.TaskName AS Task, 
                      dbo.[RoundUpToNextHalfHour](SUM(te.BillableTime)) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate, u.UserName, c.CustomerID, p.ProjectID, 
                      u.UserID, u.Name, te.Billable
FROM         dbo.TimeEntries AS te INNER JOIN
                      dbo.Tasks AS t ON t.TaskID = te.TaskID INNER JOIN
                      dbo.Projects AS p ON p.ProjectID = t.ProjectID INNER JOIN
                      dbo.Customers AS c ON c.CustomerID = p.CustomerID INNER JOIN
                      dbo.Users AS u ON u.UserID = te.UserID
GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName, t.TimeLeft, t.TimeEstimated, u.UserName, c.CustomerID, p.ProjectID, 
                      u.UserID, u.Name, te.Billable


