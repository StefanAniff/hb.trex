using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using StructureMap;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TaskService : LogableBase, ITaskService
    {
        private readonly ITrexContextProvider _entityContext;
        public TaskService(ITrexContextProvider contextProvider)
        {
            _entityContext = contextProvider;
        }

        public Task SaveTask(Task task)
        {
            task.ChangeTracker.ChangeTrackingEnabled = true;
            using (var db = _entityContext.TrexEntityContext)
            {
                task.ModifyDate = DateTime.Now;
                db.Tasks.ApplyChanges(task);
                db.SaveChanges();
            }
            return task;
        }

        public bool DeleteTask(Task task)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                db.Tasks.Attach(task);

                if ((task.TimeEntries == null) || task.TimeEntries != null && task.TimeEntries.Count == 0)
                {
                    db.Tasks.DeleteObject(task);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }


        }

        public List<Task> SearchTasks(string searchString)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                var searchTasks = (from task in db.Tasks
                                   select new
                                   {
                                       Task = task,
                                       SearchString = task.TaskName + " " + task.Project.ProjectName + " " + task.Project.Customer.CustomerName
                                   });

                var searchArray = searchString.Split(" ".ToCharArray());
                searchArray = searchArray.Where(s => !String.IsNullOrEmpty(s)).ToArray();

                var result = new List<Task>();
                var query = from t in searchTasks select t;

                searchArray.ToList().ForEach(s => query = query.Where(st => st.SearchString.ToLower().Contains(s.ToLower())));

                result.AddRange(query.Take(10).ToList().Select(t => t.Task));

                foreach (var task in result)
                {
                    db.LoadProperty(task, "Project");
                    db.LoadProperty(task.Project, "Customer");
                }

                return result;

            }
        }

        public Task GetTaskById(int taskId, bool includeParents, bool includeSubTasks, bool includeTimeEntries)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                var task = db.Tasks;

                if (includeTimeEntries)
                    task.Include("TimeEntries");

                return task.SingleOrDefault(t => t.TaskID == taskId);

            }
        }

        public List<Task> EntityTaskRequest(List<int> taskIds, bool includeParents, bool includeSubTasks, bool includeTimeEntries)
        {
            var taskList = new List<Task>();

            foreach (var taskId in taskIds)
            {
                var task = GetTaskById(taskId, includeParents, includeSubTasks, includeTimeEntries);

                taskList.Add(task);
            }
            return taskList.OrderBy(t => t.TaskName).ToList();
        }
    }
}
