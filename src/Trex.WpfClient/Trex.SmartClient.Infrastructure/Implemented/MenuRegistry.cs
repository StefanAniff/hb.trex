using System.Collections.ObjectModel;
using System.Linq;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class MenuRegistry : IMenuRegistry
    {
        public ObservableCollection<IMenuInfo> MenuList { get; private set; }

        public MenuRegistry()
        {
            MenuList = new ObservableCollection<IMenuInfo>();
        }

        public IMenuInfo GetStartPage
        {
            get { return MenuList.SingleOrDefault(m => m.IsStartPage); }
        }

        public void RegisterMenuInfo(IMenuInfo menuInfo)
        {
            MenuList.Add(menuInfo);

            if (menuInfo.IsStartPage)
            {
                ApplicationCommands.ChangeScreenCommand.Execute(menuInfo);
            }
        }
    }
}