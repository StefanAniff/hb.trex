using System;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using ServiceStack.Common.ServiceClient.Web;
using ServiceStack.ServiceClient.Web;
using Trex.Common.ServiceStack;
using Trex.SmartClient.Core.Exceptions;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Service.Helpers;
using Trex.SmartClient.Service.TrexPortalService;
using UserStatistics = Trex.Common.ServiceStack.UserStatistics;

namespace Trex.SmartClient.Service
{
    public class UserService : IUserService
    {
        private readonly IServiceStackClient _serviceStackClient;
        private readonly ServiceFactory _serviceFactory;

        public UserService(IServiceStackClient serviceStackClient, ServiceFactory serviceFactory)
        {
            _serviceStackClient = serviceStackClient;
            _serviceFactory = serviceFactory;
        }

        public Core.Model.User GetUserByUserNameAndPassword(string userName, string password, IUserSession userSession)
        {
            var client = _serviceFactory.GetServiceClient(userSession.LoginSettings);

            try
            {
                var user = client.GetUser(userName, password);

                if (user != null)
                {
                    return Core.Model.User.Create(user.UserName, user.FullName, user.UserId, user.Roles.ToList(), user.Permissions.ToList());
                }
                else
                {
                    throw new UserNotFoundException("User not found: " + userName);
                }
            }
            catch (CommunicationException ex)
            {
                throw new ServiceAccessException("Error while accessing service: GetUser", ex);
            }
            finally
            {
                client.Close();
            }
        }

        public Core.Model.UserStatistics GetPerformanceInfo(IUserSession userSession)
        {
            if (userSession.CurrentUser == null)
            {
                return null;
            }

            try
            {
                var info = _serviceStackClient.Get(new GetUserStatisticsRequest()
                {
                    NumberOfDaysBack = userSession.UserPreferences.StatisticsNumOfDaysBack,
                    UserId = userSession.CurrentUser.Id
                }).UserStatistics;

                var returnInfo = new Core.Model.UserStatistics
                                     {
                                         RegisteredHoursThisWeek = info.RegisteredHoursThisWeek,
                                         RegisteredHoursToday = info.RegisteredHoursToday,
                                         RegisteredHoursThisMonth = info.RegisteredHoursThisMonth,
                                         EarningsThisMonth = info.EarningsThisMonth,
                                         EarningsThisWeek = info.EarningsThisWeek,
                                         EarningsToday = info.EarningsToday,
                                         BillableHoursThisMonth = info.BillableHoursThisMonth,
                                         BillableHoursThisWeek = info.BillableHoursThisWeek,
                                         BillableHoursToday = info.BillableHoursToday,
                                     };
                return returnInfo;
            }
            catch (CommunicationException ex)
            {
                throw new ServiceAccessException("Error while accessing service: GetPerformanceInfo", ex);
            }
          
        }

        public DateTime GetTimeEntryMinStartDate(IUserSession userSession)
        {
            if (userSession.CurrentUser == null)
            {
                return default(DateTime);
            }

            try
            {
                return _serviceStackClient.Get(new GetGeneralSettingsRequest()
                {
                    UserId = userSession.CurrentUser.Id
                }).TimeEntryMinStartDate;
            }
            catch (CommunicationException ex)
            {
                throw new ServiceAccessException("Error while accessing service: GetPerformanceInfo", ex);
            }
        }

        public async Task<bool> ValidateUser(ILoginSettings loginSettings)
        {
             _serviceStackClient.Authorize(loginSettings);
             using (var client = _serviceFactory.GetAuthenticationClient(loginSettings))
            {
                return await client.ValidateUserAsync(loginSettings.UserName, loginSettings.Password, loginSettings.CustomerId);
            }
        }


        public bool ChangePassword(IUserSession userSession, string oldPassword, string newPassword)
        {
            var success = false;

            var client = _serviceFactory.GetServiceClient(userSession.LoginSettings);
            try
            {
                success = client.ChangePassword(userSession.CurrentUser.UserName, oldPassword, newPassword);
            }
            finally
            {
                if (success)
                {
                    userSession.LoginSettings.Password = newPassword;
                }

                client.Close();
            }

            return success;
        }


        public bool ResetPassword(ILoginSettings loginSettings)
        {
            var client = _serviceFactory.GetAuthenticationClient(loginSettings);
            try
            {
                return client.ResetPassword(loginSettings.UserName);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
