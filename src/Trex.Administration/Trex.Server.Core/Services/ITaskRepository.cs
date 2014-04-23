using System;
using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface ITaskRepository
    {
        void Update(Task task);
        void Save(List<Task> tasks);
        void Delete(int taskId);
        void Delete(Task task);
        Task GetById(int id);
        Task GetByGuid(Guid guid);
        bool ExistsByGuid(Guid guid);
        IList<Task> GetLastCreatedTasks(int numOfTasks);
        IList<Task> GetLastTasksWithTimeEntriesByUser(int numOfTasks, User user);
        IList<Task> SearchTasksBySearchString(string searchString);
        IList<Task> GetByChangeDate(DateTime startDate);
    }
}