CREATE TABLE [dbo].[Invoices] (
    [ID]                     INT              IDENTITY (1, 1) NOT NULL,
    [CreateDate]             DATETIME         NOT NULL,
    [InvoiceDate]            DATETIME         CONSTRAINT [DF_Invoices_InvoiceDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]              INT              NOT NULL,
    [VAT]                    FLOAT (53)       NOT NULL,
    [FooterText]             NVARCHAR (1000)  NULL,
    [StartDate]              DATETIME         NOT NULL,
    [EndDate]                DATETIME         NOT NULL,
    [Closed]                 BIT              NOT NULL,
    [DueDate]                DATETIME         NULL,
    [Regarding]              NVARCHAR (100)   NULL,
    [InvoiceID]              INT              DEFAULT (NULL) NULL,
    [CustomerInvoiceGroupId] INT              NOT NULL,
    [Guid]                   UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Delivered]              BIT              DEFAULT ((0)) NOT NULL,
    [IsCreditNote]           BIT              DEFAULT ((0)) NOT NULL,
    [InvoiceLinkId]          INT              DEFAULT (NULL) NULL,
    [DeliveredDate]          DATETIME         NULL,
    [Attention]              NVARCHAR (100)   NULL,
    [AmountPaid]             FLOAT (53)       CONSTRAINT [AmountPaid_DefaultValue] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Invoices] PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([CustomerInvoiceGroupId]) REFERENCES [dbo].[CustomerInvoiceGroup] ([CustomerInvoiceGroupID]),
    CONSTRAINT [FK_Invoices_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserID])
);



