/*71332*/
CREATE VIEW dbo.ContolHours
AS
SELECT dbo.RoundUpToNextQuarter(SUM(A.BillableTime)) AS BillableTime, DATEPART(Day, dbo.ConvertToSmallDate(A.EndTime)) AS EndDay, DATEPART(MONTH, 
               dbo.ConvertToSmallDate(A.EndTime)) AS Month, DATEPART(year, dbo.ConvertToSmallDate(A.EndTime)) AS Year, dbo.ConvertToSmallDate(A.EndTime) 
               AS EndDate, A.Price, B.TaskName, c.ProjectName, D.CustomerName, G.componentname, u.UserName, A.Billable, CAST(MAX(A.CreateDate) AS date) 
               AS LastRegistration, ISNULL(J.fielddata, '') AS OutOfScope
FROM  dbo.TimeEntries AS A INNER JOIN
               dbo.Tasks AS B ON A.TaskID = B.TaskID INNER JOIN
               dbo.Projects AS c ON B.ProjectID = c.ProjectID INNER JOIN
               dbo.Users AS u ON A.UserID = u.UserID INNER JOIN
               dbo.Customers AS D ON D.CustomerID = c.CustomerID LEFT OUTER JOIN
               dbo.BugtrackerIntegrationTask AS E ON E.TrexTaskID = B.TaskID LEFT OUTER JOIN
               GeminiV3.dbo.gemini_issuecomponents AS F ON E.GeminiTaskID = F.issueid LEFT OUTER JOIN
               GeminiV3.dbo.gemini_components AS G ON G.componentid = F.componentid LEFT OUTER JOIN
               dbo.BugtrackerIntegrationTask AS H ON B.TaskID = H.TrexTaskID LEFT OUTER JOIN
               GeminiV3.dbo.gemini_issues AS I ON H.GeminiTaskID = I.issueid LEFT OUTER JOIN
                   (SELECT customfielddataid, customfieldid, userid, projectid, issueid, fielddata, created, tstamp, filedata, numericdata, datedata
                    FROM   GeminiV3.dbo.gemini_customfielddata
                    WHERE (customfieldid = 24)) AS J ON I.issueid = J.issueid
GROUP BY A.Price, B.TaskName, c.ProjectName, D.CustomerName, G.componentname, u.UserName, dbo.ConvertToSmallDate(A.EndTime), A.Billable, J.fielddata
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ContolHours';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'       Begin Table = "G"
            Begin Extent = 
               Top = 140
               Left = 961
               Bottom = 268
               Right = 1173
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "H"
            Begin Extent = 
               Top = 252
               Left = 401
               Bottom = 362
               Right = 663
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "I"
            Begin Extent = 
               Top = 273
               Left = 48
               Bottom = 401
               Right = 264
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "J"
            Begin Extent = 
               Top = 273
               Left = 711
               Bottom = 401
               Right = 911
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1176
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ContolHours';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "A"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 135
               Right = 245
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "B"
            Begin Extent = 
               Top = 7
               Left = 293
               Bottom = 135
               Right = 529
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 7
               Left = 577
               Bottom = 135
               Right = 818
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "u"
            Begin Extent = 
               Top = 7
               Left = 866
               Bottom = 135
               Right = 1055
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "D"
            Begin Extent = 
               Top = 140
               Left = 48
               Bottom = 268
               Right = 353
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "E"
            Begin Extent = 
               Top = 140
               Left = 401
               Bottom = 250
               Right = 663
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "F"
            Begin Extent = 
               Top = 140
               Left = 711
               Bottom = 268
               Right = 913
            End
            DisplayFlags = 280
            TopColumn = 0
         End
  ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ContolHours';

