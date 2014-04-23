CREATE TABLE [dbo].[Version] (
    [Version]     NVARCHAR (50)  NOT NULL,
    [Date]        DATETIME       NOT NULL,
    [Creator]     NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (500) NULL,
    CONSTRAINT [pk_Version] PRIMARY KEY CLUSTERED ([Version] ASC)
);

