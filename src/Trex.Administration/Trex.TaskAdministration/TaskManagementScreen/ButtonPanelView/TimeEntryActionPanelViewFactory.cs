using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView
{
    public class TimeEntryActionPanelViewFactory : IActionPanelViewFactory
    {
        private readonly TimeEntry _timeEntry;

        public TimeEntryActionPanelViewFactory(TimeEntry timeEntry)
        {
            _timeEntry = timeEntry;
        }

        #region IActionPanelViewFactory Members

        public IView CreateActionPanelView()
        {
            var timeEntryActionsViewModel = new TimeEntryActionsViewModel(_timeEntry);
            var timeEntryActionsView = new TimeEntryActionsView();
            timeEntryActionsView.ViewModel = timeEntryActionsViewModel;

            return timeEntryActionsView;
        }

        #endregion
    }
}