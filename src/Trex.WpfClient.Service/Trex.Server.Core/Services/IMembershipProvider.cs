using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Core.Services
{
    public interface IMembershipProvider
    {
        bool ValidateUser(string userName, string userPassword);
        void AddUserToRoles(string userName, string[] roles);
        void RemoveUserFromRoles(string userName, string[] currentRoles);
        string[] GetRolesForUser(string userName);
        string ResetPassword(string username);
        bool ChangePassword(string username, string oldPassword, string newPassword);
        string[] GetAllRoles();
        string[] GetUsersInRole(string role);
        bool RoleExists(string name);
        void DeleteRole(string roleName);
        void CreateRole(string roleName);
        string GetApplicationName();
    }
}
