using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.Infrastructure.Implemented;
using TrexSL.Shell.Menu.MenuModel;

namespace TrexSL.Shell.Menu.MenuView
{
    public class MenuViewModel : ViewModelBase, IMenuViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMenuRegistry _menuRegistry;
        private readonly IUserSession _userSession;

        private MenuItemsCollection _items;

        public MenuViewModel(IMenuRegistry menuRegistry, IEventAggregator eventAggregator, IUserSession userSession)
        {
            _menuRegistry = menuRegistry;
            _eventAggregator = eventAggregator;
            _userSession = userSession;

            Items = new MenuItemsCollection();

            WireUpEvents();
            ApplicationCommands.LoginSucceeded.RegisterCommand(new DelegateCommand<object>(LoginSucceeded));
            LogOutCommand = new DelegateCommand<object>(LogoutUser);
        }

        public MenuItemsCollection Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        public string UserName
        {
            get
            {
                if (_userSession.IsLoggedIn)
                {
                    return _userSession.CurrentUser.Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public bool IsUserLoggedIn
        {
            get { return _userSession.IsLoggedIn; }
        }

        public DelegateCommand<object> LogOutCommand { get; set; }

        private void WireUpEvents()
        {
            // _eventAggregator.GetEvent<MenuItemRegisteredEvent>().Subscribe(AddMenuItem);
        }

        //public void InitializeMenu(string message)
        //{
        //    foreach (var menuInfo in _menuRegistry.MenuList)
        //    {
        //        AddMenuItem(menuInfo);
        //    }
        //}

        public void AddMenuItem(IMenuInfo menuInfo)
        {
            Items = GetMenuFromRegistry();
            if (menuInfo.IsStartPage && _userSession.IsLoggedIn)
            {
                ApplicationCommands.ChangeScreenCommand.Execute(menuInfo);
            }
        }

        private MenuItemsCollection GetMenuFromRegistry()
        {
            var menuItems = new MenuItemsCollection();
            foreach (var menuInfo in _menuRegistry)
            {
                if (!UserContext.Instance.User.HasPermission(menuInfo.RequiredPermission))
                {
                    continue;
                }
                menuItems.Add(new MenuItem(menuInfo));
            }
            return menuItems;
        }

        private void LoginSucceeded(object obj)
        {
            Refresh();
            Items = GetMenuFromRegistry();
            var menuInfo = _menuRegistry.GetStartPage();
            if (menuInfo != null)
            {
                ApplicationCommands.ChangeScreenCommand.Execute(menuInfo);
            }
        }

        private void LogoutUser(object obj)
        {
            _userSession.LogOutUser();
            Refresh();
            ApplicationCommands.UserLoggedOut.Execute(null);
        }

        private void Refresh()
        {
            OnPropertyChanged("UserName");
            OnPropertyChanged("IsUserLoggedIn");
        }
    }
}