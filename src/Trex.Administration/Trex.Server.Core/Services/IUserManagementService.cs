﻿using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IUserManagementService
    {
        UserCreationResponse CreateUser(User user, string password, string passwordQuestion, string passwordAnswer);
        void DeleteUser(User user);
        void DeactivateUser(User user);
        void ActivateUser(User user);
        bool ChangePassword(User user, string oldPassword, string newPassword);
        string ResetPassword(User user);
        List<string> GetRoles();
        List<string> GetRolesForUser(string userName);
        void UpdateUserRoles(User user, List<string> roles);
    }
}