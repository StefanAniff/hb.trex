CREATE TABLE [dbo].[CustomerInvoiceGroup] (
    [CustomerInvoiceGroupID]      INT            IDENTITY (1, 1) NOT NULL,
    [CustomerID]                  INT            NOT NULL,
    [Label]                       NVARCHAR (250) DEFAULT ('') NOT NULL,
    [Attention]                   NVARCHAR (250) DEFAULT (NULL) NULL,
    [City]                        NVARCHAR (100) DEFAULT (NULL) NULL,
    [Country]                     NVARCHAR (200) DEFAULT (NULL) NULL,
    [Address1]                    NVARCHAR (400) DEFAULT (NULL) NULL,
    [Address2]                    NVARCHAR (400) DEFAULT (NULL) NULL,
    [ZipCode]                     NVARCHAR (50)  DEFAULT (NULL) NULL,
    [Email]                       NVARCHAR (200) DEFAULT (NULL) NULL,
    [InvoiceTemplateIdPrint]      INT            DEFAULT (NULL) NULL,
    [InvoiceTemplateIdMail]       INT            DEFAULT (NULL) NULL,
    [SpecificationTemplateIdMail] INT            DEFAULT (NULL) NULL,
    [DefaultCig]                  BIT            DEFAULT ((0)) NOT NULL,
    [SendFormat]                  INT            DEFAULT ((0)) NOT NULL,
    [CreditNoteTemplateIdMail]    INT            NULL,
    [CreditNoteTemplateIdPrint]   INT            NULL,
    [EmailCC]                     NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([CustomerInvoiceGroupID] ASC),
    FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID])
);

