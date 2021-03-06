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
   
/****** Object:  StoredProcedure [dbo].[spBookUnbilledInvoiceLines]    Script Date: 11/19/2008 13:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[spBookUnbilledInvoiceLines] @invoiceId INT, @customerId INT, @startdate DATETIME, @enddate DATETIME, @excludeList NVARCHAR(1000)=NULL
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


INSERT INTO Version (Version ,Date,Creator,Description) VALUES('0.9.5.0',GetDate(),'tga','Selectable timeentries when creating invoices')
	GO
