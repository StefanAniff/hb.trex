using System;
using Microsoft.Practices.Unity;
using Trex.Core.Attributes;
using Trex.Core.Implemented;

namespace Trex.Reports.StatusReportWithEstimatesScreen
{
    [Screen(Name = "StatusReportWithEstimates", CanBeDeactivated = false)]
    public class StatusReportWithEstimatesScreen : ScreenBase
    {
        public StatusReportWithEstimatesScreen(Guid guid, IUnityContainer unityContainer) : base(guid, unityContainer) {}

        protected override void Initialize()
        {
            var statusReportView = new ReportView.ReportView();

            MasterView = statusReportView;
        }
    }
}