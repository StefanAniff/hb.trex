using System;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented.Factories
{
    public class TimeEntryFactory : ITimeEntryFactory
    {
        #region ITimeEntryFactory Members

        public TimeEntry Create(User user, Task task, TimeEntryType timeEntryType, DateTime startTime, DateTime endTime, string description, double timeSpent, double pauseTime, double billableTime,
                                bool billable, double price)
        {
            return Create(Guid.NewGuid(), user, task, timeEntryType, startTime, endTime, description, timeSpent, pauseTime,
                          billableTime, billable, price);
        }

        public TimeEntry Create(Guid guid, User user, Task task, TimeEntryType timeEntryType, DateTime startTime, DateTime endTime, string description, double timeSpent, double pauseTime, double billableTime,
                                bool billable, double price)
        {
            if (task == null)
            {
                throw new ParameterNullOrEmptyException("Task cannot be null");
            }

            if (user == null)
            {
                throw new ParameterNullOrEmptyException("User createdBy cannot be null");
            }

            return new TimeEntry(guid, startTime, endTime, timeEntryType, description, timeSpent, pauseTime, billableTime, billable, price, task, user);
        }

        #endregion
    }
}