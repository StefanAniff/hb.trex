using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.TaskAdministration.SearchView
{
    public class ProjectSearchResultViewModel
    {
        private readonly Project _project;

        public ProjectSearchResultViewModel(Project project)
        {
            _project = project;
        }

        public Project Project
        {
            get { return _project; }
        }

        public string ProjectName
        {
            get { return Project.ProjectName; }
        }

        public string CustomerName
        {
            get { return Project.Customer.CustomerName; }
        }

        public bool IsSelected { get; set; }
    }
}