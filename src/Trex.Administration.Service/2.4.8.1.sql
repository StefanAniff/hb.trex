--Default gruppe: 0 = false, 1 = true
ALTER TABLE CustomerInvoiceGroup
ADD [DefaultCig] bit NOT NULL DEFAULT 0
GO

--Update all since they must be the default
UPDATE CustomerInvoiceGroup
SET DefaultCig = 1
GO

INSERT INTO Version ([Version], [Date], [Creator], [Description])
VALUES ('2.4.8.1', GETDATE(), 'RAB', 'Added DefaultCig to CustomerInvoiceGroup and set them to true')
GO