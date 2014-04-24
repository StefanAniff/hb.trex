using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Overview.DayOverviewScreen.Viewmodels;
using Trex.SmartClient.Overview.WeeklyOverviewScreen;

namespace Trex.SmartClient.Overview.OverviewScreen
{
    public class OverviewScreenFactory : IScreenFactory
    {
        private readonly IUnityContainer _unityContainer;
        private readonly IRegionNames _regionNames;

        public OverviewScreenFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
            _regionNames = _unityContainer.Resolve<IRegionNames>(); ;
        }

        public IScreen CreateScreen(IRegion region, Guid guid)
        {
            var reportScreen = new OverviewScreen(guid, _unityContainer);
            reportScreen.InitializeScreen(region, guid);

            // Disabled for H&B
            //var dailyOverview = new DayOverviewScreen.DayOverviewScreen();
            //var desktopPanelViewModel = _unityContainer.Resolve<IDayOverviewViewModel>();
            //dailyOverview.ApplyViewModel(desktopPanelViewModel);

            var weeklyOverview = new WeeklyOverviewScreen.WeeklyOverviewScreen();
            var weeklyOverviewViewModel = _unityContainer.Resolve<IWeeklyOverviewViewmodel>();
            weeklyOverview.ApplyViewModel(weeklyOverviewViewModel);

            var desktopRegion = _regionNames.SubmenuRegion;
            // Disabled for H&B
            //reportScreen.AddRegion(desktopRegion, dailyOverview);
            reportScreen.AddRegion(desktopRegion, weeklyOverview);

            return reportScreen;
        }
    }
}