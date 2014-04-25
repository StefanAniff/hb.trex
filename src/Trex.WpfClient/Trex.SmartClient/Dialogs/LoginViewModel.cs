using System;
using System.Threading;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Resources;

namespace Trex.SmartClient.Dialogs
{
    public class LoginViewModel : ViewModelBase
    {
        private string _userName;
        private string _customerId;
        private string _statusMessage;
        private string _resetPasswordStatusMessage;
        private readonly IUserSession _userSession;
        private readonly IUserService _userService;
        private readonly IConnectivityService _connectivityService;

        public DelegateCommand<object> LoginCommand { get; set; }
        public DelegateCommand<object> ForgotPasswordCommand { get; set; }

        public LoginViewModel(IUserSession userSession, IAppSettings appSettings, IUserService userService, IConnectivityService connectivityService)
        {
            ForgotPasswordCommand = new DelegateCommand<object>(ExecuteForgotPassword, CanExecuteForgotPassword);
            LoginCommand = new DelegateCommand<object>(ExecuteLogin, CanExecuteLogin);
            _userSession = userSession;
            _userService = userService;
            _connectivityService = connectivityService;

            UserName = appSettings.UserName;
            CustomerId = appSettings.CustomerId;
            CustomerId = "hb";
            ApplicationCommands.LoginFailed.RegisterCommand(new DelegateCommand<string>(UserLoginFailed));
            ApplicationCommands.ConnectivityChanged.RegisterCommand(new DelegateCommand<object>(ConnectivityChanged));
        }

        private void ExecuteLogin(object obj)
        {
            var password = ExtractPassword(obj);
            StatusMessage = LoginResources.LoginProgress;

            DoLogin(UserName, password, CustomerId);
        }

        private void DoLogin(string userName, string password, string customerId)
        {
            // Do in background. GUI is locking eventhough UserSession.LoginUser is async!?
            new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    _userSession.LoginUser(userName, password, customerId, true);
                }).Start();
        }

        /// <summary>
        /// Hack. PasswordBox.Password can't be bound. Therefor the manual extraction.
        /// Better placed here than code-behind, so commandbinding can be used
        /// </summary>
        /// <param name="shouldBePasswordBox"></param>
        /// <returns></returns>
        private static string ExtractPassword(object shouldBePasswordBox)
        {
            var passwordBox = shouldBePasswordBox as PasswordBox;
            if (passwordBox == null)
                throw new ApplicationException("Was expecting a PasswordBox object as parameter! Have you set CommandParameter binding?");

            return passwordBox.Password;
        }

        private void ConnectivityChanged(object obj)
        {
            LoginCommand.RaiseCanExecuteChanged();
            ForgotPasswordCommand.RaiseCanExecuteChanged();
        }

        private bool CanExecuteLogin(object arg)
        {
            return _connectivityService.IsOnline;
        }

        private bool CanExecuteForgotPassword(object arg)
        {
            return !string.IsNullOrWhiteSpace(UserName) && _connectivityService.IsOnline;
        }

        private void ExecuteForgotPassword(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(CustomerId))
                {
                    ResetPasswordStatusMessage = "Please fill out customer id!";
                    return;
                }
                if (string.IsNullOrEmpty(UserName))
                {
                    ResetPasswordStatusMessage = "Please fill out username!";
                    return;
                }
                _userSession.AttachUserNameAndCustomerId(UserName, CustomerId);
                if (_userService.ResetPassword(_userSession.LoginSettings))
                {
                    ResetPasswordStatusMessage =
                        "Password successfully reset. An email has been sent to your e-mail address with the new password";
                }
                else
                {
                    ResetPasswordStatusMessage =
                        "Password could not be reset. Please check that your username is spelled correctly";
                }
            }
            catch (Exception ex)
            {
                ResetPasswordStatusMessage = ex.Message;
            }
        }

        private void UserLoginFailed(string obj)
        {
            StatusMessage = obj;
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
                ForgotPasswordCommand.RaiseCanExecuteChanged();
            }
        }        

        public string CustomerId
        {
            get { return _customerId; }
            set
            {
                _customerId = value;
                OnPropertyChanged("CustomerId");
            }
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged("StatusMessage");
            }
        }

        public string ResetPasswordStatusMessage
        {
            get { return _resetPasswordStatusMessage; }
            set
            {
                _resetPasswordStatusMessage = value;
                OnPropertyChanged("ResetPasswordStatusMessage");
                ForgotPasswordCommand.RaiseCanExecuteChanged();
            }
        }
    }
}