using System.Collections.Generic;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{
    public interface ITimeEntryRepository
    {
        void AddOrUpdate(TimeEntry timeEntry);
        bool Exists(TimeEntry timeEntry);
        List<TimeEntry> GetUnsyncedTimeEntries();
        void DeleteAllRepositories();
        void AddOrUpdateRange(List<TimeEntry> timeEntries);
        void RemoveTimeEntiresWithErrors();
    }
}