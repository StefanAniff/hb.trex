using System;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Attributes;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Reporting.ReportScreen.ReportScreenMasterView;

namespace Trex.SmartClient.Reporting.ReportScreen
{
    [Screen(IsStartScreen = false, CanBeDeactivated = true)]
    public class ReportScreen : ScreenBase
    {
        public ReportScreen(Guid guid, IUnityContainer unityContainer)
            : base(guid)
        {
            var view = new ReportScreenMasterView.ReportScreenMasterview();
            var viewModel = unityContainer.Resolve<ReportScreenMasterViewModel>();
            view.ApplyViewModel(viewModel);

            MasterView = view;
        }
    }
}
