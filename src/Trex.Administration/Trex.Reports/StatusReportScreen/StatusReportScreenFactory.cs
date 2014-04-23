using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.Core.Interfaces;

namespace Trex.Reports.StatusReportScreen
{
    public class StatusReportScreenFactory : IScreenFactory
    {
        private readonly IUnityContainer _unityContainer;

        public StatusReportScreenFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        #region IScreenFactory Members

        public IScreen CreateScreen(IRegion region, Guid guid)
        {
            var screen = new StatusReportScreen(guid, _unityContainer);

            region.Add(screen.MasterView, screen.Guid.ToString());
            return screen;
        }

        #endregion
    }
}