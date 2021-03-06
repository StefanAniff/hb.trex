/*
   28. november 2013 
   User: IVA 
   Database: Trex
   Application: 
*/

INSERT INTO [dbo].[DomainSettings]
           ([Name]
           ,[Value])
     VALUES
           ('ProjectForecastTypeNameInt'
           ,1)
GO

-- Delete empty project forecasts
delete Forecasts
from
	Forecasts fc
	join ForecastMonths fm on fm.ForecastMonthId = fc.ForecastMonthId
	join Users usr on usr.UserID = fm.UserId
	join ForecastTypes ft on ft.Id = fc.ForecastTypeInt
	left join ForecastProjectHours fph on fph.ForecastId = fc.ForecastId
where
	fph.ForecastProjectHoursId is null
	and fc.ForecastTypeInt = 1
	
