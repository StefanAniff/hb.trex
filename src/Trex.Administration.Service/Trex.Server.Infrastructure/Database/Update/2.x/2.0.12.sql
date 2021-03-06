
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[spGetGeneratedInvoiceLines] @InvoiceId INT

AS


SELECT 
      SUM(TimeSpent) BillableTime,[Price] as Price
     
      
  FROM [Trex].[dbo].[RoundedTimeSpentPrDayPrUser]
  where InvoiceId = @InvoiceId
  GROUP BY Price

  GO

  INSERT INTO [dbo].[Version] (Version,Date,Creator,Description)
  VALUES ('2.0.12',GetDate(),'tga','Updated DB to new service version')
  GO