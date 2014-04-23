using System.Collections.Generic;
using Trex.ServiceContracts;

namespace Trex.Server.Core.Services
{
    public interface IUserManagementService
    {
        UserCreationResponse CreateUser(User user, bool sendEmail, string language, string customerId);
        void SendNewUserEmail(User user, string password, string language, string customerId);
        void SaveUser(User user);
        void DeleteUser(User user);
        User GetUserByUserName(string userName, int clientId);
        List<User> GetAllUsers();
        bool ResetPassword(User user,string language);
        void SendResetPasswordMail(User user, string password, string language);
    }
}
