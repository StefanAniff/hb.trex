using System;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.ChangePasswordView
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IUserSession _userSession;

        public DelegateCommand<object> ChangePasswordCommand { get; set; }

        public ChangePasswordViewModel(IUserService userService, IUserSession userSession)
        {
            _userService = userService;
            _userSession = userSession;
            ChangePasswordCommand = new DelegateCommand<object>(ExecuteChangePassword, CanExecuteChangePassword);
        }

        private bool CanExecuteChangePassword(object arg)
        {
            return !string.IsNullOrWhiteSpace(NewPassword1) && !string.IsNullOrWhiteSpace(OldPassword) && NewPassword1 == NewPassword2;
        }

        private void ExecuteChangePassword(object obj)
        {
            try
            {
                if (_userService.ChangePassword(_userSession, OldPassword, NewPassword1))
                {
                    ApplicationCommands.ChangePasswordSucceeded.Execute(null);
                }
                else
                {
                    StatusMessage = "Your password could not be changed. Please retype your password again.";
                }
            }
            catch (Exception e)
            {
                StatusMessage = e.Message;
            }

        }


        private string _newPassword1;
        public string NewPassword1
        {
            get { return _newPassword1; }
            set
            {
                _newPassword1 = value;
                OnPropertyChanged("NewPassword1");
                ChangePasswordCommand.RaiseCanExecuteChanged();
            }
        }

        private string _newPassword2;
        public string NewPassword2
        {
            get { return _newPassword2; }
            set
            {
                _newPassword2 = value;
                OnPropertyChanged("NewPassword2");
                ChangePasswordCommand.RaiseCanExecuteChanged();
            }
        }

        private string _oldPassword;
        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                _oldPassword = value;
                OnPropertyChanged("OldPassword");
                ChangePasswordCommand.RaiseCanExecuteChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged("StatusMessage");
                ChangePasswordCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
