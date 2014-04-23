
CREATE PROCEDURE [dbo].[spGetPermissionsForRoles]
@RoleName nvarchar(100),
@ClientApplicationID int
AS
SELECT PermissionID, ps.Permission from PermissionsInRoles as pr
	join 
		Roles as r on r.ID = pr.RoleID
	join
		dbo.[Permissions] as ps on ps.ID = pr.PermissionID
	where r.Title= @RoleName and ps.ClientApplicationID = @ClientApplicationID 