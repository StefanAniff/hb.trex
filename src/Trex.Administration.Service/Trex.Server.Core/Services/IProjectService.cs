using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public interface IProjectService
    {
        Project GetProjectById(int projectId, bool includeParents, bool includeInactive, bool includeTasks,
                               bool includeTimeEntries);

        Project SaveProject(Project project);
        bool DeleteProject(Project project);
        List<Project> SearchProjects(string searchString);

        List<Project> EntityProjectRequest(List<int> projectIds, bool includeParents, bool includeInactive,
                                           bool includeTasks, bool includeTimeEntries);
    }
}
