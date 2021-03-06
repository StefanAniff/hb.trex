/*
   1. november 2013 
   User: IVA 
   Database: Trex
   Application: 
*/

insert into DomainSettings(Name, Value)
values ('PastMonthsDayLockInt', 3)
go


-- NEW TABLE FORECASTMONTHS
CREATE TABLE [dbo].[ForecastMonths](
	[ForecastMonthId] [int] IDENTITY(1,1) NOT NULL,
	[Month] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[UnLocked] [bit] NOT NULL,
	[LockedFrom] [date] NOT NULL,
	[CreatedById] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL
 CONSTRAINT [PK_ForecastMonths] PRIMARY KEY CLUSTERED 
(
	[ForecastMonthId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)) 
GO

ALTER TABLE [dbo].[ForecastMonths]  WITH CHECK ADD  CONSTRAINT [FK_ForecastMonths_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ForecastMonths] CHECK CONSTRAINT [FK_ForecastMonths_Users]
GO

ALTER TABLE [dbo].[ForecastMonths]  WITH CHECK ADD  CONSTRAINT [FK_CratedBy_Users] FOREIGN KEY([CreatedById])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ForecastMonths] CHECK CONSTRAINT [FK_CratedBy_Users]
GO

-- INSERT INTO FORECASTMONTHS
insert into ForecastMonths (Month, Year, UserId, UnLocked, LockedFrom, CreatedById, CreatedDate)
select 	
	DATEPART(MONTH, fc.Date) as 'Month',
	DATEPART(YEAR, fc.Date) as 'Year',
	fc.UserId,
	0,
	DATEADD(month, 1, (rtrim(cast((DATEPART(YEAR, fc.Date)) as varchar)) +'-'+ rtrim(cast((DATEPART(MONTH, fc.Date)) as varchar)) +'-'+ (select top 1 Value from DomainSettings where Name = 'PastMonthsDayLockInt'))),
	fc.UserId,
	(rtrim(cast((DATEPART(YEAR, fc.Date)) as varchar)) +'-'+ rtrim(cast((DATEPART(MONTH, fc.Date)) as varchar)) +'-'+ '1')
from 
	Forecasts fc
group by
	fc.UserId,
	DATEPART(MONTH, fc.Date),
	DATEPART(YEAR, fc.Date)
order by
	fc.UserId,
	DATEPART(YEAR, fc.Date),
	DATEPART(MONTH, fc.Date)
go
	
-- Add ForecastMonthId column to Forecasts
alter table forecasts
add ForecastMonthId int not null default(0)
go

-- UPDATE FORECASTMONTHID
update forecasts
set
	ForecastMonthId = fm.ForecastMonthId
from
	Forecasts fc
	join ForecastMonths fm on fm.Month = DATEPART(MONTH, fc.Date) and fm.Year = DATEPART(YEAR, fc.Date) and fc.UserId = fm.UserId

-- ADD FORECAST MONTH RELATION
alter table forecasts
add constraint FK_ForecastMonth
foreign key (ForecastMonthId)
references ForecastMonths(ForecastMonthId)
go	
ALTER TABLE [dbo].[Forecasts] CHECK CONSTRAINT [FK_ForecastMonth]
GO	
	
-- CREATE INDEX' ON FORECASTMONTHS
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserMonthYear] ON [dbo].[ForecastMonths]
(
	[UserId] ASC,
	[Month] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO	
CREATE NONCLUSTERED INDEX [IX_User] ON [dbo].[ForecastMonths]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX [IX_MonthYearUser] ON [dbo].[ForecastMonths]
(
	[Month] ASC,
	[Year] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

-- Cleanup Forecasts table
alter table forecasts
drop constraint FK_Forecasts_CreatedBy_Users, FK_Forecasts_Users
go

alter table forecasts
drop constraint 
	DF__Forecasts__Creat__79FD19BE, 
	DF__Forecasts__Creat__7AF13DF7

drop index 
	IX_UserDate on forecasts,
	IX_Date on forecasts,
	IX_User on forecasts

alter table forecasts  
drop column CreatedById, CreatedDate, UserId
go

CREATE UNIQUE NONCLUSTERED INDEX [IX_ForecastMonthDate] ON [dbo].[Forecasts]
(
	[ForecastMonthId] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

-- STORED PROCEDURES
create procedure spUpdateForecastMonthUnlocked
( 
		@monthCrit int,
		@yearCrit int,
		@userIdCrit int,
		@unlockValue bit
)
as
begin	
	update ForecastMonths 
	set 
		UnLocked = @unlockValue
	where
		Month = @monthCrit
		and Year = @yearCrit 
		and UserId = @userIdCrit
end
go


create procedure spUpdateForecastPeriod 
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
	if (@forecasttypeid = 1)
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
go

-- DOMAINSETTINGS CLEANUP
delete DomainSettings
where Name = 'D60CustomerIdsIntList'
go