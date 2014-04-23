using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using StructureMap;
using Trex.Server.Core.Resources;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.EmailComposers;
using Trex.ServiceContracts;
using Roles = System.Web.Security.Roles;

namespace Trex.Server.Infrastructure.Implemented
{
    public class MembershipService : IMembershipService
    {

        public string GeneratePassword()
        {
            return Membership.GeneratePassword(6,0);
        }


        

        public UserCreationResponse CreateMember(User user, string password)
        {
            if (Membership.GetUser(user.UserName) != null)
                return new UserCreationResponse(UserManagementResource.UserAlreadyExistText, false, user);

            var memberShipCreateStatus = MembershipCreateStatus.Success;


            Membership.CreateUser(user.UserName, password, user.Email,
                                                                  "nothing", "nothing", true,
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
                        UpdateUserRoles(user, user.Roles);
                        return new UserCreationResponse(UserManagementResource.CreationSuccess, true, user);
                    }

                default:
                    return new UserCreationResponse(UserManagementResource.UnspecifiedError, false, user);
            }

        }

        public bool DeleteUser(User user)
        {
            return Membership.DeleteUser(user.UserName);
        }

        public void DeactivateUser(User user)
        {
            var memberShipUser = Membership.GetUser(user.UserName);
            if (memberShipUser != null)
            {
                if (memberShipUser.IsApproved)
                {
                    memberShipUser.IsApproved = false;
                    Membership.UpdateUser(memberShipUser);
                }

            }
        }

        public void ActivateUser(User user)
        {

            var memberShipUser = Membership.GetUser(user.UserName);
            if (memberShipUser != null)
            {
                if (!memberShipUser.IsApproved)
                {
                    memberShipUser.IsApproved = true;
                    Membership.UpdateUser(memberShipUser);
                }

            }
            
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

        public string ApplicationName
        {
            get { return Membership.ApplicationName; }
            set
            {
                Membership.ApplicationName = value;
                Roles.ApplicationName = value;
            }

        }

        public void CreateRole(string roleName)
        {
            Roles.CreateRole(roleName);
        }

        public void DeleteRole(string roleName)
        {
            Roles.DeleteRole(roleName, true);
        }

        public void AddUserToRole(User user, string role)
        {
            Roles.AddUserToRole(user.UserName, role);

        }

    }
}