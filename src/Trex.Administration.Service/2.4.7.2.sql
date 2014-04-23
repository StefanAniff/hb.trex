SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FindBillableTimeEntries]
(	
	@customerInvoiceGroupId INT, 
	@startdate DATETIME, 
	@enddate DATETIME
)
RETURNS TABLE 
AS
RETURN 
(
   SELECT   
		dbo.RoundUpToNextQuarter(te.BillableTime) AS TimeSpent, 
		dbo.ConvertToSmallDate(te.StartTime) AS StartDate, 
		dbo.ConvertToSmallDate(te.EndTime) AS EndDate,
		p.ProjectName AS Project, 
		p.ProjectID AS [Project ID],
		t.TaskName AS Task, 
		t.TaskID AS [Task ID],
		te.Price AS [Price pr Hour],
		te.InvoiceId AS InvoiceID,
		p.CustomerInvoiceGroupID AS [CIG ID],
		p.CustomerID AS [Customer ID],
		cig.Label AS GroupName,
		te.TimeEntryID AS [TimeEntry ID]
                    
   FROM 
		dbo.TimeEntries AS te 
        INNER JOIN dbo.Tasks AS t 
			ON t.TaskID = te.TaskID 
        INNER JOIN dbo.Projects AS p 
			ON p.ProjectID = t.ProjectID 
        INNER JOIN dbo.CustomerInvoiceGroup AS cig 
			ON cig.CustomerInvoiceGroupID = p.CustomerInvoiceGroupID 
        INNER JOIN dbo.Users AS u 
			ON u.UserID = te.UserID 
   WHERE 
		te.StartTime >= @startdate 
		AND te.endtime <= @enddate
		AND cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
		AND te.Billable = 1
)
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FindAllTimeEntries]
(	
	@customerInvoiceGroupId INT, 
	@startdate DATETIME, 
	@enddate DATETIME
)
RETURNS TABLE 
AS
RETURN 
(
   SELECT   
		dbo.RoundUpToNextQuarter(te.BillableTime) AS TimeSpent, 
		dbo.ConvertToSmallDate(te.StartTime) AS StartDate, 
		dbo.ConvertToSmallDate(te.EndTime) AS EndDate,
		p.ProjectName AS Project, 
		p.ProjectID AS [Project ID],
		t.TaskName AS Task, 
		t.TaskID AS [Task ID],
		te.Price AS [Price pr Hour],
		te.InvoiceId AS InvoiceID,
		p.CustomerInvoiceGroupID AS [CIG ID],
		p.CustomerID AS [Customer ID],
		cig.Label AS GroupName,
		te.TimeEntryID AS [TimeEntry ID]
                    
   FROM 
		dbo.TimeEntries AS te 
        INNER JOIN dbo.Tasks AS t 
			ON t.TaskID = te.TaskID 
        INNER JOIN dbo.Projects AS p 
			ON p.ProjectID = t.ProjectID 
        INNER JOIN dbo.CustomerInvoiceGroup AS cig 
			ON cig.CustomerInvoiceGroupID = p.CustomerInvoiceGroupID 
        INNER JOIN dbo.Users AS u 
			ON u.UserID = te.UserID 
   WHERE 
		te.StartTime >= @startdate 
		AND te.endtime <= @enddate
		AND cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
)
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateTimeEntriesInvoiceId]
	@customerInvoiceGroupId INT, 
	@startdate DATETIME, 
	@enddate DATETIME,
	@invoiceId int
AS
BEGIN

	UPDATE TimeEntries
	SET InvoiceId = @invoiceId
	WHERE TimeEntryID = ANY(SELECT [TimeEntry ID] 
						FROM [dbo].FindAllTimeEntries(@customerInvoiceGroupId, @startdate, @enddate))
