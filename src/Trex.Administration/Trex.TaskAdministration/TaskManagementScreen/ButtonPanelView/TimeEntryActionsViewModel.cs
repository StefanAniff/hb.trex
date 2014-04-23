using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView
{
    public class TimeEntryActionsViewModel : ViewModelBase
    {
        private readonly TimeEntry _timeEntry;

        public TimeEntryActionsViewModel(TimeEntry timeEntry)
        {
            _timeEntry = timeEntry;

            EditTimeEntry = new DelegateCommand<object>(ExecuteEditTimeEntry, CanEditTimeEntry);
            DeleteTimeEntry = new DelegateCommand<object>(ExecuteDeleteTimeEntry, CanDeleteTimeEntry);
        }

        public TimeEntry TimeEntry
        {
            get { return _timeEntry; }
        }

        public DelegateCommand<object> EditTimeEntry { get; set; }
        public DelegateCommand<object> DeleteTimeEntry { get; set; }

        private bool CanDeleteTimeEntry(object arg)
        {
            return _timeEntry.InvoiceId.HasValue == false;
        }

        private void ExecuteDeleteTimeEntry(object obj)
        {
            InternalCommands.TimeEntryDeleteStart.Execute(_timeEntry);
        }

        private bool CanEditTimeEntry(object arg)
        {
            return _timeEntry.InvoiceId.HasValue == false;
        }

        private void ExecuteEditTimeEntry(object obj)
        {
            InternalCommands.TimeEntryEditStart.Execute(_timeEntry);
        }
    }
}