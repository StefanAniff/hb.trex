using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;
using Trex.Server.Core.Resources;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.Server.Infrastructure.EmailComposers;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class UserManagementService : LogableBase,  IUserManagementService
    {
        private readonly IMembershipService _membershipService;
        private readonly ITrexContextProvider _trexContextProvider;
        private readonly IEmailService _emailService;
        private readonly IPermissionService _permissionService;
        private readonly IAppSettings _appSettings;

        public UserManagementService(IMembershipService membershipService, ITrexContextProvider trexContextProvider,
                                     IEmailService emailService, IPermissionService permissionService,
                                     IAppSettings appSettings)
        {
            _membershipService = membershipService;
            _trexContextProvider = trexContextProvider;
            _emailService = emailService;
            _permissionService = permissionService;
            _appSettings = appSettings;
        }


        public UserCreationResponse CreateUser(User user, bool sendEmail, string language, string customerId)
        {
            var entityContext = _trexContextProvider.TrexEntityContext;

            var existingUser = entityContext.Users.Include("UsersCustomers").SingleOrDefault(u => u.UserName == user.UserName);
            if (existingUser != null)
                return new UserCreationResponse(UserManagementResource.UserAlreadyExistText, false, user);

            var password = _membershipService.GeneratePassword();
            var userCreationResponse = _membershipService.CreateMember(user, password);

            if (userCreationResponse.Success)
            {
                SaveUser(user);
                if (sendEmail)
                    SendNewUserEmail(user, password, language, customerId);
            }

            return userCreationResponse;
        }

        public void SendNewUserEmail(User user, string password, string language, string customerId)
        {
            var emailComposer = new NewUserEmailComposer(user, password, language, customerId, _permissionService, _appSettings);
            emailComposer.Recipients.Add(user.Email);
            emailComposer.Sender = _appSettings.TrexSupportEmail;

            _emailService.SendEmail(emailComposer);
        }

        public void SaveUser(User user)
        {
            try
            {
                var entityContext = _trexContextProvider.TrexEntityContext;

                _membershipService.UpdateUserRoles(user, user.Roles);

                if (user.Inactive)
                {
                    _membershipService.DeactivateUser(user);
                    user.Inactive = true;
                }
                else
                {
                    _membershipService.ActivateUser(user);
                    user.Inactive = false;

                }
                entityContext.Users.ApplyChanges(user);
                entityContext.SaveChanges();

            }
            catch (Exception ex)
            {
                LogError(ex);
                throw ex;
            }

        }

        public void DeleteUser(User user)
        {
            if (_membershipService.DeleteUser(user))
            {
                var context = _trexContextProvider.TrexEntityContext;

                context.Users.Attach(user);
                context.Users.DeleteObject(user);
                context.SaveChanges();
            }
        }

        public User GetUserByUserName(string userName, int clientId)
        {
            using (var entityContext = ObjectFactory.GetInstance<ITrexContextProvider>().TrexEntityContext)
            {
                var user = entityContext.Users.Include("UsersCustomers").Where(u => u.UserName == userName).ToList().FirstOrDefault();

                user.Roles = _membershipService.GetRolesForUser(user.UserName);

                var userPermissions = _permissionService.GetPermissionsForRoles(user.Roles, clientId);
                user.Permissions = userPermissions.Select(u => u.Permission).ToList();

                return user;
            }
        }

        public List<User> GetAllUsers()
        {
            var entityContext = _trexContextProvider.TrexEntityContext;
            var users = entityContext.Users.Include("UsersCustomers").ToList();

            foreach (var user in users)
            {
                user.Roles = _membershipService.GetRolesForUser(user.UserName);
            }
            return users;
        }

        public bool ResetPassword(User user, string language)
        {
            var newPassword = _membershipService.ResetPassword(user);
            if (newPassword != null)
            {
                SendResetPasswordMail(user, newPassword, language);
                return true;
            }

            return false;
        }

        public void SendResetPasswordMail(User user, string password, string language)
        {
            var mailComposer = new ForgotPasswordEmailComposer(user.Name, password, language, _appSettings);

            mailComposer.Recipients.Add(user.Email);
            mailComposer.Sender = _appSettings.TrexSupportEmail;
            _emailService.SendEmail(mailComposer);
        }
    }
}
