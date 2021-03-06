USE [Trex_base]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 12/14/2012 14:49:07 ******/
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
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Permissions] ON
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (1, N'ApplicationLoginPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (2, N'UserAdministrationPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (3, N'EditUserPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (4, N'DeactivateUserPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (5, N'TaskManagementPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (6, N'TimeEntryTypesPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (7, N'ReportPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (8, N'InvoicePermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (9, N'CreateUserPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (10, N'DeleteUserPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (11, N'UserDeactivatePermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (12, N'EditUserPricesPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (13, N'CreateCustomerPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (14, N'CreateProjectPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (15, N'CreateTaskPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (16, N'CreateTimeEntryPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (17, N'EditCustomerPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (18, N'EditProjectPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (19, N'EditTaskPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (20, N'EditTimeEntryPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (21, N'ChangeRolePermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (22, N'EditSelfPermission', 1)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (23, N'CreateTaskPermission', 2)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (24, N'CreateTaskPermission', 3)
INSERT [dbo].[Permissions] ([ID], [Permission], [ClientApplicationID]) VALUES (25, N'CreateTaskPermission', 4)
SET IDENTITY_INSERT [dbo].[Permissions] OFF
/****** Object:  Table [dbo].[Invitations]    Script Date: 12/14/2012 14:49:07 ******/
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
/****** Object:  Table [dbo].[Customers]    Script Date: 12/14/2012 14:49:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](200) NOT NULL,
	[VatNumber] [nvarchar](200) NOT NULL,
	[Country] [nvarchar](200) NOT NULL,
	[Address1] [nvarchar](200) NOT NULL,
	[Address2] [nvarchar](200) NULL,
	[Address3] [nvarchar](200) NULL,
	[Address4] [nvarchar](200) NULL,
	[Address5] [nvarchar](200) NULL,
	[CreatorUserName] [nvarchar](200) NULL,
	[CreatorFullName] [nvarchar](200) NOT NULL,
	[CreatorPhone] [nvarchar](100) NOT NULL,
	[CreatorEmail] [nvarchar](100) NULL,
	[CustomerId] [nvarchar](50) NOT NULL,
	[ConnectionString] [nvarchar](300) NULL,
	[Inactive] [bit] NOT NULL,
	[InactiveDate] [datetime] NULL,
	[IsLockedOut] [bit] NOT NULL,
	[LockedOutDate] [datetime] NULL,
	[CreateDate] [datetime] NOT NULL,
	[IsActivated] [bit] NOT NULL,
	[ActivationId] [nvarchar](100) NULL,
	[IsActivationEmailSent] [bit] NULL,
	[City] [nvarchar](200) NULL,
	[Zipcode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ClientApplications]    Script Date: 12/14/2012 14:49:07 ******/
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
SET IDENTITY_INSERT [dbo].[ClientApplications] ON
INSERT [dbo].[ClientApplications] ([ID], [ClientApplicationType]) VALUES (1, N'Administration')
INSERT [dbo].[ClientApplications] ([ID], [ClientApplicationType]) VALUES (2, N'SilverlightClient')
INSERT [dbo].[ClientApplications] ([ID], [ClientApplicationType]) VALUES (3, N'WpfClient')
INSERT [dbo].[ClientApplications] ([ID], [ClientApplicationType]) VALUES (4, N'WindowsPhoneClient')
SET IDENTITY_INSERT [dbo].[ClientApplications] OFF
/****** Object:  Table [dbo].[Version]    Script Date: 12/14/2012 14:49:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Version](
	[Version] [nvarchar](50) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Creator] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL
) ON [PRIMARY]
GO
INSERT [dbo].[Version] ([Version], [Date], [Creator], [Description]) VALUES (N'0.1.1.0', CAST(0x00009EB6011F4852 AS DateTime), N'djo', N'new stored procedure spGetAllPermissions')
INSERT [dbo].[Version] ([Version], [Date], [Creator], [Description]) VALUES (N'0.1.2.0', CAST(0x00009ED901329BF4 AS DateTime), N'djo', N'new tables "Invitations" and "ClientApplications / "Permissions" table update to support different types of applications / new column "IsActivationEmailSent" in "Customers" table / "spGetAllPermissions" update')
INSERT [dbo].[Version] ([Version], [Date], [Creator], [Description]) VALUES (N'0.1.1.0', CAST(0x00009F4800ECDC76 AS DateTime), N'djo', N'new stored procedure spGetAllPermissions')
INSERT [dbo].[Version] ([Version], [Date], [Creator], [Description]) VALUES (N'0.1.2.0', CAST(0x00009F4800ECF029 AS DateTime), N'djo', N'new stored procedure spGetConnection')
/****** Object:  StoredProcedure [dbo].[spGetConnection]    Script Date: 12/14/2012 14:49:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetConnection]
@CustomerID varchar(100)
AS
SELECT connectionstring FROM customers WHERE CustomerId = @CustomerID
GO
/****** Object:  StoredProcedure [dbo].[spGetAllPermissions]    Script Date: 12/14/2012 14:49:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetAllPermissions]
@ClientApplicationID int
AS
select Permissions.ID, Permissions.Permission from Permissions where Permissions.ClientApplicationID = @ClientApplicationID
GO
