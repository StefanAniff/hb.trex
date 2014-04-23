using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IUserManagementService
    {
        bool ChangePassword(User user, string oldPassword, string newPassword);
        string ResetPassword(User user);
        List<string> GetRoles();
        List<string> GetRolesForUser(string userName);
        void UpdateUserRoles(User user, List<string> roles);
    }
}
