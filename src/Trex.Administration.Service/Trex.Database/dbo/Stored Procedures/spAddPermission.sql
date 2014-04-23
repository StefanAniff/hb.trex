


CREATE PROCEDURE [dbo].[spAddPermission]
@RoleName nvarchar(100),
@PermissionID int
AS
insert into PermissionsInRoles values((select ID from Roles where Title=@RoleName),@PermissionID)


