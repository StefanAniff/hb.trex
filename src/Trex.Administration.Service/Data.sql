USE [nameofinstance]
GO

/***Insert default time entry type**/
INSERT INTO [dbo].[TimeEntryTypes] VALUES (null,'Default',1,1)
GO

INSERT INTO [dbo].[Roles] VALUES('Administrator')

GO

INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,1)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,2)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,3)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,4)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,5)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,6)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,7)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,8)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,9)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,10)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,11)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,12)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,13)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,14)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,15)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,16)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,17)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,18)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,19)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,20)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,21)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,22)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,23)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,24)
INSERT INTO [dbo].[PermissionsInRoles] VALUES(1,25)

GO

INSERT INTO [dbo].[Version]
           ([Version]
           ,[Date]
           ,[Creator]
           ,[Description])
     VALUES
           ('2.5.0.15'
           ,GETDATE()
           ,'AutoScript'
           ,'Createscript run')
GO

