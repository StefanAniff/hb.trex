using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Overview.DayOverviewScreen.Viewmodels;
using Trex.SmartClient.Overview.Implemented;
using Trex.SmartClient.Overview.Interfaces;
using Trex.SmartClient.Overview.OverviewScreen;
using Trex.SmartClient.Overview.WeeklyOverviewScreen;
using Microsoft.Practices.Prism.Modularity;
using Trex.SmartClient.Overview.WeeklyOverviewScreen.Viewmodels;

namespace Trex.SmartClient.Overview
{
    [Module(ModuleName = "OverviewModule")]
    public class OverviewModule : IModule
    {
        private readonly IMenuRegistry _menuRegistry;
        private readonly IScreenFactoryRegistry _screenFactoryRegistry;
        private readonly IAppSettings _appSettings;
        private readonly IUnityContainer _unityContainer;
        private SubMenuInfo _subMenuDailyOverview;
        private SubMenuInfo _subMenuWeekly;
        private MenuInfo taskAdminScreenInfo;

        public OverviewModule(IUnityContainer unityContainer, IMenuRegistry menuRegistry, IScreenFactoryRegistry screenFactoryRegistry, 
            IAppSettings appSettings)
        {
            _menuRegistry = menuRegistry;
            _screenFactoryRegistry = screenFactoryRegistry;
            _appSettings = appSettings;
            _unityContainer = unityContainer;

            _unityContainer.RegisterType<IOverviewScreenMasterviewModel, OverviewScreenMasterviewModel>();

            // Disabled for H&B
            //_unityContainer.RegisterType<IDayOverviewViewModel, DayOverviewViewModel>();

            _unityContainer.RegisterType<IWeeklyOverviewViewmodel, WeeklyOverviewViewmodel>();
            _unityContainer.RegisterType<IOverviewSwitcherService, OverviewSwitcherService>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<ITaskItemViewmodelFactory, TaskItemViewmodelFactory>();

        }

      

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
             taskAdminScreenInfo = MenuInfo.Create(1, "Overview", true, _appSettings.StartScreenIsWeekOverview, false, true);
            _screenFactoryRegistry.RegisterFactory(taskAdminScreenInfo.ScreenGuid, new OverviewScreenFactory(_unityContainer));

            _menuRegistry.RegisterMenuInfo(taskAdminScreenInfo);

            // Disabled for H&B
            //_subMenuDailyOverview = SubMenuInfo.Create("Daily", typeof(DayOverviewScreen.DayOverviewScreen).Name, taskAdminScreenInfo);
            //_subMenuDailyOverview.IsActive = !_appSettings.StartScreenIsWeekOverview;
            //taskAdminScreenInfo.AddSubMenu(_subMenuDailyOverview);

            _subMenuWeekly = SubMenuInfo.Create("Weekly", typeof(WeeklyOverviewScreen.WeeklyOverviewScreen), taskAdminScreenInfo);
            _subMenuWeekly.IsActive = _appSettings.StartScreenIsWeekOverview;
            taskAdminScreenInfo.AddSubMenu(_subMenuWeekly);

            var overview = _unityContainer.Resolve<IOverviewSwitcherService>();
            // Disabled for H&B
            //overview.AttachDailyOverviewSubmenu(_subMenuDailyOverview);
            overview.AttachWeeklyOverviewSubmenu(_subMenuWeekly);

            _unityContainer.Resolve<IDialogService>();
        }

    }
}