END
GO


  ALTER TABLE Tasks
  ADD FOREIGN KEY (ProjectID)
  REFERENCES Projects(ProjectID)
  
  ALTER TABLE TimeEntries
  ADD FOREIGN KEY (InvoiceId)
  REFERENCES Invoices(ID);
  GO
  
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
ALTER TABLE dbo.Projects
	DROP CONSTRAINT FK_Projects_Users1
GO
ALTER TABLE dbo.Tasks
	DROP CONSTRAINT FK_Tasks_Projects
GO
ALTER TABLE dbo.Customers
	DROP CONSTRAINT FK_Customers_Users1
GO
ALTER TABLE dbo.Users SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


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
ALTER TABLE dbo.Invoices
	DROP COLUMN CustomerName, StreetAddress, ZipCode, City, Country, Attention
GO
ALTER TABLE dbo.Invoices SET (LOCK_ESCALATION = TABLE)
GO
COMMIT




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGenerateNewInvoiceDraft]
@customerInvoiceGroupId int,
@createdBy int,
@VAT float,
@startDate DATETIME,
@endDate DATETIME
AS
BEGIN

	DECLARE @Duedate DATETIME;
	DECLARE @invoiceDate DATETIME;
	DECLARE @today DATETIME;
	SET @today = GETDATE();

	DECLARE @LastDayOfLastMonth DATETIME;
	--DECLARE @LastDayOfCurrentMonth DATETIME;
	--DECLARE @FirstDayOfCurrentMonth DATETIME;
	DECLARE @FirstDayOfNextMonth DATETIME;

	SET @LastDayOfLastMonth = DATEADD(dd,-(DAY(@today)),@today);
	SET @invoiceDate = @LastDayOfLastMonth;
	--SET @FirstDayOfCurrentMonth = DATEADD(dd,-(DAY(@today)-1),@today);
	--SET @LastDayOfCurrentMonth = DATEADD(dd,-(DAY(DATEADD(mm,1,@today))),DATEADD(mm,1,@today));
	SET @FirstDayOfNextMonth = DATEADD(dd,-(DAY(DATEADD(mm,1,@invoiceDate))-1),DATEADD(mm,1,@invoiceDate));

	--SELECT
	--	@LastDayOfCurrentMonth AS [Last of current],
	--	@LastDayOfLastMonth AS [Last of last],
	--	@FirstDayOfCurrentMonth AS [First of current],
	--	@FirstDayOfNextMonth AS [First of next]

	
	SET @Duedate = (
		SELECT 
			CASE
				WHEN c.PaymentTermsIncludeCurrentMonth = 0 --Month not included
					THEN DATEADD(D, c.PaymentTermsNumberOfDays, @invoiceDate) -- Add paymentTermsNumbersOfDays to InvoiceDate
					ELSE DATEADD(D, c.PaymentTermsNumberOfDays, @FirstDayOfNextMonth) --Add paymentTermsNumbersOfDays from the next month's 1th to InvoiceDate
			END
		FROM Customers c
			INNER JOIN CustomerInvoiceGroup cig ON
				cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
		WHERE cig.CustomerID = c.CustomerID);

			
	--Insert data for a draft
	INSERT INTO Invoices (
		CreateDate, 
		InvoiceDate, 
		CreatedBy, 
		VAT, 
		StartDate, 
		EndDate, 
		Closed, 
		DueDate, 
		CustomerInvoiceGroupId)
	VALUES 
		(
		GETDATE(),
		@invoiceDate, --InvoiceDate - what is it?
		@createdBy,
		@VAT,
		@startDate,
		@endDate,
		0, --Not closed
		@DueDate, 
		@customerInvoiceGroupId
		);
		
	SELECT TOP 1 *
	FROM Invoices i	
	ORDER BY i.ID DESC
	
END
GO


ALTER TABLE Invoices
DROP COLUMN Address2
GO

