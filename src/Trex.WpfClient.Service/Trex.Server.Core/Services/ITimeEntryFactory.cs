using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface ITimeEntryFactory
    {
        TimeEntry Create(Guid guid, User user, Task task, TimeEntryType timeEntryType,
                                             DateTime startTime, DateTime endTime, string description, double timeSpent,
                                             double pauseTime, double billableTime, bool billable, double price, int clientSourceId);
    }
}
