using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface ITimeEntryTypeRepository
    {
        TimeEntryType GetById(int timeEntryTypeId);
        List<TimeEntryType> GetAllGlobal();
        List<TimeEntryType> GetAll();

        void Update(TimeEntryType timeEntryType);
        void Delete(TimeEntryType timeEntryType);
    }
}