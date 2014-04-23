#region

using System;
using System.ServiceModel;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.ServiceContracts;

#endregion

namespace Trex.Infrastructure.Implemented
{
    public class UserSession : IUserSession
    {
        private readonly IAppSettings _appSettings;
        private readonly IDataService _dataService;
        private readonly IUserSettingsService _userSettingsService;
        private bool _isInitialized;
        private ILoginSettings _loginSettings;

        public UserSession(IUserSettingsService userSettingsService, IDataService dataService, IAppSettings appSettings)
        {
            _userSettingsService = userSettingsService;
            _loginSettings = _userSettingsService.GetSettings();
            _dataService = dataService;
            _appSettings = appSettings;
        }

        #region IUserSession Members

        public IUserSettingsService UserSettingsService
        {
            get { return _userSettingsService; }
        }

        public string CustomerId { get; private set; }

        public bool IsLoggedIn
        {
            get { return CurrentUser != null; }
        }

        public void Initialize(Action<LoginResponse> callbackAction)
        {
            if (_loginSettings != null)
            {
                LoginUser(_loginSettings, callbackAction);
            }
            else
            {
                callbackAction(new LoginResponse {Response = "No saved login credentials"});
            }
        }

        public User CurrentUser
        {
            get { return UserContext.Instance.User; }
            set { UserContext.Instance.User = value; }
        }

        public void LoginUser(string username, string password, string customerId, bool persistLogin,
                              Action<LoginResponse> callBackAction)
        {
            _loginSettings = new LoginSettings
                                 {
                                     CreateDate = DateTime.Now,
                                     UserName = username,
                                     Password = password,
                                     CustomerId = customerId,
                                     PersistLogin = persistLogin
                                 };

            var authenticationService = new AuthenticationService(_loginSettings);
            var result = authenticationService.AuthenticateUser(username, password, customerId);
            result.Subscribe(isValid =>
                                 {
                                     if (isValid)
                                     {
                                         LoginUser(_loginSettings, callBackAction);
                                     }
                                     else
                                     {
                                         callBackAction(new LoginResponse
                                                            {
                                                                Response = "Incorrect username or password",
                                                                Success = false
                                                            });
                                     }
                                 },
                             (e) =>
                                 {
                                     if (e is FaultException<ExceptionDetail>)
                                     {
                                         var detail = ((FaultException<ExceptionDetail>) e).Detail;
                                         //if (detail.Type.Contains("UserValidationException"))

                                         callBackAction(new LoginResponse {Response = detail.Message, Success = false});
                                     }
                                 });
        }

        public void LogOutUser()
        {
            CurrentUser = null;
            _userSettingsService.DeleteSettings();
        }

        #endregion

        public event EventHandler Initialized;

        private void LoginUser(ILoginSettings loginSettings, Action<LoginResponse> callbackAction)
        {
            _userSettingsService.SaveSettings(loginSettings);
            _dataService.Initialize(loginSettings);
            _dataService.GetUserByUserName(loginSettings.UserName).Subscribe(
                user =>
                    {
                        if (user.HasPermission(Permissions.ApplicationLoginPermission))
                        {
                            CurrentUser = user;
                            _userSettingsService.SaveSettings(_loginSettings);
                            CustomerId = _loginSettings.CustomerId;
                            callbackAction(new LoginResponse {Response = "Login succeedet", Success = true});
                        }
                        else
                        {
                            _userSettingsService.DeleteSettings();
                            callbackAction(new LoginResponse
                                               {
                                                   Response = "You do not have permission to access this application.",
                                                   Success = false
                                               });
                        }
                    },
                (e) => callbackAction(new LoginResponse {Response = e.ToString(), Success = false}));
        }

        //public void LoginAsHttpUser()
        //{
        //    var loginSettings = GetPersistedLogin();
        //    if (loginSettings != null)
        //    {
        //        _authenticationService.LoginAsHttpUser(loginSettings.UserName, loginSettings.Password, loginSettings.CustomerId);
        //    }
        //}

        //private void _authenticationService_UserAuthenticationCompleted(object sender, UserAuthenticationCompletedEventArgs e)
        //{
        //    if (e.Result)
        //    {
        //        _userSettingsService.SaveSettings(_loginSettings);
        //        _dataService.GetUserByUserNameCompleted += GetUserByUserNameCompleted;
        //        _dataService.GetUserByUserName(_loginSettings.UserName);
        //    }
        //    else
        //    {
        //        OnInitialized();
        //        if (UserLoginFailed != null)
        //        {
        //            UserLoginFailed(this, new LoginFailedEventArgs("Incorrect username or password"));
        //        }
        //    }
        //}

        //private void GetUserByUserNameCompleted(object sender, UserEventArgs e)
        //{
        //    if (!e.User.HasPermission(Permissions.ApplicationLoginPermission))
        //    {
        //        OnInitialized();
        //        if (UserLoginFailed != null)
        //        {
        //            UserLoginFailed(this, new LoginFailedEventArgs("You do not have permission to access this application."));
        //        }
        //        return;
        //    }

        //    CurrentUser = e.User;
        //    OnInitialized();

        //    ApplicationCommands.LoginSucceeded.Execute(null);

        //    //if (UserLoggedIn != null)
        //    //    UserLoggedIn(this, e);
        //}

        //private void OnInitialized()
        //{
        //    if (!_isInitialized)
        //    {
        //        _isInitialized = true;
        //        if (Initialized != null)
        //        {
        //            Initialized(this, null);
        //        }
        //    }
        //}

        private bool HasPersistedLogin()
        {
            var loginSettings = _userSettingsService.GetSettings();
            if (loginSettings == null)
            {
                return false;
            }

            if (!loginSettings.PersistLogin)
            {
                if ((DateTime.Now - loginSettings.CreateDate) > _appSettings.SessionTimeOut)
                {
                    return false;
                }
            }
            return true;
        }

        private ILoginSettings GetPersistedLogin()
        {
            if (HasPersistedLogin())
            {
                return _userSettingsService.GetSettings();
            }
            return null;
        }
    }
}