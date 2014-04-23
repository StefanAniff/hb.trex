using System;
using System.Collections.Generic;
using System.Linq;
using Trex.Server.Core.Model;
using NHibernate;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TaskRepository : GenericRepository<Task>, ITaskRepository
    {

        public TaskRepository(ISession openSession)
            : base(openSession)
        {
        }

        public TaskRepository(IActiveSessionManager activeSessionManager)
            : base(activeSessionManager)
        {
        }

        /// <summary>
        /// Gets the task by GUID.
        /// </summary>
        public Task GetByGuid(Guid guid)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();
            var task = session.QueryOver<Task>().Where(tasks => tasks.Guid == guid);
            var returnTask = task.SingleOrDefault();

            if (returnTask == null)
                throw new NotFoundByIDException("Task not found by Guid: " + guid.ToString());
            return returnTask;
        }


        /// <summary>
        /// Checks if the task exists by the given guid
        /// </summary>
        public bool ExistsByGuid(Guid guid)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();
            var task = session.QueryOver<Task>().Where(x => x.Guid == guid).SingleOrDefault();
            return task != null;
        }


        public List<Task> GetByChangeDate(DateTime? startDate)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();
            Project project = null;
            Task task = null;
            Company company = null;
            User pUser = null;

            var queryOver = session.QueryOver(() => task)
                                   .Left.JoinAlias(() => task.CreatedBy, () => pUser)
                                   .Left.JoinAlias(() => task.Project, () => project)
                                   .Left.JoinAlias(() => project.Company, () => company);

            queryOver = queryOver.Where(x => !x.Inactive
                                        && !project.Inactive
                                        && !company.Inactive);

            if (startDate.HasValue)
            {
                queryOver = queryOver
                    .And(task1 => task1.CreateDate >= startDate
                                  || (task1.ChangeDate != null
                                      && task1.ChangeDate >= startDate));
            }


            return queryOver.List<Task>().ToList();
        }

        public List<Task> GetByIds(List<int> taskIds)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();
            Project project = null;
            Task task = null;
            Company company = null;
            User pUser = null;

            var queryOver = session.QueryOver(() => task)
                                   .Left.JoinAlias(() => task.CreatedBy, () => pUser)
                                   .Left.JoinAlias(() => task.Project, () => project)
                                   .Left.JoinAlias(() => project.Company, () => company);

            queryOver = queryOver.WhereRestrictionOn(x => x.TaskID).IsIn(taskIds);

            return queryOver.List<Task>().ToList();
        }
    }
}