using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Forecast.ForecastMasterScreen;
using Trex.SmartClient.Forecast.ForecastOverview;
using Trex.SmartClient.Forecast.ForecastRegistration;
using Trex.SmartClient.Forecast.ForecastRegistration.Helpers;
using Trex.SmartClient.Forecast.ForecastStatistics;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast
{
    [Module(ModuleName = "ForecastModule")]
    public class ForecastModule : IModule
    {
        private readonly IUnityContainer _unityContainer;
        private readonly IMenuRegistry _menuRegistry;
        private readonly IScreenFactoryRegistry _screenFactoryRegistry;
        private readonly ForecastScreenFactory _forecastScreenFactory;

        private MenuInfo _adminScreenInfo;

        public ForecastModule(IUnityContainer unityContainer
            , IMenuRegistry menuRegistry
            , IScreenFactoryRegistry screenFactoryRegistry
            , ForecastScreenFactory forecastScreenFactory)
        {
            _unityContainer = unityContainer;
            _menuRegistry = menuRegistry;
            _screenFactoryRegistry = screenFactoryRegistry;
            _forecastScreenFactory = forecastScreenFactory;

            RegisterServices();
            RegisterViews();
            RegisterViewModels();
        }

        private void RegisterServices()
        {
            _unityContainer.RegisterType<ForecastTypeProvider>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<ForecastRegistrationSelectedUserHandler>(new ContainerControlledLifetimeManager());
        }

        public void Initialize()
        {
            // Main module
            _adminScreenInfo = MenuInfo.Create(1, "Workplan", true, false, false, false);
            _screenFactoryRegistry.RegisterFactory(_adminScreenInfo.ScreenGuid, _forecastScreenFactory);
            _menuRegistry.RegisterMenuInfo(_adminScreenInfo);
            
            // Sub module Registration
            var submenuForecastRegistration = SubMenuInfo.Create("Registration", typeof(ForecastRegistrationRootView), _adminScreenInfo);
            submenuForecastRegistration.IsActive = true;
            _adminScreenInfo.AddSubMenu(submenuForecastRegistration);

            // Sub module Overview
            var submenuForecastOverview = SubMenuInfo.Create("Overview", typeof(ForecastOverviewView), _adminScreenInfo);
            _adminScreenInfo.AddSubMenu(submenuForecastOverview);
        }

        private void RegisterViews()
        {
            _unityContainer.RegisterType<IForecastMasterView, ForecastMasterView>();
            _unityContainer.RegisterType<IForecastRegistrationRootView, ForecastRegistrationRootView>();
            _unityContainer.RegisterType<IForecastRegistrationView, ForecastRegistrationView>();
            _unityContainer.RegisterType<IForecastStatisticsTabView, ForecastStatisticsTabView>();
            _unityContainer.RegisterType<IForecastOverviewView, ForecastOverviewView>();
        }

        private void RegisterViewModels()
        {
            _unityContainer.RegisterType<IForecastMasterViewModel, ForecastMasterViewModel>();
            _unityContainer.RegisterType<IForecastRegistrationRootViewModel, ForecastRegistrationRootViewModel>();
            _unityContainer.RegisterType<IForecastRegistrationViewModel, ForecastRegistrationViewModel>();
            _unityContainer.RegisterType<IForecastStatisticsTabViewModel, ForecastStatisticsTabViewModel>();
            _unityContainer.RegisterType<IForecastOverviewViewModel, ForecastOverviewViewModel>();
        }
    }
}