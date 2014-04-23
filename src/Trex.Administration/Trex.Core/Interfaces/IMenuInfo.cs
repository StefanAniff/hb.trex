using System;
using System.Collections.Generic;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.Interfaces
{
    public interface IMenuInfo
    {
        IMenuInfo Parent { get; }
        List<IMenuInfo> Children { get; set; }
        bool VisibleInMenu { get; }
        string DisplayName { get; }
        int CategoryIndex { get; }
        int MenuIndex { get; set; }
        Guid ScreenGuid { get; }
        bool IsStartPage { get; }
        Permissions RequiredPermission { get; }
    }
}