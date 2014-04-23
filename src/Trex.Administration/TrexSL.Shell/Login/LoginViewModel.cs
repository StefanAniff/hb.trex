using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using TrexSL.Shell.Resources;

namespace TrexSL.Shell.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IMenuInfo _returnPage;
        private readonly IUserSession _userSession;
        private string _customerId;
        private string _password;
        private bool _persistLogin;
        private string _statusMessage;
        private string _userName;

        public LoginViewModel(IMenuInfo returnPage, IUserSession userSession)
        {
            LoginCommand = new DelegateCommand<object>(ExecuteLogin);
            _userSession = userSession;
            _returnPage = returnPage;
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
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

        public bool PersistLogin
        {
            get { return _persistLogin; }
            set
            {
                _persistLogin = value;
                OnPropertyChanged("PersistLogin");
            }
        }

        public DelegateCommand<object> LoginCommand { get; set; }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged("StatusMessage");
            }
        }

        public string WindowTitle
        {
            get { return LoginResources.WindowTitle; }
        }

        private void ExecuteLogin(object obj)
        {
            StatusMessage = LoginResources.LoginProgress;
           
            _userSession.LoginUser(UserName, Password, CustomerId, PersistLogin,LoginCompleted);
        }

        private void LoginCompleted(LoginResponse obj)
        {
            if(obj.Success)
                ApplicationCommands.LoginSucceeded.Execute(null);
            else
            {
                StatusMessage = obj.Response;
            }
        }

        
    }
}