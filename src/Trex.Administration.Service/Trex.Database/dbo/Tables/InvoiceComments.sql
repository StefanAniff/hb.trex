CREATE TABLE [dbo].[InvoiceComments] (
    [InvoiceCommentID] INT            IDENTITY (1, 1) NOT NULL,
    [UserID]           INT            NOT NULL,
    [InvoiceID]        INT            NOT NULL,
    [Comment]          NVARCHAR (255) NULL,
    [TimeStamp]        DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([InvoiceCommentID] ASC),
    FOREIGN KEY ([InvoiceID]) REFERENCES [dbo].[Invoices] ([ID]),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

