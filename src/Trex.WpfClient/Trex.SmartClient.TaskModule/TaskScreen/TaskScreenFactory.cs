using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.TaskModule.TaskScreen
{
    class TaskScreenFactory:IScreenFactory
    {
        private readonly IUnityContainer _unityContainer;
        private readonly IUserSession _userSession;

        public TaskScreenFactory(IUnityContainer unityContainer, IUserSession userSession)
        {
            _userSession = userSession;
            _unityContainer = unityContainer;
        }

        public IScreen CreateScreen(IRegion region, Guid guid)
        {
            var syncService = _unityContainer.Resolve<ISyncService>();
            var timeEntryRepository = _unityContainer.Resolve<ITimeEntryRepository>();
            var connectivityService = _unityContainer.Resolve<IConnectivityService>();


            var screen = new TaskScreen(_unityContainer, guid);

            var localRegionManager = region.Add(screen.MasterView, guid.ToString(),true);

            var historyView = new HistoryFeedView.HistoryFeedView();

            var historyViewModel = _unityContainer.Resolve<IHistoryFeedViewModel>();

            historyView.ApplyViewModel(historyViewModel);

            localRegionManager.Regions["HistoryRegion"].Add(historyView);



            return screen;
        }
    }
}
