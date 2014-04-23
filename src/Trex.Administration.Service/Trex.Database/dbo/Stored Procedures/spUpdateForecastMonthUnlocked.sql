
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