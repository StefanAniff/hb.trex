
using System.Collections.Generic;
using Trex.ServiceContracts;

namespace Trex.Server.Core.Services
{
    public interface IRoleManagementService
    {
        void CreateRole(string roleName);
        void DeleteRole(string roleName);
        List<Role> GetAllRoles();
    }
}
