CREATE TABLE [dbo].[BugtrackerIntegrationUser] (
    [BugtrackerIntegrationUserID] INT          IDENTITY (1, 1) NOT NULL,
    [TrexUserID]                  INT          NOT NULL,
    [GeminiUserID]                NUMERIC (10) NOT NULL,
    CONSTRAINT [PK_BugtrackerIntegrationUser] PRIMARY KEY CLUSTERED ([BugtrackerIntegrationUserID] ASC)
);

