


CREATE PROCEDURE [dbo].[spCreateRole]
@role nvarchar(100)
AS
insert into Roles values(@role)


