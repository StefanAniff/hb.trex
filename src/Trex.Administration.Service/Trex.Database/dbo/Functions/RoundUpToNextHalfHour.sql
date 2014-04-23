

CREATE FUNCTION [dbo].[RoundUpToNextHalfHour]
(
	@float float
	
)
RETURNS FLOAT
AS
BEGIN
	
DECLARE @returnFloat float

SELECT @returnFloat =CEILING(@float*2)/2
RETURN @ReturnFloat
	 
END



