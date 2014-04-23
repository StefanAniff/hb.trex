CREATE TABLE [dbo].[ForecastProjectHours] (
    [ForecastProjectHoursId] INT            IDENTITY (1, 1) NOT NULL,
    [Hours]                  DECIMAL (5, 2) NOT NULL,
    [ProjectId]              INT            NOT NULL,
    [ForecastId]             INT            NOT NULL,
    CONSTRAINT [PK_ClientForecastHours] PRIMARY KEY CLUSTERED ([ForecastProjectHoursId] ASC),
    CONSTRAINT [FK_ForecastProjectHours_Forecasts] FOREIGN KEY ([ForecastId]) REFERENCES [dbo].[Forecasts] ([ForecastId]),
    CONSTRAINT [FK_ForecastProjectHours_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([ProjectID])
);




GO
CREATE NONCLUSTERED INDEX [IX_ForecastId]
    ON [dbo].[ForecastProjectHours]([ForecastId] ASC);

