



CREATE PROCEDURE [dbo].[spDeleteRole]
@role nvarchar(100)
AS
delete from Roles where Title=@role


