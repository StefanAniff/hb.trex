CREATE TABLE [dbo].[Users] (
    [UserID]     INT            IDENTITY (1, 1) NOT NULL,
    [UserName]   NVARCHAR (255) NOT NULL,
    [Name]       NVARCHAR (200) NULL,
    [Email]      NVARCHAR (100) NULL,
    [Price]      FLOAT (53)     NOT NULL,
    [Inactive]   BIT            CONSTRAINT [DF_Users_Inactive] DEFAULT ((0)) NOT NULL,
    [Department] NVARCHAR (255) NULL,
    [Location]   NVARCHAR (255) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

