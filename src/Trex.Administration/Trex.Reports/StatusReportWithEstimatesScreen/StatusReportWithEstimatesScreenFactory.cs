using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.Core.Interfaces;

namespace Trex.Reports.StatusReportWithEstimatesScreen
{
    public class StatusReportWithEstimatesScreenFactory : IScreenFactory
    {
        private readonly IUnityContainer _unityContainer;

        public StatusReportWithEstimatesScreenFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        #region IScreenFactory Members

        public IScreen CreateScreen(IRegion region, Guid guid)
        {
            var screen = new StatusReportWithEstimatesScreen(guid, _unityContainer);

            region.Add(screen.MasterView, screen.Guid.ToString());
            return screen;
        }

        #endregion
    }
}