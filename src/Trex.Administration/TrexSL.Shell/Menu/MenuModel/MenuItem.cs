using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.Infrastructure.Commands;
using Trex.Infrastructure.Implemented;

namespace TrexSL.Shell.Menu.MenuModel
{
    [ContentProperty("Items")]
    public class MenuItem : INotifyPropertyChanged
    {
        private bool _isChecked;
        private bool _isEnabled = true;

        public MenuItem(IMenuInfo menuInfo)
        {
            MenuInfo = menuInfo;

            Text = menuInfo.DisplayName;

            Items = new MenuItemsCollection(this);
            ItemClicked = new DelegateCommand<object>(ExecuteItemClicked);
            AddChildren();
        }

        public IMenuInfo MenuInfo { get; private set; }

        public int Index
        {
            get { return MenuInfo.MenuIndex; }
        }

        public bool IsGroupMenuItem { get; set; }

        public string Text { get; set; }

        public string GroupName { get; set; }

        public bool IsCheckable { get; set; }

        public bool IsSeparator { get; set; }

        public string ImageUrl { get; set; }

        public bool StaysOpenOnClick { get; set; }

        public MenuItemsCollection Items { get; private set; }

        public MenuItem Parent { get; set; }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    OnPropertyChanged("IsChecked");

                    if (!string.IsNullOrEmpty(GroupName))
                    {
                        if (IsChecked)
                        {
                            UncheckOtherItemsInGroup();
                        }
                        else
                        {
                            IsChecked = true;
                        }
                    }
                }
            }
        }

        public Image Image
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrl))
                {
                    return null;
                }

                return new Image
                           {
                               Source = new BitmapImage(new Uri(ImageUrl, UriKind.RelativeOrAbsolute))
                           };
            }
        }

        public DelegateCommand<object> ItemClicked { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void AddChildren()
        {
            foreach (var child in MenuInfo.Children)
            {
                if (!UserContext.Instance.User.HasPermission(child.RequiredPermission))
                {
                    continue;
                }
                Items.Add(new MenuItem(child));
            }
        }

        private void UncheckOtherItemsInGroup()
        {
            var groupItems = Parent.Items.Where(item => item.GroupName == GroupName);
            foreach (var item in groupItems)
            {
                if (item != this)
                {
                    item._isChecked = false;
                    item.OnPropertyChanged("IsChecked");
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ExecuteItemClicked(object obj)
        {
            ApplicationCommands.ChangeScreenCommand.Execute(MenuInfo);
        }
    }
}