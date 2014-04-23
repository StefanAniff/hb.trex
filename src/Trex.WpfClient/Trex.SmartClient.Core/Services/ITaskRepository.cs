using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{
    public interface ITaskRepository
    {
        List<Task> GetAll();
        void AddOrUpdate(Task task);
        void AddOrUpdate(List<Task> tasks);
        Task GetByGuid(Guid guid);
        List<Task> GetNewTasks();

        bool Exists(Guid guid);
    }
}