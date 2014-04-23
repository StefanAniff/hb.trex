USE [trex_base2]
GO

/****** Object:  Table [dbo].[Customers]    Script Date: 03/18/2011 11:42:54 ******/
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
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Permissions]    Script Date: 03/18/2011 11:43:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Permissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Permission] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Version]    Script Date: 03/18/2011 11:43:05 ******/
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

/****** Insert the default Permissions to a Permissions table ******/
INSERT INTO [dbo].[Permissions] VALUES('ApplicationLoginPermission')
INSERT INTO [dbo].[Permissions] VALUES('UserAdministrationPermission')
INSERT INTO [dbo].[Permissions] VALUES('EditUserPermission')
INSERT INTO [dbo].[Permissions] VALUES('DeactivateUserPermission')
INSERT INTO [dbo].[Permissions] VALUES('TaskManagementPermission')
INSERT INTO [dbo].[Permissions] VALUES('TimeEntryTypesPermission')
INSERT INTO [dbo].[Permissions] VALUES('ReportPermission')
INSERT INTO [dbo].[Permissions] VALUES('InvoicePermission')
INSERT INTO [dbo].[Permissions] VALUES('CreateUserPermission')
INSERT INTO [dbo].[Permissions] VALUES('DeleteUserPermission')
INSERT INTO [dbo].[Permissions] VALUES('UserDeactivatePermission')
INSERT INTO [dbo].[Permissions] VALUES('EditUserPricesPermission')
INSERT INTO [dbo].[Permissions] VALUES('CreateCustomerPermission')
INSERT INTO [dbo].[Permissions] VALUES('CreateProjectPermission')
INSERT INTO [dbo].[Permissions] VALUES('CreateTaskPermission')
INSERT INTO [dbo].[Permissions] VALUES('CreateTimeEntryPermission')
INSERT INTO [dbo].[Permissions] VALUES('EditCustomerPermission')
INSERT INTO [dbo].[Permissions] VALUES('EditProjectPermission')
INSERT INTO [dbo].[Permissions] VALUES('EditTaskPermission')
INSERT INTO [dbo].[Permissions] VALUES('EditTimeEntryPermission')
INSERT INTO [dbo].[Permissions] VALUES('ChangeRolePermission')
INSERT INTO [dbo].[Permissions] VALUES('EditSelfPermission')

GO