using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.MenuView
{
    public class MenuItemViewModel : ViewModelBase, IMenuItemViewModel
    {
        private readonly IMenuInfo _menuInfo;
        public DelegateCommand<object> ItemClicked { get; set; }

        public MenuItemViewModel(IMenuInfo menuInfo)
        {
            _menuInfo = menuInfo;
            IsEnabled = true;
            ItemClicked = new DelegateCommand<object>(ExecuteItemClicked);
        }

        private void ExecuteItemClicked(object obj)
        {
            ApplicationCommands.ChangeScreenCommand.Execute(_menuInfo);
        }

        public IMenuInfo MenuInfo
        {
            get { return _menuInfo; }
        }

        public string ItemName
        {
            get { return _menuInfo.DisplayName; }
        }

        public bool WorksOffline
        {
            get { return _menuInfo.WorksOffline; }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged(() => IsEnabled);
            }
        }


        private bool _isSelected;
        private bool _isEnabled;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(() => IsSelected);
            }
        }
    }
}