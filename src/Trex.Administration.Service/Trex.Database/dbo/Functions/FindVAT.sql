
CREATE FUNCTION [dbo].[FindVAT]
(
	@invoiceId int
)
RETURNS float
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result float;
	DECLARE @temp TABLE( exclVat float );


	INSERT INTO @temp
	SELECT
		CASE
			WHEN (il.PricePrUnit * il.Units) IS NULL
				THEN 0
				ELSE (
				CASE
					WHEN il.UnitType = 2
						THEN il.PricePrUnit
						ELSE il.PricePrUnit * il.Units
				END)
		END 
	FROM InvoiceLines il
	WHERE il.InvoiceID = @invoiceId

	SET @result = (SELECT SUM(t.exclVat)
	FROM @temp t)
	
	
	RETURN @result;

END
