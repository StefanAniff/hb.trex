using System;
using System.Collections.Generic;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{
    public interface ITaskService
    {
        System.Threading.Tasks.Task<List<Task>> GetUnsynced(DateTime lastSyncDate);
        System.Threading.Tasks.Task SaveNewTasks();
    }
}