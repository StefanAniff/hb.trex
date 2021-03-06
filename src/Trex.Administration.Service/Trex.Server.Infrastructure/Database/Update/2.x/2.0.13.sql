
/****** Object:  StoredProcedure [dbo].[spGetPermissionsForRoles]    Script Date: 02/01/2012 09:09:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[spGetPermissionsForRoles]
@RoleName nvarchar(100),
@ClientApplicationID int
AS
SELECT PermissionID, ps.Permission from PermissionsInRoles as pr
	join 
		Roles as r on r.ID = pr.RoleID
	join
		trex_base.dbo.Permissions as ps on ps.ID = pr.PermissionID
	where r.Title= @RoleName and ps.ClientApplicationID = @ClientApplicationID 