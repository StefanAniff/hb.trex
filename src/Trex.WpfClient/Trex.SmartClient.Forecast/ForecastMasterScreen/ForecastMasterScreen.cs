using System;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Attributes;
using Trex.SmartClient.Core.Implemented;

namespace Trex.SmartClient.Forecast.ForecastMasterScreen
{   
    [Screen(IsStartScreen = false, CanBeDeactivated = true)]
    public class ForecastMasterScreen : ScreenBase
    {
        public ForecastMasterScreen(Guid guid, IUnityContainer unityContainer) : base(guid)
        {
            var view = unityContainer.Resolve<IForecastMasterView>();
            var viewModel = unityContainer.Resolve<IForecastMasterViewModel>();
            view.ApplyViewModel(viewModel);

            MasterView = view;
        }
    }
}