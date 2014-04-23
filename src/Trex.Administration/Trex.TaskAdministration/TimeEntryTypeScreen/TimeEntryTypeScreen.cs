using System;
using Microsoft.Practices.Unity;
using Trex.Core.Attributes;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.TaskAdministration.TimeEntryTypeScreen.TimeEntryTypeListView;


namespace Trex.TaskAdministration.TimeEntryTypeScreen
{
    [Screen(Name = "TimeEntryTypeScreen", CanBeDeactivated = true)]
    public class TimeEntryTypeScreen:ScreenBase 
    {

        public TimeEntryTypeScreen(IUnityContainer unityContainer, Guid guid) : base(guid,unityContainer)
        {
            
            
        }

        protected override void Initialize()
        {
            var dataService = Container.Resolve<IDataService>();

            var timeEntryTypeView = new TimeEntryTypeListView.TimeEntryTypeListView();
            var timeEntryTypeViewModel = new TimeEntryTypeListViewModel(dataService);

            timeEntryTypeView.ViewModel = timeEntryTypeViewModel;

            MasterView = timeEntryTypeView;
        }
    }
}