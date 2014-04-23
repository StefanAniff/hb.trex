CREATE TABLE [dbo].[ForecastTypes] (
    [Id]                     INT           NOT NULL,
    [Name]                   VARCHAR (20)  NOT NULL,
    [Description]            VARCHAR (200) NULL,
    [SupportsProjectHours]   BIT           CONSTRAINT [DF_ForecastTypes_SupportsProjectHours] DEFAULT ((0)) NOT NULL,
    [SupportsDedicatedHours] BIT           CONSTRAINT [DF_ForecastTypes_SupportsDedicatedHours] DEFAULT ((0)) NOT NULL,
    [ColorStringHex]         VARCHAR (10)  CONSTRAINT [DF_ForecastTypes_ColorStringHex] DEFAULT ('#FFFFFFFF') NOT NULL,
    [StatisticsInclusion]    BIT           DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ForecastTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);





