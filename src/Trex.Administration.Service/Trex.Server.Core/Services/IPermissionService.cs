using System;
using System.Collections.Generic;
using Trex.Server.DataAccess;
using Trex.ServiceContracts;


namespace Trex.Server.Core.Services
{
    public interface IPermissionService
    {
        List<UserPermission> GetPermissionsForRoles(List<string> roles, int clientClientApplicationType);
        List<UserPermission> GetAllPermissions(int clientClientApplicationId);
        void AddPermission(int permissionId, string roleName);
        void RemovePermission(int permissionId, string roleName);
        void UpdatePermissionsForRole(List<UserPermission> permissions,Role role,int clientId );
    }
}