-- Generate InvoiceLines, erasing earlier autogenerated if any and recalculating the new ones
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGenerateInvoiceLines]
@invoiceId int,
@startDate DATETIME,
@endDate DATETIME,
@cigID int
AS
BEGIN
	
	--Find different prices
	SELECT 
		ID_NUM = IDENTITY(int, 1, 1),
		fte.[Price pr Hour],
		SUM(fte.TimeSpent) AS TimeSpent
	INTO #NewTable
	FROM dbo.FindAllTimeEntries(@cigID, @startDate, @endDate) fte
	GROUP BY fte.[Price pr Hour];

	--Remove old lines
	DELETE FROM InvoiceLines
	WHERE 
		InvoiceID = @invoiceId
		AND UnitType = '0'; --Auto generated lines
		
		
	DECLARE @MaxID int;
	DECLARE @Counter int;
	
	 
	SET @Counter = 1;
	
	SELECT @MaxID = COUNT(t.ID_NUM) 
	FROM #NewTable t;
	
	WHILE @Counter <= @MaxID 
	BEGIN
	
		DECLARE @f1 float;
		DECLARE @f2 float;
		DECLARE @f3 int;
		DECLARE @f4 nvarchar(50);
		DECLARE @f5 int;
		DECLARE @f6 bit;
		DECLARE @f7 float;
		DECLARE @f8 nvarchar(1000);
		
		SET @f1 = (SELECT t.[Price pr Hour] FROM #NewTable t WHERE ID_NUM = @Counter);
		SET @f2 = (SELECT t.TimeSpent FROM #NewTable t WHERE ID_NUM = @Counter);
		SET @f3 = @invoiceId;
		SET @f4 = 'timer';
		SET @f5 = 0; --Auto generated
		SET @f6 = 1; --Isexpense
		SET @f7 = (SELECT TOP 1 i.VAT FROM Invoices i WHERE i.ID = @invoiceId); --VAT
		SET @f8 = '';
		
		
		--Insert into table
		INSERT INTO InvoiceLines 
			(PricePrUnit, 
			Units, 
			InvoiceID, 
			Unit, 
			UnitType,
			IsExpense, 
			VatPercentage,
			[Text]) 
		VALUES (@f1, @f2, @f3, @f4, @f5, @f6, @f7, @f8);
				     
		-- update counter
		set @Counter = @Counter + 1;
	END;
	
	
	DROP TABLE #NewTable;
	
END
GO





SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spShowInvoiceLines]
(
	@invoiceId int
)
AS
BEGIN
	
	SELECT
		il.PricePrUnit,
		il.Units,
		il.Unit,
		il.IsExpense,
		il.[Text],
		il.VatPercentage		
	FROM  InvoiceLines il
	WHERE il.InvoiceID = @invoiceId
	ORDER BY il.PricePrUnit	

END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spFindTimeEntiresForInvoice] 
@customerInvoiceGroupId INT, 
@startdate DATETIME, 
@enddate DATETIME
AS
BEGIN
   
   SELECT   
		dbo.RoundUpToNextQuarter(te.BillableTime) AS TimeSpent, 
		dbo.ConvertToSmallDate(te.StartTime) AS StartDate, 
		dbo.ConvertToSmallDate(te.EndTime) AS EndDate,
		p.ProjectName AS Project, 
		p.ProjectID AS [Project ID],
		t.TaskName AS Task, 
		t.TaskID AS [Task ID],
		te.Price AS [Price pr Hour],
		te.InvoiceId AS InvoiceID,
		p.CustomerInvoiceGroupID AS [CIG ID],
		p.CustomerID AS [Customer ID],
		cig.Label AS GroupName,
		te.TimeEntryID AS [TimeEntry ID]
                    
   FROM 
		dbo.TimeEntries AS te 
        INNER JOIN dbo.Tasks AS t 
			ON t.TaskID = te.TaskID 
        INNER JOIN dbo.Projects AS p 
			ON p.ProjectID = t.ProjectID 
        INNER JOIN dbo.CustomerInvoiceGroup AS cig 
			ON cig.CustomerInvoiceGroupID = p.CustomerInvoiceGroupID 
        INNER JOIN dbo.Users AS u 
			ON u.UserID = te.UserID 
   WHERE 
		te.StartTime >= @startdate 
		AND te.endtime <= @enddate
		AND cig.CustomerInvoiceGroupID = @customerInvoiceGroupId
		AND te.Billable = 1
		AND te.InvoiceId is NULL
  
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGenerateInvoiceLines]
@invoiceId int,
@startDate DATETIME,
@endDate DATETIME,
@cigID int
AS
BEGIN
	
	--Find different prices
	SELECT 
		ID_NUM = IDENTITY(int, 1, 1),
		fte.[Price pr Hour],
		SUM(fte.TimeSpent) AS TimeSpent
	INTO #NewTable
	FROM dbo.FindAllTimeEntries(@cigID, @startDate, @endDate) fte
	GROUP BY fte.[Price pr Hour];

	--Remove old lines
	DELETE FROM InvoiceLines
	WHERE 
		InvoiceID = @invoiceId
		AND UnitType = '0'; --Auto generated lines
		
		
	DECLARE @MaxID int;
	DECLARE @Counter int;
	
	 
	SET @Counter = 1;
	
	SELECT @MaxID = COUNT(t.ID_NUM) 
	FROM #NewTable t;
	
	WHILE @Counter <= @MaxID 
	BEGIN
	
		DECLARE @f1 float;
		DECLARE @f2 float;
		DECLARE @f3 int;
		DECLARE @f4 nvarchar(50);
		DECLARE @f5 int;
		DECLARE @f6 bit;
		DECLARE @f7 float;
		DECLARE @f8 nvarchar(1000);
		
		SET @f1 = (SELECT t.[Price pr Hour] FROM #NewTable t WHERE ID_NUM = @Counter);
		SET @f2 = (SELECT t.TimeSpent FROM #NewTable t WHERE ID_NUM = @Counter);
		SET @f3 = @invoiceId;
		SET @f4 = 'timer';
		SET @f5 = 0; --Auto generated
		SET @f6 = 0; --Isexpense (udlæg)
		SET @f7 = (SELECT TOP 1 i.VAT FROM Invoices i WHERE i.ID = @invoiceId); --VAT
		SET @f8 = '';
		
		
		--Insert into table
		INSERT INTO InvoiceLines 
			(PricePrUnit, 
			Units, 
			InvoiceID, 
			Unit, 
			UnitType,
			IsExpense, 
			VatPercentage,
			[Text]) 
		VALUES (@f1, @f2, @f3, @f4, @f5, @f6, @f7, @f8);
				     
		-- update counter
		set @Counter = @Counter + 1;
	END;
	
	
	DROP TABLE #NewTable;
	
END
GO



CREATE PROCEDURE [dbo].[spGetSpecificationData_Project]
	@invoiceId int
AS
BEGIN
	
	SELECT 
		p.ProjectID,
		p.ProjectName, 
		sum(dbo.RoundUpToNextQuarter(te.BillableTime)) AS [TimeUsed]
	FROM TimeEntries te 
		join Tasks t on te.TaskID = t.TaskID
		join Projects p on p.ProjectID = t.ProjectID
	WHERE te.InvoiceId = @invoiceId
	GROUP BY 
		p.ProjectID,
		p.ProjectName
	
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetSpecificationData_Task]
	@invoiceId int
AS
BEGIN
	
	SELECT 
		p.ProjectID,
		t.taskname, 
		sum(dbo.RoundUpToNextQuarter(te.BillableTime)) AS [TimeUsed]
	FROM TimeEntries te 
		join Tasks t on te.taskid = t.TaskID
		join Projects p on p.ProjectID= t.ProjectID
	WHERE te.InvoiceId = @invoiceId
	GROUP BY 
		p.ProjectID,
		t.taskname
	
	
END
GO


ALTER TABLE InvoiceTemplates
ADD StandardInvoicePrint BIT NOT NULL DEFAULT 0

ALTER TABLE InvoiceTemplates
ADD StandardInvoiceMail BIT NOT NULL DEFAULT 0

ALTER TABLE InvoiceTemplates
ADD StandardSpecification BIT NOT NULL DEFAULT 0
GO

ALTER TABLE CustomerInvoiceGroup
ADD InvoiceIdPrint int

ALTER TABLE CustomerInvoiceGroup
ADD InvoiceIdMail int

ALTER TABLE CustomerInvoiceGroup
ADD SpecificationIdMail int
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FindVAT]
(
	@invoiceId int
)
RETURNS float
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result float;

	SELECT @result =
		CASE
			WHEN (SUM(il.PricePrUnit * il.Units)) IS NULL
				THEN 0
				ELSE (SUM(il.PricePrUnit * il.Units))
		END 
	FROM InvoiceLines il
	WHERE il.InvoiceID = @invoiceId

	RETURN @result;

