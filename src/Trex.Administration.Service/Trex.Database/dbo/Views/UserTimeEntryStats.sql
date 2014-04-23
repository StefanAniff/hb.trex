
CREATE VIEW [dbo].[UserTimeEntryStats]
AS
SELECT     UserID, SUM(BillableTime) AS TotalBillable, SUM(TimeSpent) AS TotalTimeSpent, COUNT(TimeEntryID) AS NumOfTimeEntries
FROM         dbo.TimeEntries
GROUP BY UserID

