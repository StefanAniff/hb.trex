CREATE TABLE [dbo].[InvoiceTemplateFiles] (
    [ID]                INT             IDENTITY (1, 1) NOT NULL,
    [InvoiceTemplateId] INT             NOT NULL,
    [File]              VARBINARY (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([InvoiceTemplateId]) REFERENCES [dbo].[InvoiceTemplates] ([TemplateId])
);

