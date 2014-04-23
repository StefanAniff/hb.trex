using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface IVersionViewModel
    {
        DelegateCommand<object> GoToNext { get; set; }
        DelegateCommand<object> GoToPrevious { get; set; }
        List<IRelease> Releases { get; set; }
        bool GoToNextCanExecute { get; }
        bool GoToPreviousCanExecute { get; }
        IRelease SelectedRelease { get; }
    }
}
