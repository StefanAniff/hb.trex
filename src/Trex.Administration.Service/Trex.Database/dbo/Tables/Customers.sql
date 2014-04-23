CREATE TABLE [dbo].[Customers] (
    [CustomerID]                      INT              IDENTITY (1, 1) NOT NULL,
    [Guid]                            UNIQUEIDENTIFIER CONSTRAINT [DF_Customers_Guid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [CreateDate]                      DATETIME         CONSTRAINT [DF_Customers_CreateDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                       INT              NOT NULL,
    [CustomerName]                    NVARCHAR (250)   NOT NULL,
    [PhoneNumber]                     NVARCHAR (50)    NULL,
    [Email]                           NVARCHAR (255)   NULL,
    [Inactive]                        BIT              CONSTRAINT [DF_Customers_Inactive] DEFAULT ((0)) NOT NULL,
    [StreetAddress]                   NVARCHAR (400)   NULL,
    [ZipCode]                         NVARCHAR (50)    NULL,
    [City]                            NVARCHAR (100)   NULL,
    [Country]                         NVARCHAR (200)   NULL,
    [ContactName]                     NVARCHAR (200)   NULL,
    [ContactPhone]                    NVARCHAR (100)   NULL,
    [InheritsTimeEntryTypes]          BIT              CONSTRAINT [DF_Customers_InheritsTimeEntryTypes] DEFAULT ((1)) NOT NULL,
    [ChangeDate]                      DATETIME         NULL,
    [ChangedBy]                       INT              NULL,
    [PaymentTermsNumberOfDays]        INT              CONSTRAINT [DF_Customers_PaymentTermsNumberOfDays] DEFAULT ((0)) NOT NULL,
    [PaymentTermsIncludeCurrentMonth] BIT              CONSTRAINT [DF_Customers_PaymentTermsIncludeCurrentMonth] DEFAULT ((1)) NOT NULL,
    [Address2]                        NVARCHAR (400)   NULL,
    [SendFormat]                      INT              DEFAULT ((1)) NOT NULL,
    [EmailCC]                         NVARCHAR (255)   NULL,
    [Internal]                        BIT              NULL,
    [UserId]                          INT              NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([CustomerID] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserID]),
    CONSTRAINT [FK_Customers_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserID])
);



