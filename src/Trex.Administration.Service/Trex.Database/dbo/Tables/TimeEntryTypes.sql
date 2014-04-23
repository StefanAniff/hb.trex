CREATE TABLE [dbo].[TimeEntryTypes] (
    [TimeEntryTypeId]     INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]          INT            NULL,
    [Name]                NVARCHAR (100) NOT NULL,
    [IsDefault]           BIT            NOT NULL,
    [IsBillableByDefault] BIT            NOT NULL,
    CONSTRAINT [PK_TimeEntryTypes] PRIMARY KEY CLUSTERED ([TimeEntryTypeId] ASC),
    CONSTRAINT [FK_CustomersTimeEntryTypes_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([CustomerID])
);

