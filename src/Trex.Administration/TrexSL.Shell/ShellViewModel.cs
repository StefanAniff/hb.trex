using System.Reflection;
using System.Text;
using System.Windows.Controls.Primitives;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Trex.Core.Interfaces;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.Infrastructure.Implemented;
using TrexSL.Shell.Dialog;
using TrexSL.Shell.Login;
using TrexSL.Shell.Menu.MenuView;
using DelegateCommand = Telerik.Windows.Controls.DelegateCommand;
using ViewModelBase = Trex.Core.Implemented.ViewModelBase;

namespace TrexSL.Shell
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly IUnityContainer _unityContainer;
        public DelegateCommand<object> VersionData { get; set; }
        private IMenuViewModel _menuViewModel;
        private Popup _p;

        public ShellViewModel(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
            MenuViewModel = unityContainer.Resolve<IMenuViewModel>();
            ApplicationCommands.GoToLogin.RegisterCommand(new DelegateCommand<IMenuInfo>(OpenLoginWindow));
            ApplicationCommands.SystemBusy.RegisterCommand(new DelegateCommand<string>(ExecuteSystemBusy));
            ApplicationCommands.SystemIdle.RegisterCommand(new DelegateCommand(ExecuteSystemIdle));
            VersionData = new DelegateCommand<object>(executeVersionData);
        }

        private void executeVersionData(object o)
        {
            //_p = new Popup
            //         {
            //             Child = new NewInVersion(),
            //             IsOpen = true
            //         };
        }

        private void ExecuteSystemIdle(object obj)
        {
            IsBusy = false;
        }

        private void ExecuteSystemBusy(string obj)
        {
            IsBusy = true;
            BusyText = obj;
        }

        public IMenuViewModel MenuViewModel
        {
            get { return _menuViewModel; }
            set
            {
                _menuViewModel = value;
                OnPropertyChanged("MenuViewModel");
            }
        }

        public string Version
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var name = new AssemblyName(assembly.FullName);

                return string.Format("v. {0}", name.Version.ToString(3));
            }
        }

        public string EnvironmentDetails
        {
            get
            {
                return new StringBuilder()
                    .AppendLine(string.Format("Environment: {0}", ServiceFactory.Environment))
                    .AppendLine()
                    .AppendLine(string.Format("Service URL: {0}", ServiceFactory.DomainChannelUrl))
                    .AppendLine(string.Format("Fileservice URL: {0}", ServiceFactory.FileDownloadChannelUrl))
                    .ToString();
            }
        }

        private string _busyText;
        public string BusyText
        {
            get { return _busyText; }
            set
            {
                _busyText = value;
                OnPropertyChanged("BusyText");
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public void OpenLoginWindow(IMenuInfo obj)
        {
            var userSession = _unityContainer.Resolve<IUserSession>();
            var loginView = new LoginView();
            var loginViewModel = new LoginViewModel(obj, userSession);
            loginView.ViewModel = (loginViewModel);
            loginView.Show();
        }
    }
}