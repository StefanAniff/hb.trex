CREATE TABLE [dbo].[Tasks] (
    [TaskID]                 INT              IDENTITY (1, 1) NOT NULL,
    [ParentID]               NCHAR (10)       NULL,
    [Guid]                   UNIQUEIDENTIFIER CONSTRAINT [DF_Tasks_Guid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [ProjectID]              INT              NOT NULL,
    [CreatedBy]              INT              NOT NULL,
    [ModifyDate]             DATETIME         CONSTRAINT [DF_Tasks_ModifyDate] DEFAULT (getdate()) NOT NULL,
    [CreateDate]             DATETIME         CONSTRAINT [DF_Tasks_CreateDate] DEFAULT (getdate()) NOT NULL,
    [TaskName]               NVARCHAR (350)   NOT NULL,
    [Description]            NVARCHAR (500)   NULL,
    [TimeEstimated]          FLOAT (53)       CONSTRAINT [DF_Tasks_TimeEstimated] DEFAULT ((0)) NOT NULL,
    [TimeLeft]               FLOAT (53)       NOT NULL,
    [Closed]                 BIT              CONSTRAINT [DF_Tasks_Closed] DEFAULT ((0)) NOT NULL,
    [WorstCaseEstimate]      FLOAT (53)       CONSTRAINT [DF_Tasks_WorstCaseEstimate] DEFAULT ((0)) NOT NULL,
    [BestCaseEstimate]       FLOAT (53)       CONSTRAINT [DF_Tasks_BestCaseEstimate] DEFAULT ((0)) NOT NULL,
    [TagID]                  INT              NULL,
    [RealisticEstimate]      FLOAT (53)       CONSTRAINT [DF_Tasks_RealisticEstimate] DEFAULT ((0)) NOT NULL,
    [Inactive]               BIT              CONSTRAINT [DF_Tasks_Inactive] DEFAULT ((0)) NOT NULL,
    [ChangeDate]             DATETIME         NULL,
    [ChangedBy]              INT              NULL,
    [TimeRegistrationTypeId] INT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED ([TaskID] ASC),
    CONSTRAINT [FK__Tasks__ProjectID__5A846E65] FOREIGN KEY ([ProjectID]) REFERENCES [dbo].[Projects] ([ProjectID]),
    CONSTRAINT [FK__Tasks__ProjectID__5B78929E] FOREIGN KEY ([ProjectID]) REFERENCES [dbo].[Projects] ([ProjectID]),
    CONSTRAINT [FK_Tasks_Tags] FOREIGN KEY ([TagID]) REFERENCES [dbo].[Tags] ([TagID]),
    CONSTRAINT [FK_Tasks_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserID])
);






GO
CREATE NONCLUSTERED INDEX [TasksTaskID_ix]
    ON [dbo].[Tasks]([TaskID] ASC)
    INCLUDE([ProjectID], [TaskName], [TimeLeft]);


GO
CREATE NONCLUSTERED INDEX [TasksprojectID_ix]
    ON [dbo].[Tasks]([ProjectID] ASC)
    INCLUDE([TaskID], [TaskName], [TimeLeft]);

