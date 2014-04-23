CREATE TABLE [dbo].[InvoiceTemplates] (
    [TemplateId]              INT              IDENTITY (1, 1) NOT NULL,
    [TemplateName]            NVARCHAR (250)   NOT NULL,
    [CreateDate]              DATETIME         NOT NULL,
    [CreatedBy]               NVARCHAR (50)    NOT NULL,
    [Guid]                    UNIQUEIDENTIFIER NOT NULL,
    [StandardInvoicePrint]    BIT              DEFAULT ((0)) NOT NULL,
    [StandardInvoiceMail]     BIT              DEFAULT ((0)) NOT NULL,
    [StandardSpecification]   BIT              DEFAULT ((0)) NOT NULL,
    [StandardCreditNotePrint] BIT              DEFAULT ((0)) NOT NULL,
    [StandardCreditNoteMail]  BIT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_InvoiceTemplates] PRIMARY KEY CLUSTERED ([TemplateId] ASC)
);

