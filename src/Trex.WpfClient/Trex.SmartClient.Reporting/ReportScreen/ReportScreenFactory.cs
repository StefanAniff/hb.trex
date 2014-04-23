using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Reporting.ReportScreen
{
    public class ReportScreenFactory : IScreenFactory
    {
        private IUnityContainer _unityContainer;

        public ReportScreenFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public IScreen CreateScreen(IRegion region, Guid guid)
        {
            var reportScreen = new ReportScreen(guid,_unityContainer);
            region.Add(reportScreen.MasterView, guid.ToString());
            return reportScreen;
        }
    }
}
