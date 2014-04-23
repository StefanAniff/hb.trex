﻿
CREATE FUNCTION [dbo].[ConvertToSmallDate]
(
	@DateTime DATETIME
)
RETURNS DATETIME
AS
BEGIN
	
	 RETURN DATEADD(dd, 0, DATEDIFF(dd, 0, @DateTime)) 
END
	
