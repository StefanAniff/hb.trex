using Trex.SmartClient.Core.Model;

namespace Trex.Dialog.SelectTask.Viewmodels.Itemviewmodel
{
    public class TaskListViewModel : TaskListViewModelbase
    {
        private readonly Task _task;

        public TaskListViewModel(Task task)
        {
            _task = task;
            TaskName = task.Name;
            ProjectName = task.Project.Name;
            CustomerName = task.Project.Company.Name;
        }

        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public Task Task { get { return _task; } }
    }
}
