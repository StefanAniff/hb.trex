using System;
using Microsoft.Practices.Unity;
using Trex.Core.Attributes;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Reports.StatusReportScreen.StatusReportView;

namespace Trex.Reports.StatusReportScreen
{
    [Screen(Name = "StatusReportScreen", CanBeDeactivated = false)]
    public class StatusReportScreen : ScreenBase
    {
        public StatusReportScreen(Guid guid, IUnityContainer unityContainer) : base(guid, unityContainer) {}

        protected override void Initialize()
        {
            var userSEttingsSErvice = Container.Resolve<IUserSettingsService>();
            var statusReportView = new StatusReportView.StatusReportView();
            var statusReportViewModel = new StatusReportViewModel(userSEttingsSErvice);
            statusReportView.ViewModel = statusReportViewModel;

            MasterView = statusReportView;
        }
    }
}