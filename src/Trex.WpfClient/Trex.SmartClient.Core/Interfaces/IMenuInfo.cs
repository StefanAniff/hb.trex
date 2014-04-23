using System;
using System.Collections.Generic;
using Trex.SmartClient.Core.Implemented;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface IMenuInfo
    {
        bool VisibleInMenu { get; }
        string DisplayName { get; }
        int MenuIndex { get; }
        Guid ScreenGuid { get; }
        bool IsStartPage { get; }
        IEnumerable<SubMenuInfo> SubMenuInfos { get; }
        bool WorksOffline { get; }
        void AddSubMenu(SubMenuInfo subMenuInfo);
    }
}