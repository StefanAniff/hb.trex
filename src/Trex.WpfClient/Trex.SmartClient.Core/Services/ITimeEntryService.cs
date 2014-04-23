using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trex.SmartClient.Core.Model;
using Task = System.Threading.Tasks.Task;

namespace Trex.SmartClient.Core.Services
{
    public interface ITimeEntryService
    {
        Task SaveNewTimeEntries(DateTime lastSyncDate);
        Task<List<TimeEntry>> GetTimeEntriesByDate(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Useful in report overviews
        /// </summary>
       Task<List<TimeEntry>> GetTimeEntriesByDateIgnoreEmptyTimeEntries(DateTime startTime, DateTime endTime);
    }
}
