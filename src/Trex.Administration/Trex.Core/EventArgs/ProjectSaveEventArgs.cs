using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class ProjectSaveEventArgs : System.EventArgs
    {
        public Project Project;

        public ProjectSaveEventArgs(Project project)
        {
            Project = project;
        }
    }
}