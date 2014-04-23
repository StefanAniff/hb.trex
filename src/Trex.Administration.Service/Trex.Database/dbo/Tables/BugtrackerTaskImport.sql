CREATE TABLE [dbo].[BugtrackerTaskImport] (
    [BugtrackerTaskImportID] INT          IDENTITY (1, 1) NOT NULL,
    [TaskID]                 INT          NOT NULL,
    [GeminiIssueID]          NUMERIC (10) NOT NULL,
    [ImportDate]             DATETIME     NOT NULL,
    CONSTRAINT [PK_BugtrackerTaskImport] PRIMARY KEY CLUSTERED ([BugtrackerTaskImportID] ASC)
);

