using Trex.Core.Implemented;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.TaskAdministration.SearchView
{
    public class TaskSearchResultViewModel : ViewModelBase
    {
        private readonly Task _task;

        public TaskSearchResultViewModel(Task task)
        {
            _task = task;
        }

        public Task Task
        {
            get { return _task; }
        }

        public string TaskName
        {
            get { return _task.TaskName; }
        }

        public string ProjectName
        {
            get { return _task.Project.ProjectName; }
        }

        public string CustomerName
        {
            get { return _task.Project.Customer.CustomerName; }
        }

        public bool IsSelected { get; set; }
    }
}