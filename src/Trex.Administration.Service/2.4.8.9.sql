  
ALTER TABLE CustomerInvoiceGroup
ADD SendFormat int not null default 0
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.4.8.9', GETDATE(), 'RAB', 'Added column "SendFormat" to CIG')