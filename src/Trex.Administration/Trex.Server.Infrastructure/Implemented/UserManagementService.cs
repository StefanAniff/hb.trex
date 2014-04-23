using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Trex.Server.Core.Model;
using Trex.Server.Core.Resources;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IPermissionService _permissionService;
        private readonly IUserRepository _userRepository;

        public UserManagementService(IUserRepository userRepository, IPermissionService permissionService)
        {
            _userRepository = userRepository;
            _permissionService = permissionService;
        }

        #region IUserManagementService Members

        public UserCreationResponse CreateUser(User user, string password, string passwordQuestion, string passwordAnswer)
        {
            if (Membership.GetUser(user.UserName) != null)
            {
                return new UserCreationResponse(UserManagementResource.UserAlreadyExistText, false, user);
            }

            if (_userRepository.UserExists(user.UserName))
            {
                return new UserCreationResponse(UserManagementResource.UserAlreadyExistText, false, user);
            }

            var memberShipCreateStatus = MembershipCreateStatus.Success;

            var membershipUser = Membership.CreateUser(user.UserName, password, user.Email,
                                                       passwordQuestion, passwordAnswer, true,
                                                       out memberShipCreateStatus);
            switch (memberShipCreateStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return new UserCreationResponse(UserManagementResource.UserAlreadyExistText, false, user);
                case MembershipCreateStatus.DuplicateEmail:
                    return new UserCreationResponse(UserManagementResource.EmailAlreadyExistsText, false, user);
                case MembershipCreateStatus.InvalidPassword:
                    return new UserCreationResponse(UserManagementResource.InvalidPasswordText, false, user);
                case MembershipCreateStatus.Success:
                    {
                        var newUser = _userRepository.Create(user.UserName, user.Name, user.Email, user.Price);
                        return new UserCreationResponse(UserManagementResource.CreationSuccess, true, newUser);
                    }
                default:
                    return new UserCreationResponse(UserManagementResource.UnspecifiedError, false, user);
            }

            return null;
        }

        public void DeleteUser(User user)
        {
            if (Membership.DeleteUser(user.UserName))
            {
                _userRepository.Delete(user);
            }
        }

        public void DeactivateUser(User user)
        {
            var memberShipUser = Membership.GetUser(user.UserName);
            if (memberShipUser != null)
            {
                memberShipUser.IsApproved = false;
                Membership.UpdateUser(memberShipUser);
            }

            user.Inactive = true;
            _userRepository.Save(user);
        }

        public void ActivateUser(User user)
        {
            var memberShipUser = Membership.GetUser(user.UserName);
            if (memberShipUser != null)
            {
                memberShipUser.IsApproved = true;
                Membership.UpdateUser(memberShipUser);
            }
            user.Inactive = false;
            _userRepository.Save(user);
        }

        public bool ChangePassword(User user, string oldPassword, string newPassword)
        {
            var memberShipUser = Membership.GetUser(user.UserName);
            if (memberShipUser != null)
            {
                return memberShipUser.ChangePassword(oldPassword, newPassword);
            }
            return false;
        }

        public string ResetPassword(User user)
        {
            var memberShipUser = Membership.GetUser(user.UserName);
            if (memberShipUser != null)
            {
                return memberShipUser.ResetPassword();
                // return "hejsa";
            }
            return null;
        }

        public List<string> GetRoles()
        {
            var roles = Roles.GetAllRoles();
            return roles.ToList();
        }

        public List<string> GetRolesForUser(string userName)
        {
            var roles = Roles.GetRolesForUser(userName);
            return roles.ToList();
        }

        public void UpdateUserRoles(User user, List<string> roles)
        {
            var currentRoles = Roles.GetRolesForUser(user.UserName);
            if (currentRoles != null && currentRoles.Length > 0)
            {
                Roles.RemoveUserFromRoles(user.UserName, currentRoles);
            }
            Roles.AddUserToRoles(user.UserName, roles.ToArray());
        }

        #endregion

        public void AddUserToRole(User user, string role)
        {
            Roles.AddUserToRole(user.UserName, role);
        }
    }
}
    ;