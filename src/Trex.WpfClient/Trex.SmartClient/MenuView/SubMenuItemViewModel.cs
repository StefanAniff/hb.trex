using System;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.MenuView
{
    public class SubMenuItemViewModel : ViewModelBase, ISubMenuItemViewModel
    {
        private readonly SubMenuInfo _subMenuInfo;
        public DelegateCommand<object> ItemClicked { get; set; }
        public Guid SubMenuGuid
        {
            get
            {
                return _subMenuInfo.Guid;
            }
        }

        public void LayoutChanged()
        {
           RaiseIsCheckedChanged();
        }

        public SubMenuItemViewModel(SubMenuInfo subMenuInfo)
        {
            _subMenuInfo = subMenuInfo;
            DisplayName = _subMenuInfo.DisplayName;
            IsChecked = subMenuInfo.IsActive;
            ItemClicked = new DelegateCommand<object>(ItemClickedExecuted);
            subMenuInfo.IsActiveChanged += (x, y) => RaiseIsCheckedChanged();
        }

        private void ItemClickedExecuted(object obj)
        {
            ApplicationCommands.ChangeSubmenuCommand.Execute(_subMenuInfo);
            IsChecked = true;
        }

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                OnPropertyChanged(() => DisplayName);
            }
        }

        public bool IsChecked
        {
            get { return _subMenuInfo.IsActive; }
            set
            {
                _subMenuInfo.IsActive = value;
                RaiseIsCheckedChanged();
            }
        }

        private void RaiseIsCheckedChanged()
        {
            OnPropertyChanged(() => IsChecked);
            OnPropertyChanged(() => CanClick);
        }

        public bool CanClick
        {
            get { return !IsChecked; }
        }
    }
}