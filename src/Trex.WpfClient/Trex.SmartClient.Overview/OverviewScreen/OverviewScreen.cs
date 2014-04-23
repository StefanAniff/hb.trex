using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Attributes;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Overview.DayOverviewScreen.Viewmodels;

namespace Trex.SmartClient.Overview.OverviewScreen
{
    [Screen(IsStartScreen = false, CanBeDeactivated = true)]
    public class OverviewScreen : ScreenBase
    {
        public OverviewScreen(Guid guid, IUnityContainer unityContainer) : base(guid)
        {
            var view = new OverviewScreenMasterview();
            var viewModel = unityContainer.Resolve<IOverviewScreenMasterviewModel>();
            view.ApplyViewModel(viewModel);

            MasterView = view;
        }
    }
}