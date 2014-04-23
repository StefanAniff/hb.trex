using System;
using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface ITimeEntryRepository : IRepository<TimeEntry>
    {
        TimeEntry GetByGuid(Guid id);
        bool Exists(Guid id);
        double GetRegisteredHours(DateTime startDate, DateTime endDate, int userId);
        double GetEarningsByUser(DateTime startDate, DateTime endDate, int userId);
        double GetBillableHours(DateTime startDate, DateTime endDate, int userId);

        IEnumerable<TimeEntry> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate);
        IEnumerable<TimeEntry> GetTimeEntriesByPeriodAndUser(int userId, DateTime startDate, DateTime endDate);
        IEnumerable<TimeEntry> GetInactiveTimeEntriesByPeriodAndUser(int userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Returns all timeentries for a given period, disregarding if relating items are inactive
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<TimeEntry> GetAllTimeEntriesByPeriod(DateTime startDate, DateTime endDate);
    }
}
