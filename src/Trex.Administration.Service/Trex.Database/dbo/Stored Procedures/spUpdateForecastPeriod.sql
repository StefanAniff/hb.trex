


create procedure [dbo].[spUpdateForecastPeriod] 
(
		@userid int,
		@projectid int,
		@forecasttypeid int,
		@startdate date,
		@enddate date,
		@monthhours decimal(5,2)
)
as
begin
	declare
		@creatorid int,
		@dayhours decimal(5,2)

	declare @forecastMonthsIds table (id int not null)

	select
		-- STATIC FROM HERE. DONT TOUCH :P
		@creatorid = 43, -- IVA
		@dayhours = @monthhours / 20 -- 20 workday avarage for a month	

	-- ##### DELETE ROWS FOR REPLACING #####
	-- delete all old entries
	delete from ForecastProjectHours
	from 
		ForecastProjectHours fph
		join Forecasts fc on fc.ForecastId = fph.ForecastId
		join ForecastMonths fm on fm.ForecastMonthId = fc.ForecastMonthId
	where
		fc.Date >= @startdate
		and fc.Date < @enddate
		and fm.UserId = @userid

	-- Remember months to delete
	insert into @forecastMonthsIds
	select
		distinct fm.ForecastMonthId
	from
		Forecasts fc
		join ForecastMonths fm on fm.ForecastMonthId = fc.ForecastMonthId
	where
		fc.Date >= @startdate
		and fc.Date < @enddate
		and fm.UserId = @userid

	-- Delete forecast children
	delete from Forecasts
	from 
		Forecasts fc 
		join ForecastMonths fm on fm.ForecastMonthId = fc.ForecastMonthId
	where
		fc.Date >= @startdate
		and fc.Date < @enddate
		and fm.UserId = @userid

	-- Delete forecast parents
	delete from ForecastMonths
	from 
		ForecastMonths fm
		join @forecastMonthsIds fmids on fmids.id = fm.ForecastMonthId
	

	-- ##### INSERT #####

	-- ForecastMonths
	insert into ForecastMonths (Month, Year, UserId, UnLocked, LockedFrom, CreatedById, CreatedDate)
	select distinct
		DATEPART(MONTH, cal.date),
		DATEPART(YEAR, cal.date),
		@userid,
		0,
		DATEADD(month, 1, (rtrim(cast((DATEPART(YEAR, cal.Date)) as varchar)) +'-'+ rtrim(cast((DATEPART(MONTH, cal.Date)) as varchar)) +'-'+ (select top 1 Value from DomainSettings where Name = 'PastMonthsDayLockInt'))),
		@creatorid,
		GETDATE()
	from
		tmp_calendar cal
	where
		cal.date >= @startdate
		and cal.date < @enddate

	-- Forecasts
	insert into Forecasts
	select
		cal.date,
		@forecasttypeid,
		null, -- DedicatedHours
		(select top 1 ForecastMonthId from ForecastMonths where UserId = @userid and Month = DATEPART(MONTH, cal.date) and Year = DATEPART(YEAR, cal.date))
	from 
		tmp_calendar cal
		left join Holidays hol on hol.Date = cal.date
	where
		hol.HolidayId is null -- exclude holidays
		and cal.date >= @startdate
		and cal.date < @enddate
		and cal.weekday not in (0,1)

	-- ForecastProjectHours
	if (@projectid > 0 and @forecasttypeid = 1)
	begin
		insert into ForecastProjectHours
		select
			@dayhours,
			@projectid,
			fc.ForecastId
		from
			Forecasts fc
			join ForecastMonths fm on fm.ForecastMonthId = fc.ForecastMonthId
		where
			fm.UserId = @userid
			and fc.Date >= @startdate
			and fc.Date < @enddate
			and fc.ForecastTypeInt = @forecasttypeid
	end
end