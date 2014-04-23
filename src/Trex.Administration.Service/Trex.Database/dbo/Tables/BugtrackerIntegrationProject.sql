CREATE TABLE [dbo].[BugtrackerIntegrationProject] (
    [BugtrackerIntegrationProjectID] INT          NOT NULL,
    [TrexProjectID]                  INT          NOT NULL,
    [GeminiProjectID]                NUMERIC (10) NOT NULL,
    [TwoWaySync]                     BIT          CONSTRAINT [DF_BugtrackerIntegrationProject_TwoWaySync] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BugtrackerIntegration] PRIMARY KEY CLUSTERED ([BugtrackerIntegrationProjectID] ASC)
);

