using System;
using System.Collections.Generic;
using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.Implemented
{
    public class MenuInfo : IMenuInfo
    {
        private MenuInfo()
        {
            Children = new List<IMenuInfo>();
        }

        #region IMenuInfo Members

        public List<IMenuInfo> Children { get; set; }

        public IMenuInfo Parent { get; set; }

        public bool VisibleInMenu { get; private set; }
        public string DisplayName { get; private set; }

        public int CategoryIndex { get; private set; }

        public int MenuIndex { get; set; }

        public Guid ScreenGuid { get; private set; }

        public bool IsStartPage { get; private set; }

        public Permissions RequiredPermission { get; set; }

        #endregion

        public static MenuInfo CreateMenuItem(string displayName, bool visibleInMenu, bool isStartPage, Permissions requiredPermission)
        {
            return new MenuInfo
                       {
                           DisplayName = displayName,
                           VisibleInMenu = visibleInMenu,
                           ScreenGuid = Guid.NewGuid(),
                           IsStartPage = isStartPage,
                           RequiredPermission = requiredPermission
                       };
        }

        //public static MenuInfo Create(string displayName, bool visibleInMenu, Guid guid)
        //{
        //    return Create(string.Empty, displayName, visibleInMenu, guid);
        //}
    }
}