
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
