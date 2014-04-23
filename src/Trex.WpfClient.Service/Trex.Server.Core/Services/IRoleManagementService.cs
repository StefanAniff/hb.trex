
namespace Trex.Server.Core.Services
{
    public interface IRoleManagementService
    {
        void CreateRole(string roleName, string conn);
        void DeleteRole(string roleName, string conn);
    }
}
