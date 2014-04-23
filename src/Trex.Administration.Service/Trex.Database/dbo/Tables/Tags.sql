CREATE TABLE [dbo].[Tags] (
    [TagID]      INT            NOT NULL,
    [CustomerID] INT            NOT NULL,
    [TagText]    NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED ([TagID] ASC),
    CONSTRAINT [FK_Tags_Customers] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID])
);

