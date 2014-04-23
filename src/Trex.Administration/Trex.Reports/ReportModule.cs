
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.Reports.InteractiveReportScreen;
using Trex.ServiceContracts;

namespace Trex.Reports
{
    public class ReportModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IMenuRegistry _menuRegistry;
        private readonly IScreenFactoryRegistry _screenFactoryRegistry;

        public ReportModule(IUnityContainer container, IScreenFactoryRegistry screenFactoryRegistry, IMenuRegistry menuRegistry)
        {
            _container = container;
            _screenFactoryRegistry = screenFactoryRegistry;
            _menuRegistry = menuRegistry;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewModels();

            //var statusReport = MenuInfo.CreateMenuItem("Time report", true, false, Permissions.ReportPermission);

            //_screenFactoryRegistry.RegisterFactory(statusReport.ScreenGuid, new StatusReportScreen.StatusReportScreenFactory(_container));
            //_menuRegistry.RegisterMenuInfo(statusReport, "Reports", 0);

            //var statusReportWithEstimates = MenuInfo.CreateMenuItem("Status report w. estimates", true, false, Permissions.ReportPermission);

            //_screenFactoryRegistry.RegisterFactory(statusReportWithEstimates.ScreenGuid, new StatusReportWithEstimatesScreen.StatusReportWithEstimatesScreenFactory(_container));
            //_menuRegistry.RegisterMenuInfo(statusReportWithEstimates, "Reports", 1);

            var interactiveReport = MenuInfo.CreateMenuItem("Interactive report", true, false, Permissions.ReportPermission);
            _screenFactoryRegistry.RegisterFactory(interactiveReport.ScreenGuid, new InteractiveReportScreenFactory(_container));

            _menuRegistry.RegisterMenuInfo(interactiveReport, "Reports", 2);
        }

        #endregion

        private void RegisterViewModels() {}
    }
}