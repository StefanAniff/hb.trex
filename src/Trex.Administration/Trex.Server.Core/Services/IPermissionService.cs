using System.Collections.Generic;

namespace Trex.Server.Core.Services
{
    public interface IPermissionService
    {
        List<string> GetPermissionsForRoles(List<string> roles, string permissionFilePath);
        List<string> GetRoles(string permissionFilePath);
    }
}