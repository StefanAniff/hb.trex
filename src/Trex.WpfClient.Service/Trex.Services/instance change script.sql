GO

/****** Object:  Table [dbo].[Roles]    Script Date: 04/11/2011 09:39:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Roles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PermissionsInRoles]    Script Date: 04/11/2011 09:46:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PermissionsInRoles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[PermissionID] [int] NOT NULL,
 CONSTRAINT [PK_PermissionsInRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Insert necessary data ******/

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


/****** Object:  StoredProcedure [dbo].[spRemovePermission]    Script Date: 03/24/2011 10:15:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spRemovePermission]
@role nvarchar(100),
@permission nvarchar(200)
AS
delete from PermissionsInRoles where RoleID=(select ID from Roles where Title=@role) and PermissionID= (select ID from  trex_base.dbo.Permissions where Permission=@permission)

GO

/****** Object:  StoredProcedure [dbo].[spAddPermission]    Script Date: 03/24/2011 10:15:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spAddPermission]
@role nvarchar(100),
@permission nvarchar(200)
AS
insert into PermissionsInRoles values((select ID from Roles where Title=@role),(select ID from  trex_base.dbo.Permissions where Permission=@permission))

GO

/****** Object:  StoredProcedure [dbo].[spGetPermissionsForRoles]    Script Date: 03/24/2011 10:15:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetPermissionsForRoles]
@role nvarchar(100)
AS
select Permissions.Permission from PermissionsInRoles left join Roles on PermissionsInRoles.RoleID = Roles.ID left join trex_base.dbo.Permissions on PermissionsInRoles.PermissionID = trex_base.dbo.Permissions.ID where Roles.Title = @role
GO

/****** Object:  StoredProcedure [dbo].[spDeleteRole]    Script Date: 03/24/2011 10:51:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[spDeleteRole]
@role nvarchar(100)
AS
delete from Roles where Title=@role

GO

/****** Object:  StoredProcedure [dbo].[spCreateRole]    Script Date: 03/24/2011 10:50:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spCreateRole]
@role nvarchar(100)
AS
insert into Roles values(@role)

GO
