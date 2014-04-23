using System;
using System.Collections.ObjectModel;
using System.Deployment.Application;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.MenuView
{
    public class MenuViewModel : ViewModelBase, IMenuViewModel
    {
        private readonly IMenuRegistry _menuRegistry;
        private readonly IUserSession _userSession;
        private readonly IAppSettings _appSettings;
        private readonly IConnectivityService _connectivityService;
        public IVersionViewModel VersionViewModel { get; private set; }
        public ILoginStatusViewModel LoginStatus { get; private set; }

        public MenuViewModel(IMenuRegistry menuRegistry, IUserSession userSession,
            ILoginStatusViewModel loginStatusViewModel,
            IVersionViewModel versionViewModel, IAppSettings appSettings,
            IConnectivityService connectivityService)
        {
            _menuRegistry = menuRegistry;
            LoginStatus = loginStatusViewModel;
            _userSession = userSession;
            _appSettings = appSettings;
            _connectivityService = connectivityService;
            VersionViewModel = versionViewModel;
            ApplicationCommands.ChangeScreenCommand.RegisterCommand(new DelegateCommand<MenuInfo>(MenuItemClicked));
            ApplicationCommands.LoginSucceeded.RegisterCommand(new DelegateCommand<object>(UserLoggedIn));
            ApplicationCommands.UserLoggedOut.RegisterCommand(new DelegateCommand<object>(UserLoggedOut));
            ApplicationCommands.ChangeSubmenuCommand.RegisterCommand(new DelegateCommand<SubMenuInfo>(ChangeSubmenuCommandExecute));

            ApplicationCommands.ConnectivityChanged.RegisterCommand(new DelegateCommand<object>(ConnectivityChangedExecute));

            Initialize();
        }

        private void ConnectivityChangedExecute(object obj)
        {
            var isOnline = (bool) obj;
            foreach (var menuItemViewModel in MenuItems)
            {
                menuItemViewModel.IsEnabled = menuItemViewModel.WorksOffline || isOnline;
            }
        }

        private void ChangeSubmenuCommandExecute(SubMenuInfo obj)
        {
            foreach (var subMenuItemViewModel in SubMenuItems)
            {
                subMenuItemViewModel.LayoutChanged();
            }
            OnPropertyChanged(() => SubMenuItems);
        }

        private void MenuItemClicked(MenuInfo obj)
        {
            if (obj == null)
            {
                return;
            }
            var menuItem = _menuItems.SingleOrDefault(m => m.MenuInfo.ScreenGuid == obj.ScreenGuid);
            if (!obj.IsWindow)
            {
                SelectedItem = menuItem;
            }
        }

        private void UserLoggedOut(object obj)
        {
            OnPropertyChanged(() => IsVisible);
        }

        private void Initialize()
        {
            SubMenuItems = new ObservableCollection<ISubMenuItemViewModel>();
            MenuItems = new ObservableCollection<IMenuItemViewModel>(_menuRegistry.MenuList.OrderBy(m => m.MenuIndex).Select(m => new MenuItemViewModel(m)));
            _menuRegistry.MenuList.CollectionChanged += menuItems_CollectionChanged;

            var isRetina = SystemParameters.PrimaryScreenHeight == 1800;            

            if (ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.IsFirstRun)
            {
                var invalidHeight = 126;
                var correctHeight = 84;
                var invalidFontSize = 14;
                var wrongSettings = _appSettings.InactiveTaskTaskNameFontSize == invalidFontSize
                                    && (_appSettings.InactiveTaskHeight == invalidHeight
                                        || _appSettings.InactiveTaskHeight == correctHeight);
                if (wrongSettings)
                {
                    //set to default
                    _appSettings.InActiveTaskWidth = 126;
                    _appSettings.InactiveTaskHeight = correctHeight;
                    _appSettings.InactiveTaskTimeSpentFontSize = 18;
                    _appSettings.InactiveTaskTaskNameFontSize = 12;
                    _appSettings.InactiveTaskDescriptionFontSize = 11;

                    _appSettings.Save();
                }

                if (isRetina && _appSettings.InactiveTaskHeight == correctHeight)
                {
                    _appSettings.InActiveTaskWidth = 205;
                    _appSettings.InactiveTaskHeight = 150;
                    _appSettings.InactiveTaskTimeSpentFontSize = 18;
                    _appSettings.InactiveTaskTaskNameFontSize = 18;
                    _appSettings.InactiveTaskDescriptionFontSize = 18;
                    _appSettings.Save();
                }


                // Display release notes so user knows what's new
                ShowReleaseNotes = true;

                if (_appSettings.ShouldResyncOnNewDeployment)
                {
                    ThreadPool.QueueUserWorkItem((x) =>
                        {
                            Thread.Sleep((int) TimeSpan.FromSeconds(5).TotalMilliseconds);
                            MessageBox.Show("This update requires a full resync. " +
                                            "Press OK to start the sync." +
                                            " Unsynced data will be preserved", "Confirm action", MessageBoxButton.OK);
                            ApplicationCommands.Resync.Execute(null);
                        });
                }
            }
        }

        public bool IsVisible { get { return _userSession.IsLoggedIn; } }

        private void UserLoggedIn(object obj)
        {
            ApplicationCommands.ChangeScreenCommand.Execute(_menuRegistry.GetStartPage);

            OnPropertyChanged(() => IsVisible);
        }

        void menuItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    MenuItems.Add(new MenuItemViewModel(item as MenuInfo));
                }
                MenuItems = new ObservableCollection<IMenuItemViewModel>(MenuItems.OrderBy(m => m.MenuInfo.MenuIndex));
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

        private ObservableCollection<IMenuItemViewModel> _menuItems;
        public ObservableCollection<IMenuItemViewModel> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                OnPropertyChanged(() => MenuItems);
                ConnectivityChangedExecute(_connectivityService.IsOnline);
            }
        }

        private IMenuItemViewModel _selectedItem;
        public IMenuItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = false;
                }

                _selectedItem = value;
                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;
                    SubMenuItems.Clear();
                    foreach (var subMenuInfo in _selectedItem.MenuInfo.SubMenuInfos)
                    {
                        SubMenuItems.Add(new SubMenuItemViewModel(subMenuInfo));
                    }
                    var submenuInfo = _selectedItem.MenuInfo.SubMenuInfos.SingleOrDefault(x => x.IsActive);
                    if (submenuInfo != null)
                    {
                        ApplicationCommands.ChangeSubmenuCommand.Execute(submenuInfo);
                    }
                }
            }
        }

        private ObservableCollection<ISubMenuItemViewModel> _subMenuItems;
        public ObservableCollection<ISubMenuItemViewModel> SubMenuItems
        {
            get { return _subMenuItems; }
            set
            {
                _subMenuItems = value;
                OnPropertyChanged(() => SubMenuItems);
            }
        }

        public string VersionToolTip
        {
            get
            {
                var msg = new StringBuilder();

                msg
                    .AppendLine("Click to see release notes")
                    .AppendLine()
                    .AppendLine("Environment:")
                    .AppendLine(string.Format("   {0}", _appSettings.EnvironmentDescription))
                    .AppendLine("Endpoints:")
                    .AppendLine(string.Format("   Auth: {0}", _appSettings.AuthenticationServiceUri))
                    .AppendLine(string.Format("   Wcf: {0}", _appSettings.TrexWcfServiceEndpointUri))
                    .AppendLine(string.Format("   Servicestack: {0}", _appSettings.JsonEndpointUri));

                return msg.ToString();
            }
        }

        public bool ShowReleaseNotes { get; set; }
    }
}
