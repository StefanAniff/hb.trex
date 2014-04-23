/*
   15. september 200812:04:00
   User: 
   Server: MICKEYMOUSE\SQLEXPRESS
   Database: Trex.Server
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Customers ADD
	StreetAddress nvarchar(400) NULL,
	ZipCode nvarchar(50) NULL,
	City nvarchar(100) NULL,
	Country nvarchar(200) NULL,
	ContactName nvarchar(200) NULL,
	ContactPhone nvarchar(100) NULL,
	CustomerNumber nvarchar(50) NULL
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Customers', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Customers', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Customers', 'Object', 'CONTROL') as Contr_Per 


/*
   16. september 200812:35:52
   User: 
   Server: MICKEYMOUSE\SQLEXPRESS
   Database: Trex.Server
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.TimeEntries ADD
	InvoiceId int NULL
GO
ALTER TABLE dbo.TimeEntries
	DROP COLUMN BilledDate, Billed
GO
COMMIT
select Has_Perms_By_Name(N'dbo.TimeEntries', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TimeEntries', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TimeEntries', 'Object', 'CONTROL') as Contr_Per 


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  Table [dbo].[Invoices]    Script Date: 09/17/2008 12:48:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoices](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CustomerName] [nvarchar](200) COLLATE Danish_Norwegian_CI_AS NULL,
	[StreetAddress] [nvarchar](400) COLLATE Danish_Norwegian_CI_AS NULL,
	[ZipCode] [nvarchar](50) COLLATE Danish_Norwegian_CI_AS NULL,
	[City] [nvarchar](100) COLLATE Danish_Norwegian_CI_AS NULL,
	[Country] [nvarchar](200) COLLATE Danish_Norwegian_CI_AS NULL,
	[InvoiceDeadline] [datetime] NULL,
	[Attention] [nvarchar](300) COLLATE Danish_Norwegian_CI_AS NULL,
	[TotalExclVAT] [float] NOT NULL,
	[VAT] [float] NOT NULL,
	[TotalInclVAT] [float] NOT NULL,
	[FooterText] [nvarchar](1000) COLLATE Danish_Norwegian_CI_AS NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Closed] [bit] NOT NULL,
 CONSTRAINT [PK_Invoices] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoices_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoices_Customers]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoices_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoices_Users]

GO

/****** Object:  Table [dbo].[InvoiceLines]    Script Date: 09/17/2008 12:49:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceLines](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](1000) COLLATE Danish_Norwegian_CI_AS NOT NULL,
	[PricePrUnit] [float] NOT NULL,
	[InvoiceID] [int] NOT NULL,
	[Units] [float] NOT NULL,
	[Unit] [nvarchar](50) COLLATE Danish_Norwegian_CI_AS NULL,
	[UnitType] [int] NOT NULL CONSTRAINT [DF_InvoiceLines_UnitType]  DEFAULT ((1)),
	[SortIndex] [int] NOT NULL CONSTRAINT [DF_InvoiceLines_SortIndex]  DEFAULT ((0)),
 CONSTRAINT [PK_InvoiceLines] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[InvoiceLines]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceLines_Invoices] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[Invoices] ([ID])
GO
ALTER TABLE [dbo].[InvoiceLines] CHECK CONSTRAINT [FK_InvoiceLines_Invoices]

GO

/****** Object:  UserDefinedFunction [dbo].[ConvertToSmallDate]    Script Date: 09/17/2008 12:57:33 ******/
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


/****** Object:  UserDefinedFunction [dbo].[RoundUpToNextQuarter]    Script Date: 09/17/2008 12:58:43 ******/
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



CREATE PROCEDURE [dbo].[spAggregatedTimeEntriesPrTaskPrDay] 
@customerId INT, @startdate DATETIME, @enddate DATETIME
AS

	--First, sum all timeentries,grouped by date, and round time to nearest quarter
	SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate, c.CustomerName AS Customer, p.ProjectName AS Project, t.TaskName AS Task, 
						  dbo.RoundUpToNextQuarter(SUM(te.TimeSpent)) AS TimeSpent, t.TimeLeft AS Remaining, t.TimeEstimated AS Estimate 
	INTO #temp
	FROM         dbo.TimeEntries AS te 
				INNER JOIN dbo.Tasks AS t ON t.TaskID = te.TaskID 
				INNER JOIN dbo.Projects AS p ON p.ProjectID = t.ProjectID 
				INNER JOIN dbo.Customers AS c ON c.CustomerID = p.CustomerID
	WHERE te.StartTime >= @startdate AND te.endtime <= @enddate
	AND c.CustomerID = @customerId
	GROUP BY dbo.ConvertToSmallDate(te.StartTime), c.CustomerName, p.ProjectName, t.TaskName, t.TimeLeft, t.TimeEstimated

	--Then, sum the calculated timeentries, grouped by task
	SELECT     Customer, Project, Task, SUM(TimeSpent) AS TimeSpent, Remaining, Estimate
	FROM       #temp
	GROUP BY Customer, Project, Task, Remaining, Estimate

	DROP TABLE #temp	

GO


/****** Object:  StoredProcedure [dbo].[spBookUnbilledInvoiceLines]    Script Date: 09/17/2008 12:46:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spBookUnbilledInvoiceLines] @invoiceId INT, @customerId INT, @startdate DATETIME, @enddate DATETIME
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

)
GO


/****** Object:  StoredProcedure [dbo].[spGetGeneratedInvoiceLines]    Script Date: 09/17/2008 12:47:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetGeneratedInvoiceLines] @InvoiceId INT

AS

--First, sum all timeentries,grouped by date, and round time to nearest quarter
SELECT   dbo.ConvertToSmallDate(te.StartTime) AS StartDate,  
						  dbo.RoundUpToNextQuarter(SUM(te.TimeSpent)) AS TimeSpent, te.price ,t.taskid
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


INSERT INTO Version (Version ,Date,Creator,Description) VALUES('0.9.0.0',GetDate(),'tga','Invoice modul')

GO