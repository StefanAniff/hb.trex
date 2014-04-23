using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Events;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.Infrastructure.Events;

namespace Trex.Infrastructure.Implemented
{
    public class MenuRegistry : List<IMenuInfo>, IMenuRegistry
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMenuIndexService _menuIndexService;

        public MenuRegistry(IEventAggregator eventAggregator, IMenuIndexService menuIndexService)
        {
            _eventAggregator = eventAggregator;
            _menuIndexService = menuIndexService;
        }

        #region IMenuRegistry Members

        public IMenuInfo GetStartPage()
        {
            return this.SingleOrDefault(m => m.IsStartPage);
        }

        public void RegisterMenuInfo(IMenuInfo menuInfo, string parent, int menuIndex)
        {
            menuInfo.MenuIndex = menuIndex;
            //if submenu item
            if (!string.IsNullOrEmpty(parent))
            {
                var parentMenu = FindMenuItem(parent);

                if (parentMenu == null)
                {
                    parentMenu = MenuInfo.CreateMenuItem(parent, true, false, menuInfo.RequiredPermission);
                    RegisterTopMenuItem(parentMenu, _menuIndexService.GetTopMenuIndex(parentMenu));
                }

                //if(parentMenu.Children.Count > menuIndex +1)
                //    parentMenu.Children.Insert(menuIndex,menuInfo);
                //else
                //{
                parentMenu.Children.Add(menuInfo);
                // }
                parentMenu.Children = parentMenu.Children.OrderBy(m => m.MenuIndex).ToList();
            }

            else
            {
                RegisterTopMenuItem(menuInfo, _menuIndexService.GetTopMenuIndex(menuInfo));
            }

            _eventAggregator.GetEvent<MenuItemRegisteredEvent>().Publish(menuInfo);
        }

        #endregion

        private void RegisterTopMenuItem(IMenuInfo menuInfo, int index)
        {
            menuInfo.MenuIndex = index;
            var count = Count;

            if (count == 0)
            {
                Add(menuInfo);
                return;
            }

            for (var i = 0; i < count; i++)
            {
                if (this[i].MenuIndex > index)
                {
                    Insert(i, menuInfo);
                    return;
                }
            }

            Add(menuInfo);
        }

        private IMenuInfo FindMenuItem(string menuName)
        {
            IMenuInfo menuInfo = null;
            FindMenuItemRecursive(this, menuName, out menuInfo);

            return menuInfo;
        }

        private void FindMenuItemRecursive(IEnumerable<IMenuInfo> children, string menuName, out IMenuInfo foundItem)
        {
            foreach (var menuInfo in children)
            {
                if (menuInfo.DisplayName == menuName)
                {
                    foundItem = menuInfo;
                    return;
                }

                FindMenuItemRecursive(menuInfo.Children, menuName, out foundItem);
            }

            foundItem = null;
        }
    }
}