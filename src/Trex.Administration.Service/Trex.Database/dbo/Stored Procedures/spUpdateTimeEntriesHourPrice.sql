
CREATE PROCEDURE [dbo].[spUpdateTimeEntriesHourPrice]
(
	@projectId int,
	@newPrice float,
	@invoicelineId int
)		
AS
BEGIN
	UPDATE te
	SET te.Price = @newPrice
	FROM TimeEntries te
		INNER JOIN Tasks t ON te.TaskID = t.TaskID
		INNER JOIN Projects p ON t.ProjectID = p.ProjectID
		INNER JOIN Invoices i ON i.ID = te.InvoiceId
		INNER JOIN InvoiceLines il ON il.InvoiceID = i.ID
	WHERE 
		p.ProjectID = @projectId
		AND il.ID = @invoicelineId
END

