using System.Collections.Generic;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class ProjectListEventArgs : System.EventArgs
    {
        public ProjectListEventArgs(List<Project> projects)
        {
            if (projects != null)
            {
                Projects = projects;
            }
            else
            {
                Projects = new List<Project>();
            }
        }

        public List<Project> Projects { get; set; }
    }
}