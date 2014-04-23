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
    public class TimeEntryTypeRepository : RepositoryBase, ITimeEntryTypeRepository
    {
        #region ITimeEntryTypeRepository Members

        public TimeEntryType GetById(int timeEntryTypeId)
        {
            var session = GetSession();

            return session.Load<TimeEntryType>(timeEntryTypeId);
        }

        public List<TimeEntryType> GetAllGlobal()
        {
            var session = GetSession();
            try
            {
                var timeEntryTypes = from timeEntryType in session.Linq<TimeEntryType>()
                                     where timeEntryType.Customer == null
                                     select timeEntryType;
                return timeEntryTypes.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<TimeEntryType> GetAll()
        {
            var session = GetSession();
            try
            {
                return session.Linq<TimeEntryType>().ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Update(TimeEntryType timeEntryType)
        {
            var session = GetSession();
            session.Transaction.Begin();
            try
            {
                session.SaveOrUpdate(timeEntryType);
                session.Flush();
                session.Transaction.Commit();
            }
            catch (HibernateException ex)
            {
                session.Transaction.Rollback();
                throw new EntityUpdateException("Error updating TimeEntryType", ex);
            }
        }

        public void Delete(TimeEntryType timeEntryType)
        {
            var session = GetSession();
            try
            {
                session.Transaction.Begin();
                session.Delete(timeEntryType);
                session.Flush();
                session.Transaction.Commit();
            }
            catch (HibernateException ex)
            {
                session.Transaction.Rollback();
                throw new EntityDeleteException("Error deleting TimeEntryType", ex);
            }
        }

        #endregion
    }
}