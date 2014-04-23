CREATE TABLE [dbo].[TimeEntries] (
    [TimeEntryID]     INT              IDENTITY (1, 1) NOT NULL,
    [TaskID]          INT              NOT NULL,
    [UserID]          INT              NOT NULL,
    [StartTime]       DATETIME         NOT NULL,
    [EndTime]         DATETIME         NOT NULL,
    [Description]     NVARCHAR (1000)  NULL,
    [PauseTime]       FLOAT (53)       NOT NULL,
    [BillableTime]    FLOAT (53)       NOT NULL,
    [Billable]        BIT              NOT NULL,
    [Price]           FLOAT (53)       NOT NULL,
    [TimeSpent]       FLOAT (53)       CONSTRAINT [DF_TimeEntries_TimeSpent] DEFAULT ((0)) NOT NULL,
    [InvoiceId]       INT              NULL,
    [Guid]            UNIQUEIDENTIFIER CONSTRAINT [DF_TimeEntries_Guid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [TimeEntryTypeId] INT              CONSTRAINT [DF_TimeEntries_TimeEntryType] DEFAULT ((1)) NOT NULL,
    [ChangeDate]      DATETIME         NULL,
    [ChangedBy]       INT              NULL,
    [CreateDate]      DATETIME         CONSTRAINT [DF_TimeEntries_CreateDate] DEFAULT (getdate()) NULL,
    [ClientSourceId]  INT              CONSTRAINT [DF_TimeEntries_ClientSourceId] DEFAULT ((0)) NOT NULL,
    [DocumentType]    INT              DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TimeRegs] PRIMARY KEY CLUSTERED ([TimeEntryID] ASC),
    FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoices] ([ID]),
    FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoices] ([ID]),
    CONSTRAINT [fk_DocumentType] FOREIGN KEY ([DocumentType]) REFERENCES [dbo].[DocumentType] ([ID]),
    CONSTRAINT [FK_TimeEntries_TimeEntryTypes] FOREIGN KEY ([TimeEntryTypeId]) REFERENCES [dbo].[TimeEntryTypes] ([TimeEntryTypeId]),
    CONSTRAINT [FK_TimeEntries_Users] FOREIGN KEY ([ChangedBy]) REFERENCES [dbo].[Users] ([UserID]),
    CONSTRAINT [FK_TimeRegs_Tasks] FOREIGN KEY ([TaskID]) REFERENCES [dbo].[Tasks] ([TaskID]),
    CONSTRAINT [FK_TimeRegs_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);




GO
CREATE NONCLUSTERED INDEX [TimeEntriesBillableDocType_ix]
    ON [dbo].[TimeEntries]([Billable] ASC, [DocumentType] ASC)
    INCLUDE([TaskID], [StartTime], [EndTime], [Price], [TimeSpent]);


GO
CREATE NONCLUSTERED INDEX [TimeEntriesBillableStartTime_ix]
    ON [dbo].[TimeEntries]([StartTime] ASC, [BillableTime] ASC);


GO
CREATE NONCLUSTERED INDEX [TimeEntriesBillableInvoiceIdDocType_ix]
    ON [dbo].[TimeEntries]([Billable] ASC, [InvoiceId] ASC, [DocumentType] ASC)
    INCLUDE([TaskID], [Price]);


GO
CREATE NONCLUSTERED INDEX [IX_TaskIdCreateDate]
    ON [dbo].[TimeEntries]([TaskID] ASC, [CreateDate] ASC);

