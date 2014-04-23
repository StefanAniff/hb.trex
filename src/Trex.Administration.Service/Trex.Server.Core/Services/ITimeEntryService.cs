using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public interface ITimeEntryService
    {
        List<TimeEntry> GetTimeEntriesByPeriodAndUser(User user, DateTime startDate, DateTime endDate);
        List<TimeEntryView> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate);
        TimeEntry GetTimeEntryByTimeEntry(TimeEntry timeEntry);
        TimeEntry SaveTimeEntry(TimeEntry timeEntry);
        void ExcludeTimeEntry(TimeEntry timeEntry);
        bool DeleteTimeEntry(TimeEntry timeEntry);
        List<TimeEntryType> GetAllTimeEntryTypes();
        TimeEntryType SaveTimeEntryType(TimeEntryType timeEntryType);
        List<TimeEntryType> GetGlobalTimeEntryTypes();
        ServerResponse UpdateTimeEntryPrice(int projectId);
    }
}
