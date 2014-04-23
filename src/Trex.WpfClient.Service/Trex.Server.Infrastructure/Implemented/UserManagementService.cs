using System.Collections.Generic;
using System.Linq;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IMembershipProvider _membershipProvider;

        public UserManagementService(IMembershipProvider membershipProvider)
        {
            _membershipProvider = membershipProvider;
        }

        public bool ChangePassword(User user, string oldPassword, string newPassword)
        {
            return _membershipProvider.ChangePassword(user.UserName, oldPassword, newPassword);
        }

        public string ResetPassword(User user)
        {
            return _membershipProvider.ResetPassword(user.UserName);
        }

        public List<string> GetRoles()
        {
            var roles = _membershipProvider.GetAllRoles();
            return roles.ToList();
        }

        public List<string> GetRolesForUser(string userName)
        {
            var roles = _membershipProvider.GetRolesForUser(userName);
            return roles.ToList();
        }

        public void UpdateUserRoles(User user, List<string> roles)
        {
            var currentRoles = _membershipProvider.GetRolesForUser(user.UserName);

            if (currentRoles.Contains("Administrator") && _membershipProvider.GetUsersInRole("Administrator").Count() <= 1)
                return;

            if (currentRoles != null && currentRoles.Length > 0)
            {
                _membershipProvider.RemoveUserFromRoles(user.UserName, currentRoles);
            }
            _membershipProvider.AddUserToRoles(user.UserName, roles.ToArray());
        }
    }
}