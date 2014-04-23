CREATE TABLE [dbo].[ForecastMonths] (
    [ForecastMonthId] INT      IDENTITY (1, 1) NOT NULL,
    [Month]           INT      NOT NULL,
    [Year]            INT      NOT NULL,
    [UserId]          INT      NOT NULL,
    [UnLocked]        BIT      NOT NULL,
    [LockedFrom]      DATE     NOT NULL,
    [CreatedById]     INT      NOT NULL,
    [CreatedDate]     DATETIME NOT NULL,
    CONSTRAINT [PK_ForecastMonths] PRIMARY KEY CLUSTERED ([ForecastMonthId] ASC),
    CONSTRAINT [FK_CratedBy_Users] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[Users] ([UserID]),
    CONSTRAINT [FK_ForecastMonths_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserID])
);


GO
CREATE NONCLUSTERED INDEX [IX_MonthYear]
    ON [dbo].[ForecastMonths]([Month] ASC, [Year] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserMonthYear]
    ON [dbo].[ForecastMonths]([UserId] ASC, [Month] ASC, [Year] ASC);

