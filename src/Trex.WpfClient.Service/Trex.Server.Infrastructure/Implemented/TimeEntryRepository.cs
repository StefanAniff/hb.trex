using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using Trex.Server.Core.Model;
using NHibernate;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TimeEntryRepository : GenericRepository<TimeEntry>, ITimeEntryRepository
    {

        public TimeEntryRepository(ISession openSession)
            : base(openSession)
        {
        }

        public TimeEntryRepository(IActiveSessionManager activeSessionManager)
            : base(activeSessionManager)
        {
        }

        public TimeEntry GetByGuid(Guid id)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();
            var timeEntry = session.QueryOver<TimeEntry>().Where(te => te.Guid == id);
            return timeEntry.SingleOrDefault();
        }

        public bool Exists(Guid id)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();
            var timeEntry = session.QueryOver<TimeEntry>().Where(te => te.Guid == id).List();

            return timeEntry.Any();
        }

        public IEnumerable<TimeEntry> GetTimeEntriesByPeriodAndUser(int userId, DateTime startDate, DateTime endDate)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();

            //include the whole enddate
            var realEnddate = endDate.Date.AddDays(1);
            var timeEntries = session.QueryOver<TimeEntry>()
                                     .Where(te => te.StartTime >= startDate.Date
                                         && te.StartTime < realEnddate
                                                  && te.User.UserID == userId);
            return timeEntries.List();
        }

        public IEnumerable<TimeEntry> GetInactiveTimeEntriesByPeriodAndUser(int userId, DateTime startDate, DateTime endDate)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();

            Task pTask = null;
            Project pProject = null;
            Company pCompany = null;

            //include the whole enddate
            var realEnddate = endDate.Date.AddDays(1);
            var inactiveTimeEntries = session.QueryOver<TimeEntry>()
                                     .Left.JoinAlias(t => t.Task, () => pTask)
                                     .Left.JoinAlias(t => pTask.Project, () => pProject)
                                     .Left.JoinAlias(t => pProject.Company, () => pCompany)
                                     .Where(te => te.StartTime >= startDate.Date && te.StartTime < realEnddate
                                                  && te.User.UserID == userId
                                                  && (pTask.Inactive
                                                      || pProject.Inactive
                                                      || pCompany.Inactive));
            return inactiveTimeEntries.List();
        }


        public double GetBillableHours(DateTime startDate, DateTime endDate, int userId)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();

            Task tTask = null;
            var hours = session.QueryOver<TimeEntry>()
                               .Left.JoinAlias(t => t.Task, () => tTask)
                               .Where(te => te.StartTime >= startDate
                                            && te.StartTime < endDate.Date.AddDays(1)
                                            && te.User.UserID == userId && te.Billable
                                                 && tTask.TimeRegistrationTypeId != (int) TimeRegistrationTypeEnum.Projection)
                               .Select(Projections.Sum<TimeEntry>(x => x.BillableTime))
                               .SingleOrDefault<double>();

            return hours;
        }

        /// <summary>
        /// Returns all active timeentries.
        /// Active task, project and customer.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<TimeEntry> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();

            Task pTask = null;
            Project pProject = null;
            Company pCompany = null;
            User pUser = null;

            var timeEntries = session.QueryOver<TimeEntry>()
                                     .Left.JoinAlias(t => t.Task, () => pTask)
                                     .Left.JoinAlias(t => pTask.Project, () => pProject)
                                     .Left.JoinAlias(t => pProject.Company, () => pCompany)
                                     .Left.JoinAlias(t => t.User, () => pUser)
                                     .Where(te => te.StartTime >= startDate.Date
                                                  && pTask.Inactive == false
                                                  && pProject.Inactive == false
                                                  && pCompany.Inactive == false);
            return timeEntries.List();
        }

        /// <summary>
        /// Returns all timeentries for a given period, disregarding if relating items are inactive
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<TimeEntry> GetAllTimeEntriesByPeriod(DateTime startDate, DateTime endDate)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();

            Task pTask = null;
            Project pProject = null;
            Company pCompany = null;
            User pUser = null;

            var timeEntries = session.QueryOver<TimeEntry>()
                                     .Left.JoinAlias(t => t.Task, () => pTask)
                                     .Left.JoinAlias(t => pTask.Project, () => pProject)
                                     .Left.JoinAlias(t => pProject.Company, () => pCompany)
                                     .Left.JoinAlias(t => t.User, () => pUser)
                                     .Where(te => te.StartTime >= startDate.Date
                                                  && te.StartTime <= endDate);
            return timeEntries.List();
        }
    

        public double GetRegisteredHours(DateTime startDate, DateTime endDate, int userId)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();

            Task tTask = null;
            var hours = session.QueryOver<TimeEntry>()
                               .Left.JoinAlias(t => t.Task, () => tTask)
                               .Where(te => te.StartTime >= startDate
                                            && te.StartTime < endDate.Date.AddDays(1)
                                            && te.User.UserID == userId
                                            && tTask.TimeRegistrationTypeId != (int)TimeRegistrationTypeEnum.Projection)
                               .Select(Projections.Sum<TimeEntry>(x => x.TimeSpent))
                               .SingleOrDefault<double>();

            return hours;
        }

        public double GetEarningsByUser(DateTime startDate, DateTime endDate, int userId)
        {
            var session = OpenSession ?? ActiveSessionManager.GetActiveSession();

            //User pUser = null;
            var billableTimeSum = session.QueryOver<TimeEntry>()
                                         .Where(te => te.StartTime >= startDate
                                                      && te.StartTime < endDate.Date.AddDays(1)
                                                      && te.User.UserID == userId && te.Billable)
                                         .Select(Projections.Sum<TimeEntry>(x => x.BillableTime))
                                         .SingleOrDefault<double>();

            var priceSum = session.QueryOver<TimeEntry>()
                                  .Where(te => te.StartTime >= startDate
                                               && te.StartTime < endDate.Date.AddDays(1)
                                               && te.User.UserID == userId && te.Billable)
                                  .Select(Projections.Sum<TimeEntry>(x => x.Price))
                                  .SingleOrDefault<double>();
            return billableTimeSum * priceSum;
        }
    }
}