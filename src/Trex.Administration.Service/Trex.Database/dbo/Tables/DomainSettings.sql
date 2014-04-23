CREATE TABLE [dbo].[DomainSettings] (
    [Id]    INT           IDENTITY (1, 1) NOT NULL,
    [Name]  VARCHAR (50)  NOT NULL,
    [Value] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_DomainSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Name]
    ON [dbo].[DomainSettings]([Name] ASC);

