using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface IMenuItemViewModel
    {
        IMenuInfo MenuInfo { get; }
        string ItemName { get; }
        DelegateCommand<object> ItemClicked { get; set; }
        bool IsSelected { get; set; }
        bool WorksOffline { get; }
        bool IsEnabled { get; set; }
    }
}
