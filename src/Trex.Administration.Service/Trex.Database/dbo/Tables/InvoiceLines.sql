CREATE TABLE [dbo].[InvoiceLines] (
    [ID]            INT             IDENTITY (1, 1) NOT NULL,
    [Text]          NVARCHAR (1000) NOT NULL,
    [PricePrUnit]   FLOAT (53)      NOT NULL,
    [InvoiceID]     INT             NOT NULL,
    [Units]         FLOAT (53)      NOT NULL,
    [Unit]          NVARCHAR (50)   NULL,
    [UnitType]      INT             CONSTRAINT [DF_InvoiceLines_UnitType] DEFAULT ((1)) NOT NULL,
    [SortIndex]     INT             CONSTRAINT [DF_InvoiceLines_SortIndex] DEFAULT ((0)) NOT NULL,
    [IsExpense]     BIT             CONSTRAINT [DF_InvoiceLines_IsExpense] DEFAULT ((0)) NOT NULL,
    [VatPercentage] FLOAT (53)      CONSTRAINT [DF_InvoiceLines_VatPercentage] DEFAULT ((0.25)) NOT NULL,
    CONSTRAINT [PK_InvoiceLines] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_InvoiceLines_Invoices] FOREIGN KEY ([InvoiceID]) REFERENCES [dbo].[Invoices] ([ID])
);

