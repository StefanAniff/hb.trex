IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Email] [nvarchar](100) NULL,
	[Price] [float] NULL,
	[Inactive] [bit] NOT NULL CONSTRAINT [DF_Users_Inactive]  DEFAULT ((0)),
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UsersProjects]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UsersProjects](
	[ProjectID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_UsersProjects] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Projects]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Projects](
	[ProjectID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] ROWGUIDCOL  NULL CONSTRAINT [DF_Projects_Guid]  DEFAULT (newid()),
	[CustomerID] [int] NOT NULL,
	[ProjectName] [nvarchar](50) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_Projects_CreateDate]  DEFAULT (getdate()),
	[Inactive] [bit] NOT NULL CONSTRAINT [DF_Projects_Inactive]  DEFAULT ((0)),
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Customers_Guid]  DEFAULT (newid()),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_Customers_CreateDate]  DEFAULT (getdate()),
	[CreatedBy] [int] NOT NULL,
	[CustomerName] [nvarchar](250) NOT NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[CellPhoneNumber] [nvarchar](50) NULL,
	[Email] [nvarchar](255) NULL,
	[Inactive] [bit] NOT NULL CONSTRAINT [DF_Customers_Inactive]  DEFAULT ((0)),
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TimeEntries]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TimeEntries](
	[TimeEntryID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[PauseTime] [float] NOT NULL,
	[BillableTime] [float] NOT NULL,
	[BilledDate] [datetime] NULL,
	[Billed] [bit] NOT NULL,
	[Billable] [bit] NOT NULL,
	[Price] [float] NOT NULL,
 CONSTRAINT [PK_Trex.Servers] PRIMARY KEY CLUSTERED 
(
	[TimeEntryID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tasks]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Tasks](
	[TaskID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [nchar](10) NULL,
	[Guid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Tasks_Guid]  DEFAULT (newid()),
	[ProjectID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_Tasks_ModifyDate]  DEFAULT (getdate()),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_Tasks_CreateDate]  DEFAULT (getdate()),
	[TaskName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[TimeEstimated] [float] NOT NULL CONSTRAINT [DF_Tasks_TimeEstimated]  DEFAULT ((0)),
	[TimeLeft] [int] NOT NULL,
	[Closed] [bit] NOT NULL CONSTRAINT [DF_Tasks_Closed]  DEFAULT ((0)),
	[WorstCaseEstimate] [float] NOT NULL CONSTRAINT [DF_Tasks_WorstCaseEstimate]  DEFAULT ((0)),
	[BestCaseEstimate] [float] NOT NULL CONSTRAINT [DF_Tasks_BestCaseEstimate]  DEFAULT ((0)),
	[TagID] [int] NULL,
	[RealisticEstimate] [float] NOT NULL CONSTRAINT [DF_Tasks_RealisticEstimate]  DEFAULT ((0)),
	[Inactive] [bit] NOT NULL CONSTRAINT [DF_Tasks_Inactive]  DEFAULT ((0)),
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UsersCustomers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UsersCustomers](
	[UserID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[Price] [float] NOT NULL,
 CONSTRAINT [PK_UsersCustomers] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[CustomerID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Tags](
	[TagID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[TagText] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TaskTree]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TaskTree](
	[ParentID] [int] NOT NULL,
	[ChildID] [int] NOT NULL,
 CONSTRAINT [PK_TaskTree] PRIMARY KEY CLUSTERED 
(
	[ParentID] ASC,
	[ChildID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersProjects_Projects]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersProjects]'))
ALTER TABLE [dbo].[UsersProjects]  WITH CHECK ADD  CONSTRAINT [FK_UsersProjects_Projects] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Projects] ([ProjectID])
GO
ALTER TABLE [dbo].[UsersProjects] CHECK CONSTRAINT [FK_UsersProjects_Projects]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersProjects_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersProjects]'))
ALTER TABLE [dbo].[UsersProjects]  WITH CHECK ADD  CONSTRAINT [FK_UsersProjects_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UsersProjects] CHECK CONSTRAINT [FK_UsersProjects_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Customers]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projects]'))
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Customers]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Projects_Users1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Projects]'))
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Users1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Users1]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Customers_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Customers]'))
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_Customers_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_Customers_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Trex.Servers_Tasks]') AND parent_object_id = OBJECT_ID(N'[dbo].[TimeEntries]'))
ALTER TABLE [dbo].[TimeEntries]  WITH CHECK ADD  CONSTRAINT [FK_Trex.Servers_Tasks] FOREIGN KEY([TaskID])
REFERENCES [dbo].[Tasks] ([TaskID])
GO
ALTER TABLE [dbo].[TimeEntries] CHECK CONSTRAINT [FK_Trex.Servers_Tasks]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Trex.Servers_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[TimeEntries]'))
ALTER TABLE [dbo].[TimeEntries]  WITH CHECK ADD  CONSTRAINT [FK_Trex.Servers_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[TimeEntries] CHECK CONSTRAINT [FK_Trex.Servers_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Tasks_Projects]') AND parent_object_id = OBJECT_ID(N'[dbo].[Tasks]'))
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Projects] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Projects] ([ProjectID])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Projects]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Tasks_Tags]') AND parent_object_id = OBJECT_ID(N'[dbo].[Tasks]'))
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Tags] FOREIGN KEY([TagID])
REFERENCES [dbo].[Tags] ([TagID])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Tags]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Tasks_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Tasks]'))
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersCustomers_Customers]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersCustomers]'))
ALTER TABLE [dbo].[UsersCustomers]  WITH CHECK ADD  CONSTRAINT [FK_UsersCustomers_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[UsersCustomers] CHECK CONSTRAINT [FK_UsersCustomers_Customers]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersCustomers_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersCustomers]'))
ALTER TABLE [dbo].[UsersCustomers]  WITH CHECK ADD  CONSTRAINT [FK_UsersCustomers_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UsersCustomers] CHECK CONSTRAINT [FK_UsersCustomers_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Tags_Customers]') AND parent_object_id = OBJECT_ID(N'[dbo].[Tags]'))
ALTER TABLE [dbo].[Tags]  WITH CHECK ADD  CONSTRAINT [FK_Tags_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Tags] CHECK CONSTRAINT [FK_Tags_Customers]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TaskTree_Tasks]') AND parent_object_id = OBJECT_ID(N'[dbo].[TaskTree]'))
ALTER TABLE [dbo].[TaskTree]  WITH CHECK ADD  CONSTRAINT [FK_TaskTree_Tasks] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Tasks] ([TaskID])
GO
ALTER TABLE [dbo].[TaskTree] CHECK CONSTRAINT [FK_TaskTree_Tasks]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TaskTree_Tasks1]') AND parent_object_id = OBJECT_ID(N'[dbo].[TaskTree]'))
ALTER TABLE [dbo].[TaskTree]  WITH CHECK ADD  CONSTRAINT [FK_TaskTree_Tasks1] FOREIGN KEY([ChildID])
REFERENCES [dbo].[Tasks] ([TaskID])
GO
ALTER TABLE [dbo].[TaskTree] CHECK CONSTRAINT [FK_TaskTree_Tasks1]
