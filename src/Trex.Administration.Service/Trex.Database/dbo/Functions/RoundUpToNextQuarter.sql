
CREATE FUNCTION [dbo].[RoundUpToNextQuarter]
(
	@float float
	
)
RETURNS FLOAT
AS
BEGIN
	
DECLARE @returnFloat float

SELECT @returnFloat =CEILING(@float*4)/4
RETURN @ReturnFloat
	 
END


