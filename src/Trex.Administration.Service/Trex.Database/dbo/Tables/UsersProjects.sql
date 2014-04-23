CREATE TABLE [dbo].[UsersProjects] (
    [ProjectID] INT NOT NULL,
    [UserID]    INT NOT NULL,
    CONSTRAINT [PK_UsersProjects] PRIMARY KEY CLUSTERED ([ProjectID] ASC, [UserID] ASC),
    CONSTRAINT [FK_UsersProjects_Projects] FOREIGN KEY ([ProjectID]) REFERENCES [dbo].[Projects] ([ProjectID]),
    CONSTRAINT [FK_UsersProjects_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

