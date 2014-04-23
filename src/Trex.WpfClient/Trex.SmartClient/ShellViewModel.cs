using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Infrastructure.Implemented;

namespace Trex.SmartClient
{
    public class ShellViewModel : ViewModelBase, IShellViewModel
    {
        private readonly IBusyService _busyService;
        private readonly IUserSession _userSession;
        private readonly IUserService _userService;
        private readonly IApplicationStateService _applicationStateService;
        private readonly IUnityContainer _unityContainer;

        private DelegateCommand<object> ToggleState;
        private DelegateCommand<object> BootCompletedCOmmand;
        private DelegateCommand<object> OpenPasswordCommand;

        public DelegateCommand<object> StartActiveTask { get; set; }
        public DelegateCommand<object> PauseActiveTask { get; set; }

        public bool IsRunning
        {
            get { return _applicationStateService.CurrentState.ActiveTaskState == TimerState.Running; }
        
        }

        public ShellViewModel(IBusyService busyService, IUserSession userSession, IUserService userService, IApplicationStateService applicationStateService)
        {
            _busyService = busyService;
            _userSession = userSession;
            _userService = userService;
            _applicationStateService = applicationStateService;
            ToggleState = new DelegateCommand<object>(UpdateButtons);
            BootCompletedCOmmand = new DelegateCommand<object>(BootCompleted);
            OpenPasswordCommand = new DelegateCommand<object>(OpenChangePasswordDialog);
            StartActiveTask = new DelegateCommand<object>(ToggleExecute, o => !IsRunning);
            PauseActiveTask = new DelegateCommand<object>(ToggleExecute, o => IsRunning);

            ApplicationCommands.BootCompleted.RegisterCommand(BootCompletedCOmmand);
            ApplicationCommands.ChangePasswordDialogOpen.RegisterCommand(OpenPasswordCommand);

            _applicationStateService.CurrentState.PropertyChanged += _applicationState_PropertyChanged;

            UpdateButtons(null);
        }

        private void ToggleExecute(object obj)
        {
            TaskCommands.ToggleActiveTask.Execute(null);
        }

        private void _applicationState_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateButtons(null);
        }

        private void UpdateButtons(object obj)
        {
            StartActiveTask.RaiseCanExecuteChanged();
            PauseActiveTask.RaiseCanExecuteChanged();
            OnPropertyChanged(() => IsRunning);
        }

        private void OpenChangePasswordDialog(object obj)
        {
            var changePasswordWindow = new ChangePasswordView.ChangePasswordView();
            changePasswordWindow.ApplyViewModel(new ChangePasswordView.ChangePasswordViewModel(_userService, _userSession));
            changePasswordWindow.Owner = Application.Current.MainWindow;
            changePasswordWindow.ShowDialog();
        }

        private void BootCompleted(object obj)
        {
            _userSession.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
