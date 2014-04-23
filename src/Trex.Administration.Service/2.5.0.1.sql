CREATE TABLE DocumentType
(
	ID int PRIMARY KEY NOT NULL IDENTITY,
	Name nvarchar(50)
)
GO

INSERT INTO DocumentType
VALUES('Non-Credited')
INSERT INTO DocumentType
VALUES('Invoice')
INSERT INTO DocumentType
VALUES('Credit note')
GO

ALTER TABLE TimeEntries
ADD DocumentType int Default 1 NOT NULL
GO

ALTER TABLE TimeEntries
ADD CONSTRAINT fk_DocumentType
FOREIGN KEY (DocumentType)
REFERENCES DocumentType(ID)
GO

UPDATE TimeEntries
SET DocumentType = 1
WHERE InvoiceId IS NULL
GO

UPDATE TimeEntries
SET DocumentType = 2
WHERE InvoiceId IS NOT NULL
GO

ALTER TABLE TimeEntries
ALTER COLUMN DocumentType int NOT NULL
GO

CREATE TABLE CreditNote
(
	ID int PRIMARY KEY NOT NULL IDENTITY,
	InvoiceID int,
	TimeEntryID int,
	TaskID int,
	UserID int,
	StartTime datetime,
	EndTime datetime,
	Description nvarchar(1000),
	PauseTime float,
	BillableTime float,
	Billable bit,
	Price float,
	TimeSpent float,
	Guid uniqueidentifier,
	TimeEntryTypeId int,
	ChangeDate datetime,
	ChangedBy int,
	CreateDate datetime,
	ClientSourceId int,
	CreditNoteDate Datetime,
	FOREIGN KEY(InvoiceID) REFERENCES Invoices(ID),
	FOREIGN KEY(TimeEntryID) REFERENCES TimeEntries(TimeEntryID)
)
GO


INSERT INTO CreditNote
	(
	 Billable,
	 BillableTime,
	 ChangedBy,
	 ChangeDate,
	 ClientSourceId,
	 CreateDate,
	 CreditNoteDate,
	 Description,
	 EndTime,
	 Guid,
	 InvoiceID,
	 PauseTime,
	 Price,
	 StartTime,
	 TaskID,
	 TimeEntryID,
	 TimeEntryTypeId,
	 TimeSpent,
	 UserID
	)
SELECT 	
	te.Billable,
	te.BillableTime,
	te.ChangedBy,
	te.ChangeDate,
	te.ClientSourceId,
	te.CreateDate,
	i.InvoiceDate,
	te.Description,
	te.EndTime,
	te.Guid,
	te.InvoiceID,
	te.PauseTime,
	te.Price,
	te.StartTime,
	te.TaskID,
	te.TimeEntryID,
	te.TimeEntryTypeId,
	te.TimeSpent,
	te.UserID
FROM TimeEntries te
	INNER JOIN Invoices i ON i.ID = te.InvoiceId
WHERE te.DocumentType = 2
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.5.0.1', GETDATE(), 'LLS', 'Added 2 new tables, Credit note and Document type also added 1 new row to TimeEntries with FK to DocumentType, copied old data to this table')
GO
