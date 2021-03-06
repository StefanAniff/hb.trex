/*
   29. oktober 2013 
   User: IVA 
   Database: Trex
   Application: 
*/

alter table forecasttypes
add StatisticsInclusion bit not null default(1)
go

update ForecastTypes 
set StatisticsInclusion = 0
where Name = 'Illness'
go

CREATE TABLE [dbo].[DomainSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Value] [varchar](100) NOT NULL,
 CONSTRAINT [PK_DomainSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)) 
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Name] ON [dbo].[DomainSettings]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


INSERT INTO [dbo].[DomainSettings]
           ([Name]
           ,[Value])
     VALUES
           ('VacationForecastTypeIdInt', '3')

INSERT INTO [dbo].[DomainSettings]
           ([Name]
           ,[Value])
     VALUES
           ('D60CustomerIdsIntList', '5001,5072')
GO