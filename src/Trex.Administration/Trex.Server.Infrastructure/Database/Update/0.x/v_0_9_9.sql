
/****** Object:  StoredProcedure [dbo].[spBookTimeEntries]    Script Date: 04/01/2009 10:10:15 ******/
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


DROP PROC [dbo].[spBookUnbilledInvoiceLines]
GO


INSERT INTO Version (Version ,Date,Creator,Description) VALUES('0.9.9.0',GetDate(),'tga','New proc for booking timeentries')
	GO