using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface IMenuViewModel : IViewModel
    {
        IVersionViewModel VersionViewModel { get; }
        ILoginStatusViewModel LoginStatus { get; }
        bool IsVisible { get; }
        string Version { get; }
        ObservableCollection<IMenuItemViewModel> MenuItems { get; }
        IMenuItemViewModel SelectedItem { get; }
        bool ShowReleaseNotes { get; set; }
        ObservableCollection<ISubMenuItemViewModel> SubMenuItems { get; set; }
        string VersionToolTip { get; }
    }
}
