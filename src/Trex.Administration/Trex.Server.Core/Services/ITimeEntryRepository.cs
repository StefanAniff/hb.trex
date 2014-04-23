using System;
using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface ITimeEntryRepository
    {
        TimeEntry GetById(int id);
        TimeEntry GetByGuid(Guid id);
        bool Exists(Guid id);
        void Update(TimeEntry entry);
        void Delete(TimeEntry entry);
        void Delete(int entryId);
        void Save(TimeEntry timeEntry);
        IList<TimeEntry> GetNotBilledTimeEntries(int customerId, DateTime startDate, DateTime endDate);
        IList<TimeEntry> GetTimeEntriesByPeriodAndCustomer(int customerId, DateTime startDate, DateTime endDate);
        IList<TimeEntry> GetTimeEntriesByPeriodAndInvoice(int invoiceId, int customerId, DateTime startDate, DateTime endDate, IComparer<TimeEntry> timeEntry);
        IList<TimeEntry> GetTimeEntriesByPeriodAndUser(int userId, DateTime startDate, DateTime endDate);
        IList<TimeEntry> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate);
        double GetRegisteredHours(DateTime startDate, DateTime endDate, int userId);
        double GetEarningsByUser(DateTime startDate, DateTime endDate, int userId);
        double GetBillableHours(DateTime startDate, DateTime endDate, int userId);
    }
}