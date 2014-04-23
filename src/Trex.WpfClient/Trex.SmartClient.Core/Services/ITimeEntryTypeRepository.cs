using System.Collections.Generic;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Services
{
    public interface ITimeEntryTypeRepository
    {
        void Add(IEnumerable<TimeEntryType> timeEntryTypes);
        TimeEntryType GetById(int id);
        List<TimeEntryType> GetByCompany(Company company);
        List<TimeEntryType> GetGlobal();
    }
}