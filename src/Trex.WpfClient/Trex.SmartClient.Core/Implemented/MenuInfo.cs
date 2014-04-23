using System;
using System.Collections.Generic;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Core.Implemented
{
    public class MenuInfo : IMenuInfo
    {
        private readonly List<SubMenuInfo> _subMenuInfos;

        public bool VisibleInMenu { get; private set; }
        public string DisplayName { get; private set; }
        public int MenuIndex { get; private set; }
        public bool IsStartPage { get; private set; }
        public bool IsWindow { get; private set; }
        public Guid ScreenGuid { get; private set; }

        public IEnumerable<SubMenuInfo> SubMenuInfos
        {
            get { return _subMenuInfos; }
        }

        public bool WorksOffline { get; private set; }

        public MenuInfo()
        {
            _subMenuInfos = new List<SubMenuInfo>();
        }

        public void AddSubMenu(SubMenuInfo subMenuInfo)
        {
            _subMenuInfos.Add(subMenuInfo);
        }

        public static MenuInfo Create(int menuIndex, string displayName, bool visibleInMenu, bool isStartPage, bool isWindow, bool worksOffline)
        {
            return new MenuInfo
                {
                    DisplayName = displayName,
                    VisibleInMenu = visibleInMenu,
                    ScreenGuid = Guid.NewGuid(),
                    MenuIndex = menuIndex,
                    IsStartPage = isStartPage,
                    IsWindow = isWindow,
                    WorksOffline = worksOffline
                };
        }
    }
}