using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Trex.ServiceContracts;

namespace Trex.Core.Model
{
    public class TimeEntryFilter : ITimeEntryFilter
    {
        private readonly List<User> _users;

        public TimeEntryFilter()
        {
            _users = new List<User>();
        }

        public bool BillableOnly { get; set; }
        public bool ShowInvoiced { get; set; }
        public bool ShowNotInvoiced { get; set; }
        public bool HideEmptyTasks { get; set; }
        public bool HideEmptyProjects { get; set; }

        #region ITimeEntryFilter Members

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public ReadOnlyCollection<User> Users
        {
            get { return new ReadOnlyCollection<User>(_users); }
        }

        public void AddUser(User user)
        {
            if (_users.Count(u => u.UserID == user.UserID) == 0)
            {
                _users.Add(user);
            }
        }

        public void RemoveUser(User user)
        {
            var userToRemove = _users.SingleOrDefault(u => u.UserID == user.UserID);
            if (userToRemove != null)
            {
                _users.Remove(userToRemove);
            }
        }

        public bool CanPassFilter(TimeEntry timeEntry)
        {
            if (_users.Count > 0)
            {
                if (_users.Count(u => u.UserID == timeEntry.UserID) == 0)
                {
                    return false;
                }
            }

            if (DateFrom.HasValue)
            {
                if (timeEntry.EndTime.Date < DateFrom.Value.Date)
                {
                    return false;
                }
            }

            if (DateTo.HasValue)
            {
                if (timeEntry.EndTime.Date > DateTo.Value.Date)
                {
                    return false;
                }
            }

            if (BillableOnly && !timeEntry.Billable)
            {
                return false;
            }

            if (timeEntry.InvoiceId.HasValue)
            {
                if (!ShowInvoiced)
                {
                    return false;
                }
            }

            if (!timeEntry.InvoiceId.HasValue)
            {
                if (!ShowNotInvoiced)
                {
                    return false;
                }

                if(timeEntry.DocumentType ==2)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}