using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Dialogs;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Infrastructure.Implemented;

namespace Trex.SmartClient
{
    public class LoginDialogManager : DialogManagerBase, IDialogManager
    {
        private readonly IUserSession _userSession;
        private readonly IAppSettings _appSettings;
        private readonly IMenuRegistry _menuRegistry;
        private readonly IBusyService _busyService;
        private readonly IUserService _userService;
        private readonly IConnectivityService _connectivityService;
        private LoginDialog _loginDialog;

        public LoginDialogManager(IRegionManager regionManager, IAppSettings appSettings, IUserSession userSession, IMenuRegistry menuRegistry, IBusyService busyService, IUserService userService, IConnectivityService connectivityService)
            : base(regionManager, appSettings)
        {

            _userSession = userSession;
            _appSettings = appSettings;
            _menuRegistry = menuRegistry;
            _busyService = busyService;
            _userService = userService;
            _connectivityService = connectivityService;

            ApplicationCommands.LoginFailed.RegisterCommand(new DelegateCommand<string>(LoginFailed));
            ApplicationCommands.LoginSucceeded.RegisterCommand(new DelegateCommand<object>(LoginSucceeded));
            ApplicationCommands.UserLoggedOut.RegisterCommand(new DelegateCommand<object>(LogOutUser));

        }

        private void LogOutUser(object obj)
        {
            LoginStart(null);
        }

        private void LoginSucceeded(object obj)
        {
            _busyService.HideBusy("Login");
          
            ApplicationCommands.ChangeScreenCommand.Execute(_menuRegistry.GetStartPage);
        }

        private void LoginFailed(string obj)
        {
            _busyService.HideBusy("Login");
            LoginStart(null);
        }


        private void LoginStart(object obj)
        {

            if(_loginDialog !=null && _loginDialog.IsVisible)
                return;

            _loginDialog = new LoginDialog();
            _loginDialog.Owner = Application.Current.MainWindow;
            var loginViewModel = new LoginViewModel(_userSession, _appSettings,_userService, _connectivityService);
            _loginDialog.ApplyViewModel(loginViewModel);
            _loginDialog.Show();
            //loginDialog.Top = loginDialog.Top + loginDialog.Height /2;
           // _loginDialog.Left = _loginDialog.Left + _loginDialog.Width / 2;

        }

      




    }
}