/*
   21. oktober 2013 14:33
   User: IVA 
   Server: THOMAC
   Database: Trex
   Application: 
*/

CREATE TABLE [dbo].[Holidays] (
    [HolidayId]   INT          IDENTITY (1, 1) NOT NULL,
    [Date]        DATETIME     NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Holidays] PRIMARY KEY CLUSTERED ([HolidayId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Date]
    ON [dbo].[Holidays]([Date] ASC);
GO

CREATE TABLE [dbo].[ForecastTypes] (
    [Id]                     INT           NOT NULL,
    [Name]                   VARCHAR (20)  NOT NULL,
    [Description]            VARCHAR (200) NULL,
    [SupportsProjectHours]   BIT           CONSTRAINT [DF_ForecastTypes_SupportsProjectHours] DEFAULT ((0)) NOT NULL,
    [SupportsDedicatedHours] BIT           CONSTRAINT [DF_ForecastTypes_SupportsDedicatedHours] DEFAULT ((0)) NOT NULL,
    [ColorStringHex]         VARCHAR (10)  CONSTRAINT [DF_ForecastTypes_ColorStringHex] DEFAULT ('#FFFFFFFF') NOT NULL,
    CONSTRAINT [PK_ForecastTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [dbo].[Forecasts] (
    [ForecastId]                 INT            IDENTITY (1, 1) NOT NULL,
    [Date]                       DATE           NOT NULL,
    [UserId]                     INT            NOT NULL,
    [ForecastTypeInt]            INT            NULL,
    [DedicatedForecastTypeHours] DECIMAL (5, 2) NULL,
    [CreatedById]                INT            DEFAULT ((43)) NOT NULL,
    [CreatedDate]                DATETIME       DEFAULT ('20130101') NOT NULL,
    CONSTRAINT [PK_Forecasts] PRIMARY KEY CLUSTERED ([ForecastId] ASC),
    CONSTRAINT [FK_Forecasts_CreatedBy_Users] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[Users] ([UserID]),
    CONSTRAINT [FK_Forecasts_ForecastTypes] FOREIGN KEY ([ForecastTypeInt]) REFERENCES [dbo].[ForecastTypes] ([Id]),
    CONSTRAINT [FK_Forecasts_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserID])
);

GO
CREATE NONCLUSTERED INDEX [IX_Date]
    ON [dbo].[Forecasts]([Date] ASC, [UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserDate]
    ON [dbo].[Forecasts]([UserId] ASC, [Date] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_User]
    ON [dbo].[Forecasts]([UserId] ASC);



CREATE TABLE [dbo].[ForecastProjectHours] (
    [ForecastProjectHoursId] INT            IDENTITY (1, 1) NOT NULL,
    [Hours]                  DECIMAL (5, 2) NOT NULL,
    [ProjectId]              INT            NOT NULL,
    [ForecastId]             INT            NOT NULL,
    CONSTRAINT [PK_ClientForecastHours] PRIMARY KEY CLUSTERED ([ForecastProjectHoursId] ASC),
    CONSTRAINT [FK_ForecastProjectHours_Forecasts] FOREIGN KEY ([ForecastId]) REFERENCES [dbo].[Forecasts] ([ForecastId]),
    CONSTRAINT [FK_ForecastProjectHours_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([ProjectID])
);




GO
CREATE NONCLUSTERED INDEX [IX_ForecastId]
    ON [dbo].[ForecastProjectHours]([ForecastId] ASC);

GO


INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20130101','New Year’s Day')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20130328','Maundy Thursday')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20130329','Good Friday')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20130401','2. Easter Day')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20130426','General Prayer Day')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20130509','Ascension Day')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20130520','Whit Monday')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20131224','The day before Christmas')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20131225','Christmas Day')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20140101','New Year’s Day')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20140417','Maundy Thursday')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20140418','Good Friday')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20140421','2. Easter Day, Monday')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20140516','General Prayer Day, Friday')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20140529','Ascension Day, Thursday')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20140609','Whit Monday')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20141224','The day before Christmas')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20141225','Christmas Day')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20131226','Boxing Day')
INSERT INTO [dbo].[Holidays] ([Date] ,[Description]) VALUES ('20141226','Boxing Day')


INSERT INTO [dbo].[ForecastTypes]
           ([Id]
           ,[Name]
           ,[Description]
           ,[SupportsProjectHours]
           ,[SupportsDedicatedHours]
           ,[ColorStringHex])
     VALUES
           (1
           ,'Project'
           ,'Project'
           ,1
           ,0
           ,'#FF696969')
INSERT INTO [dbo].[ForecastTypes]
           ([Id]
           ,[Name]
           ,[Description]
           ,[SupportsProjectHours]
           ,[SupportsDedicatedHours]
           ,[ColorStringHex])
     VALUES
           (2
           ,'Illness'
           ,'Illness'
           ,0
           ,0
           ,'#FF8B0000')
INSERT INTO [dbo].[ForecastTypes]
           ([Id]
           ,[Name]
           ,[Description]
           ,[SupportsProjectHours]
           ,[SupportsDedicatedHours]
           ,[ColorStringHex])
     VALUES
           (3
           ,'Vacation'
           ,'Vacation'
           ,0
           ,0
           ,'#FF9400D3')
INSERT INTO [dbo].[ForecastTypes]
           ([Id]
           ,[Name]
           ,[Description]
           ,[SupportsProjectHours]
           ,[SupportsDedicatedHours]
           ,[ColorStringHex])
     VALUES
           (4
           ,'Open'
           ,'Open'
           ,1
           ,1
           ,'#FF008000')
INSERT INTO [dbo].[ForecastTypes]
           ([Id]
           ,[Name]
           ,[Description]
           ,[SupportsProjectHours]
           ,[SupportsDedicatedHours]
           ,[ColorStringHex])
     VALUES
           (5
           ,'Training'
           ,'Training'
           ,1
           ,1
           ,'#FFFFFF00')
INSERT INTO [dbo].[ForecastTypes]
           ([Id]
           ,[Name]
           ,[Description]
           ,[SupportsProjectHours]
           ,[SupportsDedicatedHours]
           ,[ColorStringHex])
     VALUES
           (6
           ,'Leave'
           ,'Leave'
           ,0
           ,0
           ,'#FFD2691E')