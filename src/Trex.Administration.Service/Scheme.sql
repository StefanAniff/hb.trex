USE [nameofinstance]
GO
/****** Object:  UserDefinedFunction [dbo].[ConvertToSmallDate]    Script Date: 02/01/2012 09:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ConvertToSmallDate]
(
	@DateTime DATETIME
)
RETURNS DATETIME
AS
BEGIN
	
	 RETURN DATEADD(dd, 0, DATEDIFF(dd, 0, @DateTime)) 
END
GO
/****** Object:  Table [dbo].[BugtrackerTimeEntryExport]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BugtrackerTimeEntryExport](
	[BugtrackerTimeEntryExportID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[TaskID] [int] NOT NULL,
	[TimeEntryID] [int] NOT NULL,
	[GeminiIssueID] [numeric](10, 0) NOT NULL,
	[GeminiTimeLogID] [numeric](10, 0) NOT NULL,
	[ExportDate] [datetime] NOT NULL,
 CONSTRAINT [PK_BugtrackerTimeEntryExport] PRIMARY KEY CLUSTERED 
(
	[BugtrackerTimeEntryExportID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BugtrackerTaskImport]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BugtrackerTaskImport](
	[BugtrackerTaskImportID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NOT NULL,
	[GeminiIssueID] [numeric](10, 0) NOT NULL,
	[ImportDate] [datetime] NOT NULL,
 CONSTRAINT [PK_BugtrackerTaskImport] PRIMARY KEY CLUSTERED 
(
	[BugtrackerTaskImportID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BugtrackerIntegrationUser]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BugtrackerIntegrationUser](
	[BugtrackerIntegrationUserID] [int] IDENTITY(1,1) NOT NULL,
	[TrexUserID] [int] NOT NULL,
	[GeminiUserID] [numeric](10, 0) NOT NULL,
 CONSTRAINT [PK_BugtrackerIntegrationUser] PRIMARY KEY CLUSTERED 
(
	[BugtrackerIntegrationUserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BugtrackerIntegrationTask]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BugtrackerIntegrationTask](
	[BugtrackerIntegrationTaskID] [int] IDENTITY(1,1) NOT NULL,
	[TrexTaskID] [int] NOT NULL,
	[GeminiTaskID] [numeric](10, 0) NOT NULL,
 CONSTRAINT [PK_BugtrackerIntegrationTask] PRIMARY KEY CLUSTERED 
(
	[BugtrackerIntegrationTaskID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BugtrackerIntegrationProject]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BugtrackerIntegrationProject](
	[BugtrackerIntegrationProjectID] [int] NOT NULL,
	[TrexProjectID] [int] NOT NULL,
	[GeminiProjectID] [numeric](10, 0) NOT NULL,
	[TwoWaySync] [bit] NOT NULL,
 CONSTRAINT [PK_BugtrackerIntegration] PRIMARY KEY CLUSTERED 
(
	[BugtrackerIntegrationProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 02/01/2012 09:20:45 ******/
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
/****** Object:  Table [dbo].[PermissionsInRoles]    Script Date: 02/01/2012 09:20:45 ******/
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
/****** Object:  Table [dbo].[InvoiceTemplates]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceTemplates](
	[TemplateId] [int] IDENTITY(1,1) NOT NULL,
	[TemplateName] [nvarchar](250) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[FilePath] [nvarchar](250) NULL,
	[Guid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_InvoiceTemplates] PRIMARY KEY CLUSTERED 
(
	[TemplateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[RoundUpToNextQuarter]    Script Date: 02/01/2012 09:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[RoundUpToNextQuarter]
(
	@float float
	
)
RETURNS FLOAT
AS
BEGIN
	
DECLARE @returnFloat float

SELECT @returnFloat =CEILING(@float*4)/4
RETURN @ReturnFloat
	 
END
GO
/****** Object:  UserDefinedFunction [dbo].[RoundUpToNextHalfHour]    Script Date: 02/01/2012 09:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[RoundUpToNextHalfHour]
(
	@float float
	
)
RETURNS FLOAT
AS
BEGIN
	
DECLARE @returnFloat float

SELECT @returnFloat =CEILING(@float*2)/2
RETURN @ReturnFloat
	 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 02/01/2012 09:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Split] 
   (  @Delimiter varchar(5), 
      @List      varchar(8000)
   ) 
   RETURNS @TableOfValues table 
      (  RowID   smallint IDENTITY(1,1), 
         SeparatedValue varchar(50) 
      ) 
AS 
   BEGIN
    
      DECLARE @LenString int 
 
      WHILE len( @List ) > 0 
         BEGIN 
         
            SELECT @LenString = 
               (CASE charindex( @Delimiter, @List ) 
                   WHEN 0 THEN len( @List ) 
                   ELSE ( charindex( @Delimiter, @List ) -1 )
                END
               ) 
                                
            INSERT INTO @TableOfValues 
               SELECT substring( @List, 1, @LenString )
                
            SELECT @List = 
               (CASE ( len( @List ) - @LenString ) 
                   WHEN 0 THEN '' 
                   ELSE right( @List, len( @List ) - @LenString - 1 ) 
                END
               ) 
         END
          
      RETURN 
      
   END
GO
/****** Object:  Table [dbo].[Users]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Email] [nvarchar](100) NULL,
	[Price] [float] NOT NULL,
	[Inactive] [bit] NOT NULL,
	[Department] [nvarchar](255) NULL,
	[Location] [nvarchar](255) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Version]    Script Date: 02/01/2012 09:20:45 ******/
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
/****** Object:  StoredProcedure [dbo].[spAddPermission]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAddPermission]
@RoleName nvarchar(100),
@PermissionID int
AS
insert into PermissionsInRoles values((select ID from Roles where Title=@RoleName),@PermissionID)
GO
/****** Object:  StoredProcedure [dbo].[spRemovePermission]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spRemovePermission]
@RoleName nvarchar(100),
@PermissionID int
AS
delete from PermissionsInRoles where RoleID=(select ID from Roles where Title=@RoleName) and PermissionID = @PermissionID
GO
/****** Object:  StoredProcedure [dbo].[spGetPermissionsForRoles]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetPermissionsForRoles]
@RoleName nvarchar(100),
@ClientApplicationID int
AS
SELECT PermissionID, ps.Permission from PermissionsInRoles as pr
	join 
		Roles as r on r.ID = pr.RoleID
	join
		trex_base.dbo.Permissions as ps on ps.ID = pr.PermissionID
	where r.Title= @RoleName and ps.ClientApplicationID = @ClientApplicationID
GO
/****** Object:  StoredProcedure [dbo].[spDeleteRole]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDeleteRole]
@role nvarchar(100)
AS
delete from Roles where Title=@role
GO
/****** Object:  StoredProcedure [dbo].[spCreateRole]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCreateRole]
@role nvarchar(100)
AS
insert into Roles values(@role)
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CustomerName] [nvarchar](250) NOT NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[Email] [nvarchar](255) NULL,
	[Inactive] [bit] NOT NULL,
	[StreetAddress] [nvarchar](400) NULL,
	[ZipCode] [nvarchar](50) NULL,
	[City] [nvarchar](100) NULL,
	[Country] [nvarchar](200) NULL,
	[ContactName] [nvarchar](200) NULL,
	[ContactPhone] [nvarchar](100) NULL,
	[InheritsTimeEntryTypes] [bit] NOT NULL,
	[ChangeDate] [datetime] NULL,
	[ChangedBy] [int] NULL,
	[PaymentTermsNumberOfDays] [int] NOT NULL,
	[PaymentTermsIncludeCurrentMonth] [bit] NOT NULL,
	[Address2] [nvarchar](400) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoices]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoices](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[InvoiceDate] [datetime] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CustomerName] [nvarchar](200) NULL,
	[StreetAddress] [nvarchar](400) NULL,
	[ZipCode] [nvarchar](50) NULL,
	[City] [nvarchar](100) NULL,
	[Country] [nvarchar](200) NULL,
	[Attention] [nvarchar](300) NULL,
	[VAT] [float] NOT NULL,
	[FooterText] [nvarchar](1000) NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Closed] [bit] NOT NULL,
	[DueDate] [datetime] NULL,
	[Regarding] [nvarchar](100) NULL,
	[Address2] [nvarchar](400) NULL,
 CONSTRAINT [PK_Invoices] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[ProjectID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[CustomerID] [int] NOT NULL,
	[ProjectName] [nvarchar](50) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Inactive] [bit] NOT NULL,
	[IsEstimatesEnabled] [bit] NOT NULL,
	[ChangeDate] [datetime] NULL,
	[ChangedBy] [int] NULL,
	[PONumber] [nvarchar](50) NULL,
	[FixedPriceProject] [bit] NOT NULL,
	[FixedPrice] [decimal](18, 0) NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceLines]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceLines](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](1000) NOT NULL,
	[PricePrUnit] [float] NOT NULL,
	[InvoiceID] [int] NOT NULL,
	[Units] [float] NOT NULL,
	[Unit] [nvarchar](50) NULL,
	[UnitType] [int] NOT NULL,
	[SortIndex] [int] NOT NULL,
	[IsExpense] [bit] NOT NULL,
	[VatPercentage] [float] NOT NULL,
 CONSTRAINT [PK_InvoiceLines] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TimeEntryTypes]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimeEntryTypes](
	[TimeEntryTypeId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[IsBillableByDefault] [bit] NOT NULL,
 CONSTRAINT [PK_TimeEntryTypes] PRIMARY KEY CLUSTERED 
(
	[TimeEntryTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[TagID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[TagText] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersCustomers]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersCustomers](
	[UserID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[Price] [float] NOT NULL,
 CONSTRAINT [PK_UsersCustomers] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[CustomerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[TaskID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [nchar](10) NULL,
	[Guid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ProjectID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[TaskName] [nvarchar](350) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[TimeEstimated] [float] NOT NULL,
	[TimeLeft] [float] NOT NULL,
	[Closed] [bit] NOT NULL,
	[WorstCaseEstimate] [float] NOT NULL,
	[BestCaseEstimate] [float] NOT NULL,
	[TagID] [int] NULL,
	[RealisticEstimate] [float] NOT NULL,
	[Inactive] [bit] NOT NULL,
	[ChangeDate] [datetime] NULL,
	[ChangedBy] [int] NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersProjects]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersProjects](
	[ProjectID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_UsersProjects] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[InsertGeminiTask]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[InsertGeminiTask]
	 @geminiProjectID numeric(10,0) ,
@geminiIssueID numeric(10,0),
 @summary nvarchar(255),
 @projectCode nvarchar(10),
 @geminiUserID numeric(10,0)
 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @TrexProjectID int =0
	declare @trextaskID int = 0
 select  @TrexProjectID=[TrexProjectID]
  FROM [dbo].[BugtrackerIntegrationProject] where [GeminiProjectID] = @geminiProjectID
  
  if @TrexProjectID <> 0
  begin
  if not exists  ( select     [TrexTaskID]
      
  FROM [dbo].[BugtrackerIntegrationTask] where [GeminiTaskID]= @geminiIssueID)
 begin
 declare @trexUserID int = 2
	SELECT      @trexUserID = [TrexUserID]
   FROM [dbo].[BugtrackerIntegrationUser] where GeminiUserID = @geminiUserID
 
 
 INSERT INTO [dbo].[Tasks]
           ([ParentID]
           ,[Guid]
           ,[ProjectID]
           ,[CreatedBy]
           ,[ModifyDate]
           ,[CreateDate]
           ,[TaskName]
           ,[Description]
           ,[TimeEstimated]
           ,[TimeLeft]
           ,[Closed]
           ,[WorstCaseEstimate]
           ,[BestCaseEstimate]
           ,[TagID]
           ,[RealisticEstimate]
           ,[Inactive]
           ,[ChangeDate]
           ,[ChangedBy])
     VALUES
           (null
           ,NEWID()
           ,@TrexProjectID
           ,@trexUserID
           ,GETDATE()
           ,GETDATE()
           ,@projectCode +'-'+ cast(@geminiIssueID as nvarchar)+ ' '+@summary
           ,'Created from gemini'
           ,0
           ,0
           ,0
           ,0
           ,0
           ,null
           ,0
           ,0
           ,null
           ,null)
          select @trextaskID = cast(@@IDENTITY  as int)
           
           INSERT INTO [dbo].[BugtrackerIntegrationTask]
           ([TrexTaskID]
           ,[GeminiTaskID])
     VALUES
           (@trextaskID
           ,@geminiIssueID)
           
           
 end
  
end
	
END
GO
/****** Object:  Table [dbo].[TimeEntries]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimeEntries](
	[TimeEntryID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[PauseTime] [float] NOT NULL,
	[BillableTime] [float] NOT NULL,
	[Billable] [bit] NOT NULL,
	[Price] [float] NOT NULL,
	[TimeSpent] [float] NOT NULL,
	[InvoiceId] [int] NULL,
	[Guid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[TimeEntryTypeId] [int] NOT NULL,
	[ChangeDate] [datetime] NULL,
	[ChangedBy] [int] NULL,
	[CreateDate] [datetime] NULL,
	[ClientSourceId] [int] NOT NULL,
 CONSTRAINT [PK_TimeRegs] PRIMARY KEY CLUSTERED 
(
	[TimeEntryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskTree]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskTree](
	[ParentID] [int] NOT NULL,
	[ChildID] [int] NOT NULL,
 CONSTRAINT [PK_TaskTree] PRIMARY KEY CLUSTERED 
(
	[ParentID] ASC,
	[ChildID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewInventoryValue]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewInventoryValue]
AS
SELECT     dbo.Customers.CustomerName, dbo.Users.Name, dbo.Projects.ProjectName, dbo.TimeEntries.EndTime, dbo.TimeEntries.Description, 
                      dbo.TimeEntries.BillableTime, dbo.TimeEntries.Price, dbo.Tasks.TaskName, dbo.TimeEntries.BillableTime * dbo.TimeEntries.Price AS InventoryValue, 
                      dbo.TimeEntries.TimeEntryID, dbo.Tasks.TaskID
FROM         dbo.TimeEntries INNER JOIN
                      dbo.Users ON dbo.TimeEntries.UserID = dbo.Users.UserID INNER JOIN
                      dbo.Tasks ON dbo.TimeEntries.TaskID = dbo.Tasks.TaskID INNER JOIN
                      dbo.Projects ON dbo.Tasks.ProjectID = dbo.Projects.ProjectID INNER JOIN
                      dbo.Customers ON dbo.Projects.CustomerID = dbo.Customers.CustomerID
WHERE     (dbo.TimeEntries.Billable = 1) AND (dbo.TimeEntries.InvoiceId IS NULL)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[27] 4[34] 2[20] 3) )"
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
         Begin Table = "TimeEntries"
            Begin Extent = 
               Top = 6
               Left = 418
               Bottom = 114
               Right = 569
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Users"
            Begin Extent = 
               Top = 6
               Left = 229
               Bottom = 114
               Right = 380
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Tasks"
            Begin Extent = 
               Top = 6
               Left = 607
               Bottom = 114
               Right = 781
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Projects"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Customers"
            Begin Extent = 
               Top = 6
               Left = 819
               Bottom = 114
               Right = 972
            End
            DisplayFlags = 280
            TopColumn = 3
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 36
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2880
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width =' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewInventoryValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewInventoryValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewInventoryValue'
GO
/****** Object:  View [dbo].[UserTimeEntryStats]    Script Date: 02/01/2012 09:20:45 ******/
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
/****** Object:  StoredProcedure [dbo].[spGetCustomerInvoiceView]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetCustomerInvoiceView] @endDate DATETIME
AS

select c1.customerId,c1.CustomerName,  FirstDateNotInvoiced =(

select top 1 te.StartTime from timeentries te
inner join Tasks t on t.taskid = te.TaskID
inner join Projects p on p.ProjectID = t.ProjectID
inner join Customers c on c.CustomerID = p.CustomerID
where te.Billable = 1 and te.InvoiceId is null
and c.CustomerID = c1.CustomerID
Order by te.StartTime asc)

,DistinctPrice = (
select count(distinct te.Price) from TimeEntries te 
inner join Tasks t on t.taskid = te.TaskID
inner join Projects p on p.ProjectID = t.ProjectID
inner join Customers c on c.CustomerID = p.CustomerID
where te.Billable = 1 and te.StartTime < @endDate
and c.CustomerID =c1.CustomerID
group by c.CustomerID

)

,InventoryValue = (
select sum( te.Price * te.BillableTime) from TimeEntries te 
inner join Tasks t on t.taskid = te.TaskID
inner join Projects p on p.ProjectID = t.ProjectID
inner join Customers c on c.CustomerID = p.CustomerID
where te.Billable = 1 and te.StartTime < @endDate
and c.CustomerID =c1.CustomerID and te.InvoiceId is null
group by c.CustomerID

)


from Customers c1
GO
/****** Object:  StoredProcedure [dbo].[spRoundedTimeSpentPrTaskPrDayPrUserPrPrice]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spRoundedTimeSpentPrTaskPrDayPrUserPrPrice] 
@customerId INT, @startdate DATETIME, @enddate DATETIME,@billable BIT
AS

         SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, c.CustomerID AS CustomerID, p.ProjectName AS Project, p.ProjectID, t.TaskName AS Task, t.TaskID,
                                                                   ISNULL(dbo.RoundUpToNextQuarter(SUM(te.BillableTime)),0) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate, te.Price
                                                                    
           INTO #temp
           FROM   dbo.Customers AS c
           INNER JOIN dbo.Projects AS p ON p.CustomerID = c.CustomerID 
           INNER JOIN dbo.Tasks AS t ON t.ProjectID = p.ProjectID
           LEFT JOIN dbo.TimeEntries AS te ON te.TaskID = t.TaskID
           LEFT JOIN dbo.Users AS u ON u.UserID = te.UserID
                     
           WHERE COALESCE(te.StartTime,@startdate) >= @startdate 
           AND COALESCE(te.endtime,@enddate) <= @enddate
           AND c.CustomerID = @customerId
           AND COALESCE(te.Billable ,1) = @billable
           
           GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, c.CustomerID, p.ProjectName,p.ProjectID, t.TaskName,t.TaskID, t.TimeLeft, t.TimeEstimated, te.Price

           --Then, sum the calculated timeentries, grouped by task
           SELECT     Customer,CustomerID, Project,ProjectID, Task,TaskID, SUM(TimeSpent) AS TimeSpent, Remaining, Estimate, Price
           FROM       #temp
           GROUP BY Customer,CustomerID, Project, ProjectID, Task,TaskID, Remaining, Estimate, Price

           DROP TABLE #temp
GO
/****** Object:  StoredProcedure [dbo].[spRoundedTimeSpentPrTaskPrDayPrUser]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spRoundedTimeSpentPrTaskPrDayPrUser] 
@customerId INT, @startdate DATETIME, @enddate DATETIME,@billable BIT
AS

         SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, c.CustomerID AS CustomerID, p.ProjectName AS Project, p.ProjectID, t.TaskName AS Task, t.TaskID,
                                                                   ISNULL(dbo.RoundUpToNextQuarter(SUM(te.BillableTime)),0) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate
                                                                    
           INTO #temp
           FROM   dbo.Customers AS c
           INNER JOIN dbo.Projects AS p ON p.CustomerID = c.CustomerID 
           INNER JOIN dbo.Tasks AS t ON t.ProjectID = p.ProjectID
           LEFT JOIN dbo.TimeEntries AS te ON te.TaskID = t.TaskID
           LEFT JOIN dbo.Users AS u ON u.UserID = te.UserID
                     
           WHERE COALESCE(te.StartTime,@startdate) >= @startdate 
           AND COALESCE(te.endtime,@enddate) <= @enddate
           AND c.CustomerID = @customerId
           AND COALESCE(te.Billable ,1) = @billable
           
           GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, c.CustomerID, p.ProjectName,p.ProjectID, t.TaskName,t.TaskID, t.TimeLeft, t.TimeEstimated

           --Then, sum the calculated timeentries, grouped by task
           SELECT     Customer,CustomerID, Project,ProjectID, Task,TaskID, SUM(TimeSpent) AS TimeSpent, Remaining, Estimate
           FROM       #temp
           GROUP BY Customer,CustomerID, Project, ProjectID, Task,TaskID, Remaining, Estimate

           DROP TABLE #temp
GO
/****** Object:  StoredProcedure [dbo].[spResetBookedTimeEntries]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spResetBookedTimeEntries] @invoiceId INT
AS

BEGIN TRANSACTION

BEGIN TRY
	UPDATE TimeEntries
	SET InvoiceID = NULL
	WHERE InvoiceID = @invoiceId

	DELETE FROM InvoiceLines WHERE InvoiceID= @invoiceId AND UnitType = 0
	
	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION;

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

END CATCH
GO
/****** Object:  View [dbo].[TimeEntryView]    Script Date: 02/01/2012 09:20:45 ******/
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
/****** Object:  View [dbo].[RoundedTimeSpentPrDayPrUserHalfHour]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[RoundedTimeSpentPrDayPrUserHalfHour]
AS
SELECT     dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, p.ProjectName AS Project, t.TaskName AS Task, 
                      dbo.[RoundUpToNextHalfHour](SUM(te.BillableTime)) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate, u.UserName, c.CustomerID, p.ProjectID, 
                      u.UserID, u.Name, te.Billable
FROM         dbo.TimeEntries AS te INNER JOIN
                      dbo.Tasks AS t ON t.TaskID = te.TaskID INNER JOIN
                      dbo.Projects AS p ON p.ProjectID = t.ProjectID INNER JOIN
                      dbo.Customers AS c ON c.CustomerID = p.CustomerID INNER JOIN
                      dbo.Users AS u ON u.UserID = te.UserID
GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName, t.TimeLeft, t.TimeEstimated, u.UserName, c.CustomerID, p.ProjectID, 
                      u.UserID, u.Name, te.Billable
GO
/****** Object:  View [dbo].[RoundedTimeSpentPrDayPrUserEx]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[RoundedTimeSpentPrDayPrUserEx]
AS
SELECT     dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, p.ProjectName AS Project, t.TaskName AS Task,t.TaskID, 
                      dbo.RoundUpToNextQuarter(SUM(te.BillableTime)) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate, u.UserName, c.CustomerID, p.ProjectID, 
                      u.UserID, u.Name, te.Billable,te.[InvoiceId],te.price
FROM         dbo.TimeEntries AS te INNER JOIN
                      dbo.Tasks AS t ON t.TaskID = te.TaskID INNER JOIN
                      dbo.Projects AS p ON p.ProjectID = t.ProjectID INNER JOIN
                      dbo.Customers AS c ON c.CustomerID = p.CustomerID INNER JOIN
                      dbo.Users AS u ON u.UserID = te.UserID
GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName,t.TaskID, t.TimeLeft, t.TimeEstimated, u.UserName, c.CustomerID, p.ProjectID, 
                      u.UserID, u.Name, te.Billable,te.[InvoiceId],te.price
GO
/****** Object:  View [dbo].[RoundedTimeSpentPrDayPrUser]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[RoundedTimeSpentPrDayPrUser]
AS
SELECT     dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, p.ProjectName AS Project, t.TaskName AS Task, 
                      dbo.RoundUpToNextQuarter(SUM(te.BillableTime)) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate, u.UserName, c.CustomerID, p.ProjectID, 
                      u.UserID, u.Name, te.Billable, te.Price, te.InvoiceId
FROM         dbo.TimeEntries AS te INNER JOIN
                      dbo.Tasks AS t ON t.TaskID = te.TaskID INNER JOIN
                      dbo.Projects AS p ON p.ProjectID = t.ProjectID INNER JOIN
                      dbo.Customers AS c ON c.CustomerID = p.CustomerID INNER JOIN
                      dbo.Users AS u ON u.UserID = te.UserID
GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName, t.TimeLeft, t.TimeEstimated, u.UserName, c.CustomerID, p.ProjectID, 
                      u.UserID, u.Name, te.Billable, te.Price, te.InvoiceId
GO
/****** Object:  StoredProcedure [dbo].[spBookTimeEntries]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spBookTimeEntries] @invoiceId INT, @timeEntryList NVARCHAR(MAX)=NULL
AS

UPDATE timeentries
SET InvoiceID = @invoiceID
WHERE timeentryid IN
(
	SELECT te.timeentryid
		FROM         dbo.TimeEntries AS te 
		WHERE te.TimeEntryID IN (SELECT SeparatedValue FROM dbo.Split(',',@timeEntryList))
)
GO
/****** Object:  StoredProcedure [dbo].[spAggregatedTimeEntriesPrTaskPrDayPrInvoice]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAggregatedTimeEntriesPrTaskPrDayPrInvoice] 
@InvoiceId INT
AS

	--First, sum all timeentries,grouped by date, and round time to nearest quarter
	SELECT   dbo.ConvertToSmallDate(te.StartTime) AS TaskDate, c.CustomerName AS Customer,c.customerId ,p.ProjectName AS Project, t.TaskName AS Task, 
						  dbo.RoundUpToNextQuarter(SUM(te.BillableTime)) AS TimeSpent,
					i.ID AS InvoiceNumber, i.InvoiceDate, i.StreetAddress, i.ZipCode, i.City, i.Country, i.Attention, i.StartDate, i.EndDate, i.DueDate,u.UserID
	INTO #temp
	FROM         dbo.TimeEntries AS te 
				INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
				INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
				INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
				INNER JOIN dbo.Invoices AS i ON te.InvoiceId = i.ID
				INNER JOIN dbo.Users AS u ON u.UserID = te.UserID
	WHERE i.ID = @InvoiceId
	
	GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName,c.customerId ,i.ID, i.InvoiceDate, i.StreetAddress, i.ZipCode, i.City, i.Country, i.Attention, i.StartDate, i.EndDate, i.DueDate,u.UserID

	--Then, sum the calculated timeentries, grouped by task
	SELECT     Customer AS CustomerName, Project, Task, SUM(TimeSpent) AS TimeSpent, InvoiceNumber, InvoiceDate,customerId AS CustomerNumber, StreetAddress, ZipCode, City,Country,Attention AS ContactName,StartDate, EndDate,DueDate
	FROM       #temp
	GROUP BY Customer, Project, Task,InvoiceNumber, InvoiceDate,CustomerId, StreetAddress, ZipCode, City,Country,Attention,StartDate, EndDate,DueDate

	DROP TABLE #temp
GO
/****** Object:  StoredProcedure [dbo].[spAggregatedTimeEntriesPrTaskPrDay]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAggregatedTimeEntriesPrTaskPrDay] 
@customerId INT, @startdate DATETIME, @enddate DATETIME, @billable BIT
AS

           --First, sum all timeentries,grouped by date, and round time to nearest quarter
           SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, p.ProjectName AS Project, t.TaskName AS Task, 
                                                                   dbo.RoundUpToNextQuarter(SUM(te.BillableTime)) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate
                                                                    
           INTO #temp
           FROM         dbo.TimeEntries AS te 
                                            INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
                                            INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
                                            INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
                                            INNER JOIN dbo.Users AS u ON u.UserID = te.UserID
           WHERE te.StartTime >= @startdate AND te.endtime <= @enddate
           AND c.CustomerID = @customerId
           AND te.Billable = @billable
           GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName, t.TimeLeft, t.TimeEstimated

           --Then, sum the calculated timeentries, grouped by task
           SELECT     Customer, Project, Task, SUM(TimeSpent) AS TimeSpent, Remaining, Estimate
           FROM       #temp
           GROUP BY Customer, Project, Task, Remaining, Estimate

           DROP TABLE #temp
GO
/****** Object:  StoredProcedure [dbo].[spGetGeneratedInvoiceLines]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetGeneratedInvoiceLines] @InvoiceId INT

AS


SELECT 
      SUM(TimeSpent) BillableTime,[Price] as Price
     
      
  FROM [dbo].[RoundedTimeSpentPrDayPrUser]
  where InvoiceId = @InvoiceId
  GROUP BY Price
GO
/****** Object:  View [dbo].[RoundedTimeSpentPrTaskPrDay]    Script Date: 02/01/2012 09:20:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[RoundedTimeSpentPrTaskPrDay]
AS
SELECT     Customer, Project, Task, SUM(TimeSpent) AS TimeSpent, Remaining, Estimate, CustomerID, ProjectID, StartDate
FROM         dbo.RoundedTimeSpentPrDayPrUser
GROUP BY Customer, Project, Task, Remaining, Estimate, CustomerID, ProjectID, StartDate
GO
/****** Object:  Default [DF_BugtrackerIntegrationProject_TwoWaySync]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[BugtrackerIntegrationProject] ADD  CONSTRAINT [DF_BugtrackerIntegrationProject_TwoWaySync]  DEFAULT ((0)) FOR [TwoWaySync]
GO
/****** Object:  Default [DF_Customers_Guid]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_Guid]  DEFAULT (newid()) FOR [Guid]
GO
/****** Object:  Default [DF_Customers_CreateDate]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Customers_Inactive]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_Inactive]  DEFAULT ((0)) FOR [Inactive]
GO
/****** Object:  Default [DF_Customers_InheritsTimeEntryTypes]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_InheritsTimeEntryTypes]  DEFAULT ((1)) FOR [InheritsTimeEntryTypes]
GO
/****** Object:  Default [DF_Customers_PaymentTermsNumberOfDays]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_PaymentTermsNumberOfDays]  DEFAULT ((0)) FOR [PaymentTermsNumberOfDays]
GO
/****** Object:  Default [DF_Customers_PaymentTermsIncludeCurrentMonth]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_PaymentTermsIncludeCurrentMonth]  DEFAULT ((1)) FOR [PaymentTermsIncludeCurrentMonth]
GO
/****** Object:  Default [DF_InvoiceLines_UnitType]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[InvoiceLines] ADD  CONSTRAINT [DF_InvoiceLines_UnitType]  DEFAULT ((1)) FOR [UnitType]
GO
/****** Object:  Default [DF_InvoiceLines_SortIndex]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[InvoiceLines] ADD  CONSTRAINT [DF_InvoiceLines_SortIndex]  DEFAULT ((0)) FOR [SortIndex]
GO
/****** Object:  Default [DF_InvoiceLines_IsExpense]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[InvoiceLines] ADD  CONSTRAINT [DF_InvoiceLines_IsExpense]  DEFAULT ((0)) FOR [IsExpense]
GO
/****** Object:  Default [DF_InvoiceLines_VatPercentage]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[InvoiceLines] ADD  CONSTRAINT [DF_InvoiceLines_VatPercentage]  DEFAULT ((0.25)) FOR [VatPercentage]
GO
/****** Object:  Default [DF_Invoices_InvoiceDate]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Invoices] ADD  CONSTRAINT [DF_Invoices_InvoiceDate]  DEFAULT (getdate()) FOR [InvoiceDate]
GO
/****** Object:  Default [DF_Projects_Guid]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_Guid]  DEFAULT (newid()) FOR [Guid]
GO
/****** Object:  Default [DF_Projects_CreateDate]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Projects_Inactive]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_Inactive]  DEFAULT ((0)) FOR [Inactive]
GO
/****** Object:  Default [DF_Projects_IsEstimatesEnabled]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_IsEstimatesEnabled]  DEFAULT ((0)) FOR [IsEstimatesEnabled]
GO
/****** Object:  Default [DF_Projects_FixedPriceProject]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_FixedPriceProject]  DEFAULT ((0)) FOR [FixedPriceProject]
GO
/****** Object:  Default [DF_Tasks_Guid]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_Guid]  DEFAULT (newid()) FOR [Guid]
GO
/****** Object:  Default [DF_Tasks_ModifyDate]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_ModifyDate]  DEFAULT (getdate()) FOR [ModifyDate]
GO
/****** Object:  Default [DF_Tasks_CreateDate]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Tasks_TimeEstimated]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_TimeEstimated]  DEFAULT ((0)) FOR [TimeEstimated]
GO
/****** Object:  Default [DF_Tasks_Closed]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_Closed]  DEFAULT ((0)) FOR [Closed]
GO
/****** Object:  Default [DF_Tasks_WorstCaseEstimate]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_WorstCaseEstimate]  DEFAULT ((0)) FOR [WorstCaseEstimate]
GO
/****** Object:  Default [DF_Tasks_BestCaseEstimate]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_BestCaseEstimate]  DEFAULT ((0)) FOR [BestCaseEstimate]
GO
/****** Object:  Default [DF_Tasks_RealisticEstimate]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_RealisticEstimate]  DEFAULT ((0)) FOR [RealisticEstimate]
GO
/****** Object:  Default [DF_Tasks_Inactive]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_Inactive]  DEFAULT ((0)) FOR [Inactive]
GO
/****** Object:  Default [DF_TimeEntries_TimeSpent]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[TimeEntries] ADD  CONSTRAINT [DF_TimeEntries_TimeSpent]  DEFAULT ((0)) FOR [TimeSpent]
GO
/****** Object:  Default [DF_TimeEntries_Guid]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[TimeEntries] ADD  CONSTRAINT [DF_TimeEntries_Guid]  DEFAULT (newid()) FOR [Guid]
GO
/****** Object:  Default [DF_TimeEntries_TimeEntryType]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[TimeEntries] ADD  CONSTRAINT [DF_TimeEntries_TimeEntryType]  DEFAULT ((1)) FOR [TimeEntryTypeId]
GO
/****** Object:  Default [DF_TimeEntries_ClientSourceId]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[TimeEntries] ADD  CONSTRAINT [DF_TimeEntries_ClientSourceId]  DEFAULT ((0)) FOR [ClientSourceId]
GO
/****** Object:  Default [DF_Users_Inactive]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Inactive]  DEFAULT ((0)) FOR [Inactive]
GO
/****** Object:  ForeignKey [FK_Customers_Users]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_Customers_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_Customers_Users]
GO
/****** Object:  ForeignKey [FK_Customers_Users1]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_Customers_Users1] FOREIGN KEY([ChangedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_Customers_Users1]
GO
/****** Object:  ForeignKey [FK_InvoiceLines_Invoices]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[InvoiceLines]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceLines_Invoices] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoices] ([ID])
GO
ALTER TABLE [dbo].[InvoiceLines] CHECK CONSTRAINT [FK_InvoiceLines_Invoices]
GO
/****** Object:  ForeignKey [FK_Invoices_Users]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoices_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoices_Users]
GO
/****** Object:  ForeignKey [FK_Projects_Customers]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Customers]
GO
/****** Object:  ForeignKey [FK_Projects_Users]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Users] FOREIGN KEY([ChangedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Users]
GO
/****** Object:  ForeignKey [FK_Projects_Users1]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Users1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Users1]
GO
/****** Object:  ForeignKey [FK_Tags_Customers]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Tags]  WITH CHECK ADD  CONSTRAINT [FK_Tags_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Tags] CHECK CONSTRAINT [FK_Tags_Customers]
GO
/****** Object:  ForeignKey [FK_Tasks_Projects]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Projects] FOREIGN KEY([ChangedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Projects]
GO
/****** Object:  ForeignKey [FK_Tasks_Tags]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Tags] FOREIGN KEY([TagID])
REFERENCES [dbo].[Tags] ([TagID])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Tags]
GO
/****** Object:  ForeignKey [FK_Tasks_Users]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Users]
GO
/****** Object:  ForeignKey [FK_TaskTree_Tasks]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[TaskTree]  WITH CHECK ADD  CONSTRAINT [FK_TaskTree_Tasks] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Tasks] ([TaskID])
GO
ALTER TABLE [dbo].[TaskTree] CHECK CONSTRAINT [FK_TaskTree_Tasks]
GO
/****** Object:  ForeignKey [FK_TaskTree_Tasks1]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[TaskTree]  WITH CHECK ADD  CONSTRAINT [FK_TaskTree_Tasks1] FOREIGN KEY([ChildID])
REFERENCES [dbo].[Tasks] ([TaskID])
GO
ALTER TABLE [dbo].[TaskTree] CHECK CONSTRAINT [FK_TaskTree_Tasks1]
GO
/****** Object:  ForeignKey [FK_TimeEntries_TimeEntryTypes]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[TimeEntries]  WITH CHECK ADD  CONSTRAINT [FK_TimeEntries_TimeEntryTypes] FOREIGN KEY([TimeEntryTypeId])
REFERENCES [dbo].[TimeEntryTypes] ([TimeEntryTypeId])
GO
ALTER TABLE [dbo].[TimeEntries] CHECK CONSTRAINT [FK_TimeEntries_TimeEntryTypes]
GO
/****** Object:  ForeignKey [FK_TimeEntries_Users]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[TimeEntries]  WITH CHECK ADD  CONSTRAINT [FK_TimeEntries_Users] FOREIGN KEY([ChangedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[TimeEntries] CHECK CONSTRAINT [FK_TimeEntries_Users]
GO
/****** Object:  ForeignKey [FK_TimeRegs_Tasks]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[TimeEntries]  WITH CHECK ADD  CONSTRAINT [FK_TimeRegs_Tasks] FOREIGN KEY([TaskID])
REFERENCES [dbo].[Tasks] ([TaskID])
GO
ALTER TABLE [dbo].[TimeEntries] CHECK CONSTRAINT [FK_TimeRegs_Tasks]
GO
/****** Object:  ForeignKey [FK_TimeRegs_Users]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[TimeEntries]  WITH CHECK ADD  CONSTRAINT [FK_TimeRegs_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[TimeEntries] CHECK CONSTRAINT [FK_TimeRegs_Users]
GO
/****** Object:  ForeignKey [FK_CustomersTimeEntryTypes_Customers]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[TimeEntryTypes]  WITH CHECK ADD  CONSTRAINT [FK_CustomersTimeEntryTypes_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[TimeEntryTypes] CHECK CONSTRAINT [FK_CustomersTimeEntryTypes_Customers]
GO
/****** Object:  ForeignKey [FK_UsersCustomers_Customers]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[UsersCustomers]  WITH CHECK ADD  CONSTRAINT [FK_UsersCustomers_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[UsersCustomers] CHECK CONSTRAINT [FK_UsersCustomers_Customers]
GO
/****** Object:  ForeignKey [FK_UsersCustomers_Users]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[UsersCustomers]  WITH CHECK ADD  CONSTRAINT [FK_UsersCustomers_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UsersCustomers] CHECK CONSTRAINT [FK_UsersCustomers_Users]
GO
/****** Object:  ForeignKey [FK_UsersProjects_Projects]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[UsersProjects]  WITH CHECK ADD  CONSTRAINT [FK_UsersProjects_Projects] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Projects] ([ProjectID])
GO
ALTER TABLE [dbo].[UsersProjects] CHECK CONSTRAINT [FK_UsersProjects_Projects]
GO
/****** Object:  ForeignKey [FK_UsersProjects_Users]    Script Date: 02/01/2012 09:20:45 ******/
ALTER TABLE [dbo].[UsersProjects]  WITH CHECK ADD  CONSTRAINT [FK_UsersProjects_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UsersProjects] CHECK CONSTRAINT [FK_UsersProjects_Users]
GO
