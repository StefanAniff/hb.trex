/****** Object:  Table [dbo].[Users]    Script Date: 11/26/2008 13:26:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Email] [nvarchar](100) NULL,
	[Price] [float] NULL,
	[Inactive] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Version]    Script Date: 11/26/2008 13:26:03 ******/
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
/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 11/26/2008 13:26:06 ******/
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
/****** Object:  UserDefinedFunction [dbo].[RoundUpToNextQuarter]    Script Date: 11/26/2008 13:26:06 ******/
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
	
DECLARE @Hours float
DECLARE @minutes float
DECLARE @returnFloat float

SELECT @Hours = FLOOR(@float)
SELECT @minutes = (@float - @hours) * 60

IF(@minutes > 45)
	SELECT @Hours = @Hours +1

SELECT @minutes = CASE 
	WHEN @minutes > 0 AND @minutes <= 15 THEN 15
	WHEN @minutes > 15 AND @minutes <= 30 THEN 30
	WHEN @minutes > 30 AND  @minutes <= 45 THEN 45
	WHEN @minutes > 45 THEN 0
	ELSE 0
END

SELECT @minutes = @Minutes /60
SELECT @returnFloat =@hours + @minutes
RETURN @ReturnFloat
	 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ConvertToSmallDate]    Script Date: 11/26/2008 13:26:06 ******/
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
/****** Object:  Table [dbo].[Customers]    Script Date: 11/26/2008 13:26:03 ******/
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
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoices]    Script Date: 11/26/2008 13:26:03 ******/
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
 CONSTRAINT [PK_Invoices] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersCustomers]    Script Date: 11/26/2008 13:26:03 ******/
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
/****** Object:  Table [dbo].[Tags]    Script Date: 11/26/2008 13:26:03 ******/
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
/****** Object:  Table [dbo].[InvoiceLines]    Script Date: 11/26/2008 13:26:03 ******/
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
 CONSTRAINT [PK_InvoiceLines] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 11/26/2008 13:26:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[ProjectID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] ROWGUIDCOL  NULL,
	[CustomerID] [int] NOT NULL,
	[ProjectName] [nvarchar](50) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Inactive] [bit] NOT NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 11/26/2008 13:26:03 ******/
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
	[TaskName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[TimeEstimated] [float] NOT NULL,
	[TimeLeft] [int] NOT NULL,
	[Closed] [bit] NOT NULL,
	[WorstCaseEstimate] [float] NOT NULL,
	[BestCaseEstimate] [float] NOT NULL,
	[TagID] [int] NULL,
	[RealisticEstimate] [float] NOT NULL,
	[Inactive] [bit] NOT NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersProjects]    Script Date: 11/26/2008 13:26:03 ******/
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
/****** Object:  Table [dbo].[TimeEntries]    Script Date: 11/26/2008 13:26:03 ******/
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
 CONSTRAINT [PK_Trex.Servers] PRIMARY KEY CLUSTERED 
