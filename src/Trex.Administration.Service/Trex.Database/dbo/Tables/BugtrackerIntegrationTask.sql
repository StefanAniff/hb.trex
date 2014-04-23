CREATE TABLE [dbo].[BugtrackerIntegrationTask] (
    [BugtrackerIntegrationTaskID] INT          IDENTITY (1, 1) NOT NULL,
    [TrexTaskID]                  INT          NOT NULL,
    [GeminiTaskID]                NUMERIC (10) NOT NULL,
    CONSTRAINT [PK_BugtrackerIntegrationTask] PRIMARY KEY CLUSTERED ([BugtrackerIntegrationTaskID] ASC)
);

