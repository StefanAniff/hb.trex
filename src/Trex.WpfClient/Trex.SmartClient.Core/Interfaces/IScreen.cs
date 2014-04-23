using System;
using Microsoft.Practices.Prism.Regions;
using Trex.SmartClient.Core.Attributes;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface IScreen
    {
        Guid Guid { get; }
        IView MasterView { get; }
        bool IsActive { get; set; }
        ScreenAttribute GetScreenAttribute();
        void InitializeScreen(IRegion region, Guid screenID);
        void AddRegion(string region, IView view, bool asDeactivated = true);
        void AddRegion(string region, IView view, string viewName, bool asDeactivated = true);
        void ActivateView(string argument, string regionName);
        void DeactivateView(string viewName, string regionName);
    }
}