using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Infrastructure.Releasenotes;

namespace Trex.SmartClient.MenuView.Designdata
{
    public class DesignMenuViewModel : ViewModelBase, IMenuViewModel
    {
        private ObservableCollection<ISubMenuItemViewModel> _subMenuItems;

        public IVersionViewModel VersionViewModel
        {
            get
            {
                return new VersionViewModel(new IRelease[]
                                                  {
                                                      new Version3_4_0(),
                                                      new Version3_5_0(), 
                                                  });
            }
        }

        public ILoginStatusViewModel LoginStatus
        {
            get { return new DesignLoginStatusViewModel(); }
        }

        public bool IsVisible
        {
            get { return true; }
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

        public string VersionToolTip
        {
            get { return string.Empty; }
        }

        public ObservableCollection<IMenuItemViewModel> MenuItems
        {
            get
            {
                var menuItemViewModels = new ObservableCollection<IMenuItemViewModel>();
                menuItemViewModels.Add(new MenuItemViewModel(MenuInfo.Create(0, "Registration", true, true, false, true)));
                menuItemViewModels.Add(new MenuItemViewModel(MenuInfo.Create(1, "Reports", true, false, false, true)));
                menuItemViewModels.Add(new MenuItemViewModel(MenuInfo.Create(2, "Settings", true, false, false, true)));
                return menuItemViewModels;
            }
        }

        public IMenuItemViewModel SelectedItem
        {
            get { return MenuItems.FirstOrDefault(); }
        }

        public bool ShowReleaseNotes
        {
            get { return true; }
            set { }
        }


        public ObservableCollection<ISubMenuItemViewModel> SubMenuItems
        {
            get
            {
                var list = new List<ISubMenuItemViewModel>();
                list.Add(new SubMenuItemViewModel(SubMenuInfo.Create("hej", null, null)));
                return _subMenuItems;
            }
            set { _subMenuItems = value; }
        }

        public void Dispose()
        {

        }
    }
}
