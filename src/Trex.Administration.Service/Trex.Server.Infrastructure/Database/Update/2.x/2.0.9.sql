
/****** Object:  View [dbo].[UserTimeEntryStats]    Script Date: 01/11/2012 11:37:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[UserTimeEntryStats]
AS
SELECT     UserID, SUM(BillableTime) AS TotalBillable, SUM(TimeSpent) AS TotalTimeSpent, COUNT(TimeEntryID) AS NumOfTimeEntries
FROM         dbo.TimeEntries
GROUP BY UserID

GO
