using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Regions;
using Trex.SmartClient.Core.Attributes;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Core.Implemented
{
    public class ScreenBase : IScreen
    {
        private Dictionary<string, IView> _regions = new Dictionary<string, IView>();
        public IRegionManager LocalRegionManager { get; private set; }
        protected bool _hasBeenInitialized = false;

        public ScreenBase(Guid guid)
        {
            Guid = guid;
            IsActive = false;
        }

        public void InitializeScreen(IRegion region, Guid screenID)
        {
            if (!_hasBeenInitialized)
            {
                LocalRegionManager = region.Add(MasterView, screenID.ToString(), true);
                _hasBeenInitialized = true;
            }
        }

        public Guid Guid { get; protected set; }

        public IView MasterView { get; protected set; }

        public bool IsActive { get; set; }

        public ScreenAttribute GetScreenAttribute()
        {
            ScreenAttribute attribute = null;
            foreach (var attr in GetType().GetCustomAttributes(typeof(ScreenAttribute), true))
            {
                attribute = attr as ScreenAttribute;
                break;
            }
            return attribute;
        }
        public void AddRegion(string region, IView view, string viewName, bool asDeactivated = true)
        {
            if (!_regions.ContainsKey(region))
            {
                _regions.Add(region, view);
            }
            LocalRegionManager.Regions[region].Add(view, viewName);

            if (asDeactivated)
                LocalRegionManager.Regions[region].Deactivate(view);
        }

        public void AddRegion(string region, IView view, bool asDeactivated = true)
        {
            var viewName = view.GetType().Name;
            AddRegion(region, view, viewName, asDeactivated);
        }

        public void ActivateView(string argument, string regionName)
        {
            var region = LocalRegionManager.Regions[regionName];
            var view = region.GetView(argument);
            region.Activate(view);
        }

        public void DeactivateView(string viewName, string regionName)
        {
            var region = LocalRegionManager.Regions[regionName];
            var view = region.GetView(viewName);
            region.Deactivate(view);
        }
        
    }
}