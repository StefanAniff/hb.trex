using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface IMenuRegistry
    {
        void RegisterMenuInfo(IMenuInfo menuInfo);
        ObservableCollection<IMenuInfo> MenuList { get;}
        IMenuInfo GetStartPage { get; }

        
    }
}
