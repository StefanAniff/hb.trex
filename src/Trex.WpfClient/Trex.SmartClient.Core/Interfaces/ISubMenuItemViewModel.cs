using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface ISubMenuItemViewModel
    {
        string DisplayName { get; set; }
        DelegateCommand<object> ItemClicked { get; set; }
        bool IsChecked { get; set; }
        Guid SubMenuGuid { get; }
        void LayoutChanged();
    }
}