END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spGetInvoices]
	@OrderList varchar(1000)
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF

	CREATE TABLE #TempList
	(
		CustomerId int
	)

	DECLARE @OrderID varchar(10), @Pos int

	SET @OrderList = LTRIM(RTRIM(@OrderList))+ ','
	SET @Pos = CHARINDEX(',', @OrderList, 1)

	IF REPLACE(@OrderList, ',', '') <> ''
	BEGIN
		WHILE @Pos > 0
		BEGIN
			SET @OrderID = LTRIM(RTRIM(LEFT(@OrderList, @Pos - 1)))
			IF @OrderID <> ''
			BEGIN
				INSERT INTO #TempList (CustomerId) VALUES (CAST(@OrderID AS int)) --Use Appropriate conversion
			END
			SET @OrderList = RIGHT(@OrderList, LEN(@OrderList) - @Pos)
			SET @Pos = CHARINDEX(',', @OrderList, 1)

		END
	END	

	SELECT 
		i.ID,
		i.InvoiceID,
		c.CustomerName,
		i.InvoiceDate AS [InvoiceDate],
		DATEDIFF(day,i.StartDate,i.EndDate) AS [InvoicePeriode],
		i.DueDate AS [DueDate],
		cig.Label,
		dbo.FindVAT(i.ID) AS [ExclVAT],
		i.Regarding,
		i.Closed,
		i.CustomerInvoiceGroupId,
		i.StartDate,
		i.EndDate
	FROM InvoiceLines il, #TempList t
		INNER JOIN Customers c
			ON c.CustomerID = t.CustomerId		
		INNER JOIN CustomerInvoiceGroup cig
			ON c.CustomerID = cig.CustomerID
		INNER JOIN Invoices i
			ON i.CustomerInvoiceGroupId = cig.CustomerInvoiceGroupID
	GROUP BY
		c.CustomerName,
		i.InvoiceID,
		i.ID,
		InvoiceDate,	
		cig.Label,
		i.Regarding,
		i.Closed,
		DATEDIFF(day,i.StartDate,i.EndDate),
		DueDate,
		i.CustomerInvoiceGroupId,
		i.StartDate,
		i.EndDate
