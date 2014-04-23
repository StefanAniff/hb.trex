using System;
using System.Threading.Tasks;
using Trex.Common.ServiceStack;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{
    public interface IUserService
    {
        User GetUserByUserNameAndPassword(string userName, string password, IUserSession userSession);
        Core.Model.UserStatistics GetPerformanceInfo(IUserSession userSession);
        bool ChangePassword(IUserSession userSession, string oldPassword, string newPassword);
        bool ResetPassword(ILoginSettings loginSettings);
        Task<bool> ValidateUser(ILoginSettings loginSettings);
        DateTime GetTimeEntryMinStartDate(IUserSession userSession);
    }
}
