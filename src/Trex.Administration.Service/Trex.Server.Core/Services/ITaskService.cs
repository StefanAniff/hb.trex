using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public interface ITaskService
    {
        Task GetTaskById(int taskId, bool includeParents, bool includeSubTasks, bool includeTimeEntries);

        List<Task> EntityTaskRequest(List<int> taskIds, bool includeParents, bool includeSubTasks,
                                     bool includeTimeEntries);

        Task SaveTask(Task task);
        bool DeleteTask(Task task);
        List<Task> SearchTasks(string searchString);
    }
}
