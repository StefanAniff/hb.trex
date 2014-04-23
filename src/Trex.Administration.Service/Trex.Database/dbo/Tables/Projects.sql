CREATE TABLE [dbo].[Projects] (
    [ProjectID]              INT              IDENTITY (1, 1) NOT NULL,
    [Guid]                   UNIQUEIDENTIFIER CONSTRAINT [DF_Projects_Guid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [CustomerID]             INT              NOT NULL,
    [ProjectName]            NVARCHAR (200)   NOT NULL,
    [CreatedBy]              INT              NOT NULL,
    [CreateDate]             DATETIME         CONSTRAINT [DF_Projects_CreateDate] DEFAULT (getdate()) NOT NULL,
    [Inactive]               BIT              CONSTRAINT [DF_Projects_Inactive] DEFAULT ((0)) NOT NULL,
    [IsEstimatesEnabled]     BIT              CONSTRAINT [DF_Projects_IsEstimatesEnabled] DEFAULT ((0)) NOT NULL,
    [ChangeDate]             DATETIME         NULL,
    [ChangedBy]              INT              NULL,
    [PONumber]               NVARCHAR (50)    NULL,
    [FixedPriceProject]      BIT              CONSTRAINT [DF_Projects_FixedPriceProject] DEFAULT ((0)) NOT NULL,
    [FixedPrice]             DECIMAL (18)     NULL,
    [CustomerInvoiceGroupID] INT              CONSTRAINT [DF__Projects__Custom__55BFB948] DEFAULT ((0)) NOT NULL,
    [EstimatedHours]         INT              NULL,
    CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED ([ProjectID] ASC),
    CONSTRAINT [FK__Projects__Custom__59904A2C] FOREIGN KEY ([CustomerInvoiceGroupID]) REFERENCES [dbo].[CustomerInvoiceGroup] ([CustomerInvoiceGroupID]),
    CONSTRAINT [FK_Projects_Customers] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID]),
    CONSTRAINT [FK_Projects_Users] FOREIGN KEY ([ChangedBy]) REFERENCES [dbo].[Users] ([UserID])
);