END

GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetCustomersInvoiceView]
	@endDate DATETIME
AS
BEGIN
	SET NOCOUNT ON
	SET FMTONLY OFF
	
	select 
		c1.CustomerID,
		c1.CustomerName,  
		FirstDateNotInvoiced =(

			select top 1 te.StartTime from timeentries te
			inner join Tasks t on t.taskid = te.TaskID
			inner join Projects p on p.ProjectID = t.ProjectID
			inner join Customers c on c.CustomerID = p.CustomerID
			where 
				te.Billable = 1 
				and te.InvoiceId is null
				and c.CustomerID = c1.CustomerID
			Order by te.StartTime asc)

	,DistinctPrices = (
	select count(distinct te.Price) from TimeEntries te 
	inner join Tasks t on t.taskid = te.TaskID
	inner join Projects p on p.ProjectID = t.ProjectID
	inner join Customers c on c.CustomerID = p.CustomerID
	where 
		te.Billable = 1 
		and te.StartTime < @endDate
		and c.CustomerID =c1.CustomerID
	group by c.CustomerID

	)

	,InventoryValue = (
	select sum( te.Price * te.BillableTime) from TimeEntries te 
	inner join Tasks t on t.taskid = te.TaskID
	inner join Projects p on p.ProjectID = t.ProjectID
	inner join Customers c on c.CustomerID = p.CustomerID
	where 
		te.Billable = 1 
		and te.StartTime < @endDate
		and c.CustomerID =c1.CustomerID 
		and te.InvoiceId is null
	group by c.CustomerID
	)
	,Drafts = (
	CASE
	WHEN
	(SELECT SUM(
		CASE
		WHEN i.InvoiceID is null
			THEN 1
			ELSE 0
		END)
	FROM Invoices i
		INNER JOIN CustomerInvoiceGroup cig
			ON cig.CustomerInvoiceGroupID = i.CustomerInvoiceGroupId
	WHERE c1.CustomerID = cig.CustomerID
	) is null
		THEN 0
		ELSE (SELECT SUM(
				CASE
				WHEN i.InvoiceID is null
					THEN 1
					ELSE 0
				END)
			FROM Invoices i
				INNER JOIN CustomerInvoiceGroup cig
					ON cig.CustomerInvoiceGroupID = i.CustomerInvoiceGroupId
			WHERE c1.CustomerID = cig.CustomerID
			)
				
	END)

