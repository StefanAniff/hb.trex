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
    public class TimeEntryRepository : RepositoryBase, ITimeEntryRepository
    {
        #region ITimeEntryRepository Members

        public TimeEntry GetById(int id)
        {
            var session = GetSession();
            return session.Get<TimeEntry>(id);
        }

        public TimeEntry GetByGuid(Guid id)
        {
            var session = GetSession();
            var timeEntry = from te in session.Linq<TimeEntry>()
                            where te.Guid == id
                            select te;
            return timeEntry.Single();
        }

        public bool Exists(Guid id)
        {
            var session = GetSession();
            var timeEntry = from te in session.Linq<TimeEntry>()
                            where te.Guid == id
                            select te;

            return timeEntry.Count() > 0;
        }

        public void Update(TimeEntry entry)
        {
            var session = GetSession();
            try
            {
                entry.ChangeDate = DateTime.Now;
                session.Transaction.Begin();
                session.Update(entry);
                session.Flush();
                session.Transaction.Commit();
            }
            catch (HibernateException ex)
            {
                session.Transaction.Rollback();
                throw new EntityUpdateException("Error updating TimeEntry", ex);
            }
        }

        public void Delete(TimeEntry entry)
        {
            var session = GetSession();
            try
            {
                session.Transaction.Begin();
                session.Delete(entry);
                session.Flush();
                session.Transaction.Commit();
            }
            catch (HibernateException ex)
            {
                session.Transaction.Rollback();
                throw new EntityDeleteException("Error deleting TimeEntry", ex);
            }
        }

        public void Delete(int entryId)
        {
            Delete(GetById(entryId));
        }

        public void Save(TimeEntry timeEntry)
        {
            try
            {
                var session = GetSession();
                session.SaveOrUpdate(timeEntry);
                session.Flush();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IList<TimeEntry> GetNotBilledTimeEntries(int customerId, DateTime startDate, DateTime endDate)
        {
            var session = GetSession();

            var timeEntries = from te in session.Linq<TimeEntry>()
                              where te.Invoice == null
                                    && te.Billable
                                    && te.StartTime >= startDate.Date && te.StartTime <= endDate.Date.AddDays(1).AddSeconds(-1)
                                    && te.Task.Project.Customer.Id == customerId
                              select te;

            return timeEntries.ToList();
        }

        public IList<TimeEntry> GetTimeEntriesByPeriodAndCustomer(int customerId, DateTime startDate, DateTime endDate)
        {
            var session = GetSession();

            var timeEntries = from te in session.Linq<TimeEntry>()
                              where te.Billable
                                    && te.StartTime >= startDate.Date && te.StartTime <= endDate.Date.AddDays(1).AddSeconds(-1)
                                    && te.Task.Project.Customer.Id == customerId
                              select te;

            return timeEntries.ToList();
        }

        public IList<TimeEntry> GetTimeEntriesByPeriodAndInvoice(int invoiceId, int customerId, DateTime startDate, DateTime endDate, IComparer<TimeEntry> comparer)
        {
            var session = GetSession();

            var timeEntries = from te in session.Linq<TimeEntry>()
                              where (te.Billable
                                     && te.StartTime >= startDate.Date && te.StartTime <= endDate.Date.AddDays(1).AddSeconds(-1)
                                     && te.Invoice.ID == invoiceId)
                              select te;

            var entries = timeEntries.ToList().Union(
                from te in session.Linq<TimeEntry>()
                where te.Invoice == null
                      && te.Billable
                      && te.StartTime >= startDate.Date && te.StartTime <= endDate.Date.AddDays(1).AddSeconds(-1)
                      && te.Task.Project.Customer.Id == customerId
                select te
                ).ToList();

            entries.Sort(comparer);
            return entries;
        }

        public IList<TimeEntry> GetTimeEntriesByPeriodAndUser(int userId, DateTime startDate, DateTime endDate)
        {
            var session = GetSession();

            try
            {
                var timeEntries = from te in session.Linq<TimeEntry>()
                                  where te.StartTime >= startDate.Date && te.StartTime <= endDate.Date.AddDays(1).AddSeconds(-1)
                                        && te.User.Id == userId
                                  select te;
                return timeEntries.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IList<TimeEntry> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate)
        {
            var session = GetSession();

            try
            {
                var timeEntries = from te in session.Linq<TimeEntry>()
                                  where te.StartTime >= startDate.Date && te.StartTime <= endDate.Date.AddDays(1).AddSeconds(-1)
                                  select te;
                return timeEntries.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public double GetBillableHours(DateTime startDate, DateTime endDate, int userId)
        {
            var session = GetSession();

            var hours = (from te in session.Linq<TimeEntry>()
                         where te.StartTime >= startDate && te.StartTime <= endDate.Date.AddDays(1).AddSeconds(-1)
                               && te.User.Id == userId && te.Billable
                         select te.BillableTime);

            var tmpHours = hours.ToList();
            return tmpHours.Sum();
        }

        public double GetRegisteredHours(DateTime startDate, DateTime endDate, int userId)
        {
            var session = GetSession();

            var hours = (from te in session.Linq<TimeEntry>()
                         where te.StartTime >= startDate && te.StartTime <= endDate.Date.AddDays(1).AddSeconds(-1)
                               && te.User.Id == userId
                         select te.TimeSpent);

            var tmpHours = hours.ToList();
            return tmpHours.Sum();
        }

        public double GetEarningsByUser(DateTime startDate, DateTime endDate, int userId)
        {
            var session = GetSession();

            var hours = (from te in session.Linq<TimeEntry>()
                         where te.StartTime >= startDate && te.StartTime <= endDate.Date.AddDays(1).AddSeconds(-1)
                               && te.User.Id == userId && te.Billable
                         select te.BillableTime*te.Price);

            var tmpHours = hours.ToList();
            return tmpHours.Sum();
        }

        #endregion
    }
}