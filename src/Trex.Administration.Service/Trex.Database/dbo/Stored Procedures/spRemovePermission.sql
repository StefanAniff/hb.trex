


CREATE PROCEDURE [dbo].[spRemovePermission]
@RoleName nvarchar(100),
@PermissionID int
AS
delete from PermissionsInRoles where RoleID=(select ID from Roles where Title=@RoleName) and PermissionID = @PermissionID


