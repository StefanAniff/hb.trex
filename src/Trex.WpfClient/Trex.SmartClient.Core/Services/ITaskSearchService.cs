using System.Collections.Generic;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{
    public interface ITaskSearchService
    {
        System.Threading.Tasks.Task<List<Task>> SearchTasks(string searchString);
        List<Project> SearchProjects(string searchString);
        List<Task> GetAll();
    }
}