from Customers c1
ORDER BY c1.CustomerName

END




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[FindVAT]
(
	@invoiceId int
)
RETURNS float
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result float;

	SELECT @result =
		CASE
			WHEN (SUM(il.PricePrUnit * il.Units)) IS NULL
				THEN 0
				ELSE (SUM(il.PricePrUnit * il.Units))
		END 
	FROM InvoiceLines il
	WHERE il.InvoiceID = @invoiceId

	RETURN @result;

END
GO


ALTER TABLE Customers
ADD SendFormat int NOT NULL DEFAULT 1
GO


CREATE TABLE InvoiceFiles
(
	InvoiceID int NOT NULL,
	InvoiceFileID int NOT NULL IDENTITY(1,1),
	[File] varbinary(max) NOT NULL,
	FileType int NOT NULL,
	PRIMARY KEY (InvoiceFileID),
	FOREIGN KEY (InvoiceID) REFERENCES Invoices(ID)
)
GO

  
CREATE TABLE InvoiceTemplateFiles
(
	ID int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	InvoiceTemplateId int NOT NULL FOREIGN KEY REFERENCES InvoiceTemplates(TemplateId),
	[File] varbinary(max) NOT NULL
)
GO


ALTER TABLE InvoiceTemplates
DROP COLUMN FilePath
GO


  INSERT INTO Version ([Version], [Date], [Creator], [Description])
  VALUES ('2.4.7.2', GETDATE(), 'RAB', 'Create SP [FindBillableTimeEntries], [FindAllTimeEntries], [UpdateTimeEntriesInvoiceId],   [spGenerateNewInvoiceDraft], [spGenerateInvoiceLines], [spGetSpecificationData_Task], [spGetSpecificationData_Project],   [spFindTimeEntiresForInvoice], [spShowInvoiceLines], [FindVAT], [spGetInvoices], [spGetCustomersInvoiceView],  updated FK from various entities, removed duplicate columns from Invoices,  added coloumns to specify which InvoiceTemplate to use,  created table InvoiceFiles and InvoiceTemplateFile to store pdfs')
  GO
  


  INSERT INTO Version ([Version], [Date], [Creator], [Description])
  VALUES ('2.4.7.1', GETDATE(), 'RAB', '')
  GO