(
	[TimeEntryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[spGetGeneratedInvoiceLines]    Script Date: 11/26/2008 13:25:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetGeneratedInvoiceLines] @InvoiceId INT

AS

--First, sum all timeentries,grouped by date, and round time to nearest quarter
SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate,  
						  dbo.RoundUpToNextQuarter(SUM(te.TimeSpent-te.PauseTime)) AS TimeSpent, te.price ,t.taskid
INTO #temp
	FROM         dbo.TimeEntries AS te 
				INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
				INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
				INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
	WHERE  te.InvoiceID = @InvoiceId
	GROUP BY dbo.ConvertToSmallDate(te.StartTime),  te.price, t.taskid
	
--	--Then, sum the calculated timeentries, grouped by price
	SELECT     price, SUM(TimeSpent) AS TimeSpent
	FROM       #temp
	GROUP BY price

	DROP TABLE #temp
GO
/****** Object:  StoredProcedure [dbo].[spBookUnbilledInvoiceLines]    Script Date: 11/26/2008 13:25:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spBookUnbilledInvoiceLines] @invoiceId INT, @customerId INT, @startdate DATETIME, @enddate DATETIME, @excludeList NVARCHAR(1000)=NULL
AS

UPDATE timeentries
SET InvoiceID = @invoiceID
WHERE timeentryid IN
(
	SELECT te.timeentryid

		FROM         dbo.TimeEntries AS te 
					INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
					INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
					INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
		WHERE te.StartTime >= @startDate AND te.endtime <= @endDate
		AND te.InvoiceID IS NULL
		AND te.Billable = 1
		AND c.CustomerID = @customerId
		AND te.TimeEntryID NOT IN (SELECT SeparatedValue FROM dbo.Split(',',@excludeList))
)
GO
/****** Object:  StoredProcedure [dbo].[spAggregatedTimeEntriesPrTaskPrDayPrInvoice]    Script Date: 11/26/2008 13:25:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAggregatedTimeEntriesPrTaskPrDayPrInvoice] 
@InvoiceId INT
AS

	--First, sum all timeentries,grouped by date, and round time to nearest quarter
	SELECT   dbo.ConvertToSmallDate(te.StartTime) AS TaskDate, c.CustomerName AS Customer,c.customerId ,p.ProjectName AS Project, t.TaskName AS Task, 
						  dbo.RoundUpToNextQuarter(SUM(te.TimeSpent-te.PauseTime)) AS TimeSpent,
					i.ID AS InvoiceNumber, i.InvoiceDate, i.StreetAddress, i.ZipCode, i.City, i.Country, i.Attention, i.StartDate, i.EndDate
	INTO #temp
	FROM         dbo.TimeEntries AS te 
				INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
				INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
				INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
				INNER JOIN dbo.Invoices AS i ON te.InvoiceId = i.ID
	WHERE i.ID = @InvoiceId
	
	GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName,c.customerId ,i.ID, i.InvoiceDate, i.StreetAddress, i.ZipCode, i.City, i.Country, i.Attention, i.StartDate, i.EndDate

	--Then, sum the calculated timeentries, grouped by task
	SELECT     Customer AS CustomerName, Project, Task, SUM(TimeSpent) AS TimeSpent, InvoiceNumber, InvoiceDate,customerId AS CustomerNumber, StreetAddress, ZipCode, City,Country,Attention AS ContactName,StartDate, EndDate,DATEADD(dd,14,InvoiceDate) AS DeadLine
	FROM       #temp
	GROUP BY Customer, Project, Task,InvoiceNumber, InvoiceDate,CustomerId, StreetAddress, ZipCode, City,Country,Attention,StartDate, EndDate

	DROP TABLE #temp
GO
/****** Object:  StoredProcedure [dbo].[spAggregatedTimeEntriesPrTaskPrDay]    Script Date: 11/26/2008 13:25:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAggregatedTimeEntriesPrTaskPrDay] 
@customerId INT, @startdate DATETIME, @enddate DATETIME
AS

	--First, sum all timeentries,grouped by date, and round time to nearest quarter
	SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, p.ProjectName AS Project, t.TaskName AS Task, 
						  dbo.RoundUpToNextQuarter(SUM(te.TimeSpent-te.PauseTime)) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate 
	INTO #temp
	FROM         dbo.TimeEntries AS te 
				INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
				INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
				INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
	WHERE te.StartTime >= @startdate AND te.endtime <= @enddate
	AND c.CustomerID = @customerId
	AND te.Billable = 1
	GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName, t.TimeLeft, t.TimeEstimated

	--Then, sum the calculated timeentries, grouped by task
	SELECT     Customer, Project, Task, SUM(TimeSpent) AS TimeSpent, Remaining, Estimate
	FROM       #temp
	GROUP BY Customer, Project, Task, Remaining, Estimate

	DROP TABLE #temp
GO
/****** Object:  Default [DF_Customers_Guid]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_Guid]  DEFAULT (newid()) FOR [Guid]
GO
/****** Object:  Default [DF_Customers_CreateDate]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Customers_Inactive]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_Inactive]  DEFAULT ((0)) FOR [Inactive]
GO
/****** Object:  Default [DF_InvoiceLines_UnitType]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[InvoiceLines] ADD  CONSTRAINT [DF_InvoiceLines_UnitType]  DEFAULT ((1)) FOR [UnitType]
GO
/****** Object:  Default [DF_InvoiceLines_SortIndex]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[InvoiceLines] ADD  CONSTRAINT [DF_InvoiceLines_SortIndex]  DEFAULT ((0)) FOR [SortIndex]
GO
/****** Object:  Default [DF_Invoices_InvoiceDate]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Invoices] ADD  CONSTRAINT [DF_Invoices_InvoiceDate]  DEFAULT (getdate()) FOR [InvoiceDate]
GO
/****** Object:  Default [DF_Projects_Guid]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_Guid]  DEFAULT (newid()) FOR [Guid]
GO
/****** Object:  Default [DF_Projects_CreateDate]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Projects_Inactive]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_Inactive]  DEFAULT ((0)) FOR [Inactive]
GO
/****** Object:  Default [DF_Tasks_Guid]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_Guid]  DEFAULT (newid()) FOR [Guid]
GO
/****** Object:  Default [DF_Tasks_ModifyDate]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_ModifyDate]  DEFAULT (getdate()) FOR [ModifyDate]
GO
/****** Object:  Default [DF_Tasks_CreateDate]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_Tasks_TimeEstimated]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_TimeEstimated]  DEFAULT ((0)) FOR [TimeEstimated]
GO
/****** Object:  Default [DF_Tasks_Closed]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_Closed]  DEFAULT ((0)) FOR [Closed]
GO
/****** Object:  Default [DF_Tasks_WorstCaseEstimate]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_WorstCaseEstimate]  DEFAULT ((0)) FOR [WorstCaseEstimate]
GO
/****** Object:  Default [DF_Tasks_BestCaseEstimate]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_BestCaseEstimate]  DEFAULT ((0)) FOR [BestCaseEstimate]
GO
/****** Object:  Default [DF_Tasks_RealisticEstimate]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_RealisticEstimate]  DEFAULT ((0)) FOR [RealisticEstimate]
GO
/****** Object:  Default [DF_Tasks_Inactive]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_Inactive]  DEFAULT ((0)) FOR [Inactive]
GO
/****** Object:  Default [DF_TimeEntries_TimeSpent]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[TimeEntries] ADD  CONSTRAINT [DF_TimeEntries_TimeSpent]  DEFAULT ((0)) FOR [TimeSpent]
GO
/****** Object:  Default [DF_Users_Inactive]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Inactive]  DEFAULT ((0)) FOR [Inactive]
GO
/****** Object:  ForeignKey [FK_Customers_Users]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_Customers_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_Customers_Users]
GO
/****** Object:  ForeignKey [FK_InvoiceLines_Invoices]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[InvoiceLines]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceLines_Invoices] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoices] ([ID])
GO
ALTER TABLE [dbo].[InvoiceLines] CHECK CONSTRAINT [FK_InvoiceLines_Invoices]
GO
/****** Object:  ForeignKey [FK_Invoices_Users]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoices_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoices_Users]
GO
/****** Object:  ForeignKey [FK_Projects_Customers]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Customers]
GO
/****** Object:  ForeignKey [FK_Projects_Users1]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Users1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Users1]
GO
/****** Object:  ForeignKey [FK_Tags_Customers]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Tags]  WITH CHECK ADD  CONSTRAINT [FK_Tags_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Tags] CHECK CONSTRAINT [FK_Tags_Customers]
GO
/****** Object:  ForeignKey [FK_Tasks_Projects]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Projects] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Projects] ([ProjectID])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Projects]
GO
/****** Object:  ForeignKey [FK_Tasks_Tags]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Tags] FOREIGN KEY([TagID])
REFERENCES [dbo].[Tags] ([TagID])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Tags]
GO
/****** Object:  ForeignKey [FK_Tasks_Users]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Users]
GO
/****** Object:  ForeignKey [FK_Trex.Servers_Tasks]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[TimeEntries]  WITH CHECK ADD  CONSTRAINT [FK_Trex.Servers_Tasks] FOREIGN KEY([TaskID])
REFERENCES [dbo].[Tasks] ([TaskID])
GO
ALTER TABLE [dbo].[TimeEntries] CHECK CONSTRAINT [FK_Trex.Servers_Tasks]
GO
/****** Object:  ForeignKey [FK_Trex.Servers_Users]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[TimeEntries]  WITH CHECK ADD  CONSTRAINT [FK_Trex.Servers_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[TimeEntries] CHECK CONSTRAINT [FK_Trex.Servers_Users]
GO
/****** Object:  ForeignKey [FK_UsersCustomers_Customers]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[UsersCustomers]  WITH CHECK ADD  CONSTRAINT [FK_UsersCustomers_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[UsersCustomers] CHECK CONSTRAINT [FK_UsersCustomers_Customers]
GO
/****** Object:  ForeignKey [FK_UsersCustomers_Users]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[UsersCustomers]  WITH CHECK ADD  CONSTRAINT [FK_UsersCustomers_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UsersCustomers] CHECK CONSTRAINT [FK_UsersCustomers_Users]
GO
/****** Object:  ForeignKey [FK_UsersProjects_Projects]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[UsersProjects]  WITH CHECK ADD  CONSTRAINT [FK_UsersProjects_Projects] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Projects] ([ProjectID])
GO
ALTER TABLE [dbo].[UsersProjects] CHECK CONSTRAINT [FK_UsersProjects_Projects]
GO
/****** Object:  ForeignKey [FK_UsersProjects_Users]    Script Date: 11/26/2008 13:26:03 ******/
ALTER TABLE [dbo].[UsersProjects]  WITH CHECK ADD  CONSTRAINT [FK_UsersProjects_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UsersProjects] CHECK CONSTRAINT [FK_UsersProjects_Users]
GO


INSERT INTO Version (Version ,Date,Creator,Description) VALUES('0.9.5.0',GetDate(),'tga','Created')
	GO
