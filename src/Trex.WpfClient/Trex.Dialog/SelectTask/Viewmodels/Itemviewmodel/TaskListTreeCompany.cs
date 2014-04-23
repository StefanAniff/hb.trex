using System.Collections.Generic;

namespace Trex.Dialog.SelectTask.Viewmodels.Itemviewmodel
{
    public class TaskListTreeCompany
    {
        public string CompanyName { get; set; }
        public List<TaskListTreeProject> Projects { get; set; }
    }

    public class TaskListTreeProject
    {
        public string ProjectName { get; set; }
        public List<TaskListViewModel> Tasks { get; set; }
    }
}
