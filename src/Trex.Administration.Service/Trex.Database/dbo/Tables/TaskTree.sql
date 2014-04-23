CREATE TABLE [dbo].[TaskTree] (
    [ParentID] INT NOT NULL,
    [ChildID]  INT NOT NULL,
    CONSTRAINT [PK_TaskTree] PRIMARY KEY CLUSTERED ([ParentID] ASC, [ChildID] ASC),
    CONSTRAINT [FK_TaskTree_Tasks] FOREIGN KEY ([ParentID]) REFERENCES [dbo].[Tasks] ([TaskID]),
    CONSTRAINT [FK_TaskTree_Tasks1] FOREIGN KEY ([ChildID]) REFERENCES [dbo].[Tasks] ([TaskID])
);

