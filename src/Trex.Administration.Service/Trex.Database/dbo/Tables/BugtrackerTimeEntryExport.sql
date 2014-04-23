CREATE TABLE [dbo].[BugtrackerTimeEntryExport] (
    [BugtrackerTimeEntryExportID] INT          IDENTITY (1, 1) NOT NULL,
    [ProjectID]                   INT          NOT NULL,
    [TaskID]                      INT          NOT NULL,
    [TimeEntryID]                 INT          NOT NULL,
    [GeminiIssueID]               NUMERIC (10) NOT NULL,
    [GeminiTimeLogID]             NUMERIC (10) NOT NULL,
    [ExportDate]                  DATETIME     NOT NULL,
    CONSTRAINT [PK_BugtrackerTimeEntryExport] PRIMARY KEY CLUSTERED ([BugtrackerTimeEntryExportID] ASC)
);

