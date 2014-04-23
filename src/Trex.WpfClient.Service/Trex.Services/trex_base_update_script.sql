/****** Update ******/

USE [trex_base]
GO

/****** Object:  Table [dbo].[ClientApplications]    Script Date: 04/26/2011 11:59:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClientApplications](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ClientApplicationType] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ClientApplications] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO ClientApplications VALUES ('Administration')
INSERT INTO ClientApplications VALUES ('SilverlightClient')
INSERT INTO ClientApplications VALUES ('WpfClient')
INSERT INTO ClientApplications VALUES ('WindowsPhoneClient')

GO


/****** Object:  Table [dbo].[Invitations]    Script Date: 04/26/2011 13:24:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Invitations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InviteeEmail] [nvarchar](100) NOT NULL,
	[CustomerID] [nvarchar](50) NOT NULL,
	[InvitationID] [nvarchar](100) NOT NULL,
	[InvitationDate] [datetime] NOT NULL,
	[IsUsed] [bit] NOT NULL,
	[UsedDate] [datetime] NULL,
 CONSTRAINT [PK_Invitations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/*** drop permissions table***/
DROP TABLE Permissions


/****** Object:  Table [dbo].[Permissions]    Script Date: 04/26/2011 13:27:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Permissions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Permission] [nvarchar](100) NOT NULL,
	[ClientApplicationID] [int] NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[Permissions] VALUES ('ApplicationLoginPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('UserAdministrationPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('EditUserPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('DeactivateUserPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('TaskManagementPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('TimeEntryTypesPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('ReportPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('InvoicePermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('CreateUserPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('DeleteUserPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('UserDeactivatePermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('EditUserPricesPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('CreateCustomerPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('CreateProjectPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('CreateTaskPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('CreateTimeEntryPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('EditCustomerPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('EditProjectPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('EditTaskPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('EditTimeEntryPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('ChangeRolePermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('EditSelfPermission',1)
INSERT INTO [dbo].[Permissions] VALUES ('CreateTaskPermission',2)
INSERT INTO [dbo].[Permissions] VALUES ('CreateTaskPermission',3)
INSERT INTO [dbo].[Permissions] VALUES ('CreateTaskPermission',4)



/****** Alter Customers Table ******/

ALTER TABLE Customers ADD IsActivationEmailSent bit

GO


/****** Object:  StoredProcedure [dbo].[spGetAllPermissions]    Script Date: 04/26/2011 12:10:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[spGetAllPermissions]
@ClientApplicationID int
AS
select Permissions.ID, Permissions.Permission from Permissions where Permissions.ClientApplicationID = @ClientApplicationID

GO

/*** Update Version ***/

           INSERT INTO [Version]
           ([Version]
           ,[Date]
           ,[Creator]
           ,[Description])

     VALUES
           ('0.1.2.0'
           ,GETDATE()
           ,'djo',
           'new tables "Invitations" and "ClientApplications / "Permissions" table update to support different types of applications / new column "IsActivationEmailSent" in "Customers" table / "spGetAllPermissions" update')


