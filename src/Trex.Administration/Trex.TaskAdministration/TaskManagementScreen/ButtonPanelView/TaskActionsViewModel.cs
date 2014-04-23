using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView
{
    public class TaskActionsViewModel : ViewModelBase
    {
        private readonly Task _task;

        public TaskActionsViewModel(Task task)
        {
            _task = task;
            EditTask = new DelegateCommand<object>(ExecuteEditTask, CanEditTask);
            DeleteTask = new DelegateCommand<object>(ExecuteDeleteTask, CanDeleteTask);
            AddTimeEntry = new DelegateCommand<object>(ExecuteAddTimeEntry, CanAddTimeEntry);
        }

        public DelegateCommand<object> EditTask { get; set; }
        public DelegateCommand<object> DeleteTask { get; set; }
        public DelegateCommand<object> AddTimeEntry { get; set; }

        private bool CanAddTimeEntry(object arg)
        {
            return !_task.Inactive;
        }

        private void ExecuteAddTimeEntry(object obj)
        {
            InternalCommands.TimeEntryAddStart.Execute(_task);
        }

        private void ExecuteDeleteTask(object obj)
        {
            InternalCommands.TaskDeleteStart.Execute(_task);
        }

        private bool CanDeleteTask(object arg)
        {
            return _task.TimeEntries.Count == 0;
        }

        private bool CanEditTask(object arg)
        {
            return true;
        }

        private void ExecuteEditTask(object obj)
        {
            InternalCommands.TaskEditStart.Execute(_task);
        }
    }
}