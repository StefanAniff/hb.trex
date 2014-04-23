CREATE TABLE [dbo].[Permissions] (
    [ID]                  INT            IDENTITY (1, 1) NOT NULL,
    [Permission]          NVARCHAR (100) NOT NULL,
    [ClientApplicationID] INT            NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([ID] ASC)
);

