using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.LoginStatusView
{
    public class LoginStatusViewModel : ViewModelBase, ILoginStatusViewModel
    {
        private readonly IUserSession _userSession;
        private readonly IConnectivityService _connectivityService;

        public DelegateCommand<object> LogOut { get; set; }
        public DelegateCommand<object> ChangePassword { get; set; }

        public LoginStatusViewModel(IUserSession userSession, IConnectivityService connectivityService)
        {
            _userSession = userSession;
            _connectivityService = connectivityService;
            LogOut = new DelegateCommand<object>(ExecuteLogOut);
            ChangePassword = new DelegateCommand<object>(ExecuteChangePassword, CanChangePassword);
            ApplicationCommands.LoginSucceeded.RegisterCommand(new DelegateCommand<object>(UserLoggedIn));
            ApplicationCommands.ConnectivityChanged.RegisterCommand(new DelegateCommand<object>(ConnectivityChanged));
            ApplicationCommands.ConnectivityUnstable.RegisterCommand(new DelegateCommand<object>(ConnectivityChanged));

        }

        private bool CanChangePassword(object arg)
        {
            return _connectivityService.IsOnline;
        }

        private void ExecuteChangePassword(object obj)
        {
            ApplicationCommands.ChangePasswordDialogOpen.Execute(null);
        }

        private void ConnectivityChanged(object obj)
        {
            ChangePassword.RaiseCanExecuteChanged();
            OnPropertyChanged(() => UserName);
        }

        private void ExecuteLogOut(object obj)
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _userSession.LogOutUser();
                OnPropertyChanged(() => IsVisible);
                ApplicationCommands.UserLoggedOut.Execute(null);
            }
        }

        private void UserLoggedIn(object obj)
        {
            OnPropertyChanged(() => IsVisible);
            OnPropertyChanged(() => UserName);
            OnPropertyChanged(() => ButtonText);
        }

        public string UserName
        {
            get
            {
                var userName = string.Empty;
                if (_userSession.CurrentUser != null)
                {
                    if (_userSession.IsLoggedIn)
                    {
                        userName = _userSession.CurrentUser.Name + " ";
                    }
                    else
                    {
                        userName = string.Empty;
                    }
                }

                if (!_connectivityService.IsOnline)
                {
                    userName += "(Offline)";
                }
                else if (_connectivityService.IsUnstable)
                {
                    userName += "(Unstable)";
                }
                return userName;
            }
        }

        public string ButtonText
        {
            get { return _userSession.IsLoggedIn ? "Log out" : "Log in"; }
        }

        public bool IsVisible
        {
            get { return _userSession.IsLoggedIn; }
        }
    }
}