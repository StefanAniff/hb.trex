using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Forecast.ForecastOverview;
using Trex.SmartClient.Forecast.ForecastRegistration;

namespace Trex.SmartClient.Forecast.ForecastMasterScreen
{
    public class ForecastScreenFactory : IScreenFactory
    {
        private readonly IRegionNames _regionNames;
        private readonly IUnityContainer _unityContainer;

        public ForecastScreenFactory(IRegionNames regionNames, IUnityContainer unityContainer)
        {
            _regionNames = regionNames;
            _unityContainer = unityContainer;
        }

        public IScreen CreateScreen(IRegion region, Guid guid)
        {
            var forecastScreen = new ForecastMasterScreen(guid, _unityContainer);
            forecastScreen.InitializeScreen(region, guid);

            // Forecast registration
            var registrationRootView = _unityContainer.Resolve<IForecastRegistrationRootView>();
            var registrationRootViewModel = _unityContainer.Resolve<IForecastRegistrationRootViewModel>();
            registrationRootView.ApplyViewModel(registrationRootViewModel);
            forecastScreen.AddRegion(_regionNames.SubmenuRegion, registrationRootView);

            // Forecast overview
            var overviewView = _unityContainer.Resolve<IForecastOverviewView>();
            var overviewModel = _unityContainer.Resolve<IForecastOverviewViewModel>();
            overviewView.ApplyViewModel(overviewModel);

            forecastScreen.AddRegion(_regionNames.SubmenuRegion, overviewView);
            overviewModel.Initialize();

            return forecastScreen;
        }
    }
}