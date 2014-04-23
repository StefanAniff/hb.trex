CREATE TABLE [dbo].[Forecasts] (
    [ForecastId]                 INT            IDENTITY (1, 1) NOT NULL,
    [Date]                       DATE           NOT NULL,
    [ForecastTypeInt]            INT            NULL,
    [DedicatedForecastTypeHours] DECIMAL (5, 2) NULL,
    [ForecastMonthId]            INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Forecasts] PRIMARY KEY CLUSTERED ([ForecastId] ASC),
    CONSTRAINT [FK_ForecastMonth] FOREIGN KEY ([ForecastMonthId]) REFERENCES [dbo].[ForecastMonths] ([ForecastMonthId]),
    CONSTRAINT [FK_Forecasts_ForecastTypes] FOREIGN KEY ([ForecastTypeInt]) REFERENCES [dbo].[ForecastTypes] ([Id])
);






GO



GO



GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ForecastMonthDate]
    ON [dbo].[Forecasts]([ForecastMonthId] ASC, [Date] ASC);

