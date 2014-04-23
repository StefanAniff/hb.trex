using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using ServiceStack.ServiceClient.Web;
using Trex.SmartClient.Core.Exceptions;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Core.Utils;
using Trex.SmartClient.Infrastructure.Commands;
using log4net;
using Task = System.Threading.Tasks.Task;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class UserSession : IUserSession
    {
        private readonly IUserPreferences _userPreferences;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IUserService _userService;
        private readonly IConnectivityService _connectivityService;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private bool _success;
        private string _statusMessage;
        public UserStatistics UserStatistics { get; set; }
        public ILoginSettings LoginSettings { get; private set; }

        public UserSession(IUserService userService, IUserPreferences userPreferences, IUserSettingsService userSettingsService,
                           IConnectivityService connectivityService)
        {
            _userService = userService;
            _userSettingsService = userSettingsService;
            _userPreferences = userPreferences;
            _connectivityService = connectivityService;
            UserStatistics = new UserStatistics();
        }


        public User CurrentUser
        {
            get { return UserContext.Instance.User; }
            private set { UserContext.Instance.User = value; }
        }

        public bool IsLoggedIn
        {
            get { return CurrentUser != null; }
        }


        public void Initialize()
        {
            LoginSettings = GetPersistedLogin();
            if (LoginSettings != null)
            {
                CurrentUser = User.Create(LoginSettings.UserName, LoginSettings.UserFullName, LoginSettings.UserId, new List<string>(), new List<string>());
                LoginUser(LoginSettings.UserName, LoginSettings.Password, LoginSettings.CustomerId, LoginSettings.PersistLogin);
            }
            else
            {
                ApplicationCommands.LoginFailed.Execute(null);
            }
        }

        public void AttachUserNameAndCustomerId(string username, string customerId)
        {
            LoginSettings = new LoginSettings
                {
                    CreateDate = DateTime.Now,
                    UserName = username,
                    CustomerId = customerId,
                };
        }

        public async void LoginUser(string username, string password, string customerId, bool persistPassword)
        {
            LoginSettings = new LoginSettings
                {
                    CreateDate = DateTime.Now,
                    UserName = username,
                    Password = password,
                    CustomerId = customerId,
                    PersistLogin = persistPassword
                };


            await DoLogin();

            Execute.InUIThread(() =>
                {
                    if (_success)
                    {
                        ApplicationCommands.LoginSucceeded.Execute(null);
                    }
                    else
                    {
                        ApplicationCommands.LoginFailed.Execute(_statusMessage);
                    } 
                });            
        }


        private async Task DoLogin()
        {            
            try
            {
                if (!_connectivityService.IsOnline)
                {
                    _success = true;
                    return;
                }
                var validateUser = await _userService.ValidateUser(LoginSettings);
                if (validateUser)
                {
                    CurrentUser = _userService.GetUserByUserNameAndPassword(LoginSettings.UserName,
                                                                            LoginSettings.Password, this);
                    LoginSettings.UserId = CurrentUser.Id;
                    LoginSettings.UserFullName = CurrentUser.Name;


                    _userSettingsService.SaveSettings(LoginSettings);
                    _success = true;
                }
                else
                {
                    _success = false;
                    _statusMessage = "Username or password is incorrect. Please check your credentials and try again.";
                }
            }
            catch (ServiceAccessException)
            {
                _success = false;
                _statusMessage =
                    "Login failed due to a connectivity error. Please check that the service url is correct.";
            }
            catch (WebServiceException ex)
            {
                Logger.Error(ex);
                _success = false;
                _statusMessage = ex.ErrorMessage;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                _success = false;
                _statusMessage = ex.Message;
            }
        }

        public void LogOutUser()
        {
            CurrentUser = null;
            _userSettingsService.DeleteSettings();
        }

        public IUserPreferences UserPreferences
        {
            get { return _userPreferences; }
        }

        private bool HasPersistedLogin()
        {
            var loginSettings = _userSettingsService.GetSettings();
            return loginSettings != null;
        }

        private ILoginSettings GetPersistedLogin()
        {
            return HasPersistedLogin() ? _userSettingsService.GetSettings() : null;
        }

        #region Permissions

        public bool MayEditOthersWorksplan
        {
            get
            {
                var user = CurrentUser;
                return user != null && user.HasPermission(Permissions.EditOthersWorkplanPermission);
            }
        }

        #endregion
    }
}