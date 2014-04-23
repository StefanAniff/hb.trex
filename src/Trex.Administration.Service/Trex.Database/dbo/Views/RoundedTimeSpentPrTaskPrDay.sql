
CREATE VIEW [dbo].[RoundedTimeSpentPrTaskPrDay]
AS
SELECT     Customer, Project, Task, SUM(TimeSpent) AS TimeSpent, Remaining, Estimate, CustomerID, ProjectID, StartDate
FROM         dbo.RoundedTimeSpentPrDayPrUser
GROUP BY Customer, Project, Task, Remaining, Estimate, CustomerID, ProjectID, StartDate

