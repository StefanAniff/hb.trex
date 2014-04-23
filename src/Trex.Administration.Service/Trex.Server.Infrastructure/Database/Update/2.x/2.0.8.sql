
/****** Object:  View [dbo].[TimeEntryView]    Script Date: 01/11/2012 11:36:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[TimeEntryView]
AS
SELECT     te.TimeEntryID, t.TaskName, te.BillableTime, te.TimeSpent, te.StartTime, c.CustomerName, p.ProjectName, u.Name, te.Billable, te.Description, u.Location, 
                      u.Department, te.InvoiceId, te.Price, te.EndTime, te.Guid, te.TimeEntryTypeId, te.CreateDate, te.ClientSourceId, t.TaskID, p.ProjectID, c.CustomerID
FROM         dbo.TimeEntries AS te INNER JOIN
                      dbo.Users AS u ON te.UserID = u.UserID INNER JOIN
                      dbo.Tasks AS t ON te.TaskID = t.TaskID INNER JOIN
                      dbo.Projects AS p ON p.ProjectID = t.ProjectID INNER JOIN
                      dbo.Customers AS c ON c.CustomerID = p.CustomerID

GO

