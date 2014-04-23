CREATE TABLE [dbo].[UsersCustomers] (
    [UserID]     INT        NOT NULL,
    [CustomerID] INT        NOT NULL,
    [Price]      FLOAT (53) NOT NULL,
    CONSTRAINT [PK_UsersCustomers] PRIMARY KEY CLUSTERED ([UserID] ASC, [CustomerID] ASC),
    CONSTRAINT [FK_UsersCustomers_Customers] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID]),
    CONSTRAINT [FK_UsersCustomers_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

