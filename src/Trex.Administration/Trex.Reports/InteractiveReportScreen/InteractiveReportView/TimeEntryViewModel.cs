using System;
using System.Globalization;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Reports.InteractiveReportScreen.InteractiveReportView
{
    public class TimeEntryViewModel
    {
        private readonly TimeEntryView _timeEntry;

        public TimeEntryViewModel(TimeEntryView timeEntry)
        {
            _timeEntry = timeEntry;
        }

        public string Consultant
        {
            get { return _timeEntry.Name; }
        }

        public string Location { get { return _timeEntry.Location; } }

        public string Department { get { return _timeEntry.Department; } }

        public DateTime Date
        {
            get { return _timeEntry.StartTime.Date; }
        }

        public double TimeSpent
        {
            get { return _timeEntry.BillableTime; }
        }

        public string Description
        {
            get { return _timeEntry.Description; }
        }

        public string Task
        {
            get { return _timeEntry.TaskName; }
        }

        public string Project
        {
            get { return _timeEntry.ProjectName; }
        }

        public string Customer
        {
            get { return _timeEntry.CustomerName; }
        }

        public int Week
        {
            get
            {
                return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(_timeEntry.StartTime,
                                                                         CalendarWeekRule.FirstDay,
                                                                         DayOfWeek.Monday);
            }
        }

        public string Month
        {
            get { return string.Format("{0}-{1}", _timeEntry.StartTime.ToString("MM"), _timeEntry.StartTime.Year); }
        }

        public bool Billable
        {
            get { return _timeEntry.Billable; }
        }
    }
}