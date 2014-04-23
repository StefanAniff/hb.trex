using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TaskRepository : RepositoryBase, ITaskRepository
    {
        #region ITaskRepository Members

        /// <summary>
        /// Updates the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <exception cref="EntityUpdateException"></exception>
        public void Update(Task task)
        {
            var session = GetSession();

            try
            {
                task.ChangeDate = DateTime.Now;
                session.Transaction.Begin();
                session.SaveOrUpdate(task);
                session.Flush();
                session.Transaction.Commit();
            }
            catch (HibernateException ex)
            {
                session.Transaction.Rollback();
                throw new EntityUpdateException("Error updating Task", ex);
            }
        }

        public void Save(List<Task> tasks)
        {
            var session = GetSession();

            foreach (var task in tasks)
            {
                task.Project.Tasks.Add(task);
                session.Save(task.Project);
            }
            session.Flush();
        }

        public void Delete(Task task)
        {
            var session = GetSession();
            try
            {
                session.Transaction.Begin();
                session.Delete(task);
                session.Flush();
                session.Transaction.Commit();
            }
            catch (HibernateException ex)
            {
                session.Transaction.Rollback();
                throw new EntityDeleteException("Error deleting Task", ex);
            }
        }

        public void Delete(int taskId)
        {
            Delete(GetById(taskId));
        }

        /// <summary>
        /// Gets a task the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public Task GetById(int id)
        {
            var session = GetSession();

            return session.Get<Task>(id);
        }

        /// <summary>
        /// Gets the task by GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        public Task GetByGuid(Guid guid)
        {
            var session = GetSession();

            var task = from tasks in session.Linq<Task>()
                       where tasks.Guid == guid
                       select tasks;

            var returnTask = task.SingleOrDefault();

            if (returnTask == null)
            {
                throw new NotFoundByIDException("Task not found by Guid: " + guid.ToString());
            }

            return returnTask;
        }

        /// <summary>
        /// Checks if the task exists by the given guid
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        public bool ExistsByGuid(Guid guid)
        {
            var session = GetSession();

            var task = session.Linq<Task>().SingleOrDefault(t => t.Guid == guid);
            return task != null;
        }

        /// <summary>
        /// Gets the last created tasks.
        /// </summary>
        /// <param name="numOfTasks">The num of tasks to fetch</param>
        /// <returns></returns>
        public IList<Task> GetLastCreatedTasks(int numOfTasks)
        {
            var session = GetSession();

            var tasks = (from t in session.Linq<Task>()
                         orderby t.CreateDate descending
                         select t).Take(numOfTasks);

            return tasks.ToList();
        }

        public IList<Task> GetLastTasksWithTimeEntriesByUser(int numOfTasks, User user)
        {
            var session = GetSession();

            var allTasks = from t in session.Linq<Task>()
                           orderby t.CreateDate descending
                           select t;

            IList<Task> allTasksList = allTasks.ToList();

            var usersTasks = from at in allTasksList
                             where at.TimeEntries.Count(te => te.User.Id == user.Id) > 0
                             select at;

            return usersTasks.Take(numOfTasks).ToList<Task>();
        }

        public IList<Task> GetByChangeDate(DateTime startDate)
        {
            var session = GetSession();

            var tasks = (from t in session.Linq<Task>()
                         where t.CreateDate >= startDate || (t.ChangeDate != null && t.ChangeDate >= startDate)
                         select t
                        );

            return tasks.ToList();
        }

        public IList<Task> SearchTasksBySearchString(string searchString)
        {
            //int searchId = 0;
            //ISession session = GetSession();
            ////Search by ID
            //if (int.TryParse(searchString, out searchId))
            //{
            //    List<Task> tasks = new List<Task>();
            //    Task task = session.Get<Task>(searchId);

            //    if (task != null)
            //        tasks.Add(task);
            //    return tasks;
            //}
            //else
            //{
            //    var allTasks = from t in session.Linq<Task>()
            //                   //where t.Name.ToLower().Contains(searchString.ToLower())
            //                   select t;

            //    //HACK: Because the linq statement fails
            //    IList<Task> allTasksList = allTasks.ToList<Task>();

            //    var tasks = from ts in allTasksList
            //                where ts.Name.ToLower().Contains(searchString.ToLower())
            //                select ts;

            //    return tasks.ToList<Task>();
            //}
            var session = GetSession();

            IList<Task> allTasks = session.Linq<Task>().ToList();

            var searchTasks =
                from task in allTasks
                select
                    new
                        {
                            Task = task,
                            SearchString = string.Concat(task.Name, " ", task.Project.Name, " ", task.Project.Customer.Name)
                        };

            var searchArray = searchString.Split(" ".ToCharArray());
            searchArray = searchArray.Where(s => !String.IsNullOrEmpty(s)).ToArray();
            var result = new List<Task>();
            var query = from t in searchTasks select t;

            //foreach (var s in searchArray)
            //{
            searchArray.ToList().ForEach(s =>
                                         query = query.Where(st => st.SearchString.ToLower().Contains(s.ToLower())));

            //query = query.Where(st => st.SearchString.ToLower().Contains(s.ToLower()));

            //}

            result.AddRange(query.ToList().Select(t => t.Task));
            return result.Take(10).ToList();
        }

        #endregion
    }
}