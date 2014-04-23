CREATE TABLE [dbo].[InvoiceFiles] (
    [InvoiceID]     INT             NOT NULL,
    [InvoiceFileID] INT             IDENTITY (1, 1) NOT NULL,
    [File]          VARBINARY (MAX) NOT NULL,
    [FileType]      INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([InvoiceFileID] ASC),
    FOREIGN KEY ([InvoiceID]) REFERENCES [dbo].[Invoices] ([ID])
);

