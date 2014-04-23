using System;
using System.Collections.ObjectModel;
using Trex.ServiceContracts;

namespace Trex.Core.Model
{
    public interface ITimeEntryFilter
    {
        DateTime? DateFrom { get; set; }
        DateTime? DateTo { get; set; }
        ReadOnlyCollection<User> Users { get; }
        void AddUser(User user);
        void RemoveUser(User user);
        bool CanPassFilter(TimeEntry timeEntry);
    }
}