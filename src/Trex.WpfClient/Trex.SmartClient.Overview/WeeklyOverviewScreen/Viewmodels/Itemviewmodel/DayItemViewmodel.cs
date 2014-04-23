using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Overview.WeeklyOverviewScreen.Viewmodels.Itemviewmodel
{
    public class DayItemViewmodel : ViewModelBase
    {
        public Task Task { get; private set; }
        public DateTime Date { get; set; }
        public IEnumerable<TimeEntry> TimeEntries { get; private set; }


        private bool _billable;
        public bool Billable
        {
            get { return _billable; }
            set
            {
                _billable = value;
                HasChanges = TimeEntries.Any() || _registeredHours.HasValue;
                OnPropertyChanged(() => RegisteredHours);
            }
        }


        public bool HasChanges { get; private set; }

        private double? _registeredHours;

        public double? RegisteredHours
        {
            get { return _registeredHours; }
            set
            {
                _registeredHours = value;
                HasChanges = TimeEntries.Any() || _registeredHours.HasValue;
                OnPropertyChanged(() => RegisteredHours);
            }
        }


        public bool HasComment
        {
            get { return TimeEntries.Any() ? TimeEntries.Any(x => !string.IsNullOrEmpty(x.Description)) : !string.IsNullOrEmpty(_comment); }
        }

        private string _comment;
        public string Comment
        {
            get
            {
                return !TimeEntries.Any() ? _comment : ExtractComment(TimeEntries);
            }
            set
            {
                if (!TimeEntries.Any())
                {
                    _comment = value;
                }
                else
                {
                    foreach (var timeEntry in TimeEntries)
                    {
                        timeEntry.Description = string.Empty;
                    }
                    TimeEntries.First().Description = value;
                }

                HasChanges = true;
                OnPropertyChanged(() => Comment);
                OnPropertyChanged(() => HasComment);
            }
        }

        private static string ExtractComment(IEnumerable<TimeEntry> timeEntries)
        {
            var commentBuilder = new StringBuilder();
            foreach (var description in timeEntries.Where(x => !string.IsNullOrEmpty(x.Description)).Select(x => x.Description))
            {
                commentBuilder.Append(description);
                commentBuilder.Append(description.EndsWith(".") ? " " : ". ");
            }
            var comment = commentBuilder.ToString().TrimEnd(' ');
            return comment;
        }

        public DayItemViewmodel(IEnumerable<TimeEntry> timeEntrieses, Task task, DateTime date, bool isBillable)
        {
            _billable = isBillable;
            Date = date;
            Task = task;
            TimeEntries = timeEntrieses;
            var totalHours = TimeEntries.Sum(x => x.TimeSpent.TotalHours);
            RegisteredHours = totalHours == 0 ? (double?)null : totalHours;
            _comment = string.Empty;
            HasChanges = false;
        }

        public void SetTimeEntryType(TimeEntryType selectTimeEntryType)
        {
            foreach (var dayItemViewmodel in TimeEntries)
            {
                dayItemViewmodel.TimeEntryType = selectTimeEntryType;
            }
            HasChanges = true;
            OnPropertyChanged(() => TimeEntries);
        }
    }
}
