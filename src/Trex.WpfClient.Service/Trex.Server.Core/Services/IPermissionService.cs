using System;
using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IPermissionService
    {
        List<PermissionItem> GetAllPermissions(int clientClientApplicationId);
        void AddPermission(int permissionId, string roleName, string conn);
        void RemovePermission(int permissionId, string roleName, string conn);
        List<PermissionItem> GetPermissions(IEnumerable<string> roles, string conn, int clientApplicationType);
    }
}
