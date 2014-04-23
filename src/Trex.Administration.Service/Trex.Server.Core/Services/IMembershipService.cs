using System.Collections.Generic;

using Trex.ServiceContracts;

namespace Trex.Server.Core.Services
{
    public interface IMembershipService
    {

        string GeneratePassword();
        UserCreationResponse CreateMember(User user,string password);
        bool DeleteUser(User user);
        void DeactivateUser(User user);
        void ActivateUser(User user);
        bool ChangePassword(User user, string oldPassword, string newPassword);
        string ResetPassword(User user);
        List<string> GetRoles();
        List<string> GetRolesForUser(string userName);
        void UpdateUserRoles(User user, List<string> roles);
        string ApplicationName { get; set; }

        void AddUserToRole(User user, string role);
        void CreateRole(string roleName);
        void DeleteRole(string roleName);
    }
}
