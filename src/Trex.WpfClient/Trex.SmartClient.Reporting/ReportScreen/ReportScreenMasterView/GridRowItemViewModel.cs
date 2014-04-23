using System;
using System.Globalization;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Reporting.ReportScreen.ReportScreenMasterView
{
    public class GridRowItemViewModel : ViewModelBase
    {
        public readonly TimeEntry TimeEntry;
        public bool HasChanges { get; set; }

        public GridRowItemViewModel(TimeEntry timeEntry, TimeEntryType timeEntryType)
        {
            TimeEntry = timeEntry;
            EditableType = timeEntryType;
            EditableBillable = TimeEntry.Billable;
            HasChanges = false;
        }

        public int Month
        {
            get { return TimeEntry.StartTime.Date.Month; }
        }

        public TimeRegistrationTypeEnum RegistrationType
        {
            get { return TimeEntry.Task.TimeRegistrationType; }
        }

        public int Year
        {
            get { return TimeEntry.StartTime.Date.Year; }
        }

        public DateTime Date
        {
            get { return TimeEntry.StartTime.Date; }
        }

        /// <summary>
        /// Do not rename or remove. Used by radchart
        /// </summary>
        public string ChartDate
        {
            get { return TimeEntry.StartTime.Date.ToShortDateString(); }
        }

        public string Customer
        {
            get { return TimeEntry.Task.Project.Company.Name; }
        }

        public string TaskName
        {
            get { return TimeEntry.Task.Name; }
        }

        public string ProjectName
        {
            get { return TimeEntry.Task.Project.Name; }
        }

        public string Billable
        {
            get
            {
                return EditableBillable ? "Billable" : "Not billable";
            }
        }

        private bool _editableBillable;
        public bool EditableBillable
        {
            get { return _editableBillable; }
            set
            {
                _editableBillable = value;
                HasChanges = true;
                OnPropertyChanged(() => Billable);
            }
        }

        public string Type
        {
            get { return EditableType != null ? EditableType.Name : string.Empty; }
        }

        private TimeEntryType _editableType;

        public TimeEntryType EditableType
        {
            get { return _editableType; }
            set
            {
                _editableType = value;
                HasChanges = true;
                OnPropertyChanged(() => Type);
            }
        }

        public double TimeSpent
        {
            get { return TimeEntry.TimeSpent.TotalHours; }
        }

        public TimeSpan TimeSpentTimeSpan
        {
            get { return TimeEntry.TimeSpent; }
        }

        public int WeekNumber
        {
            get
            {
                return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(TimeEntry.StartTime,
                                                                         CalendarWeekRule.FirstFourDayWeek,
                                                                         DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek);
            }
        }
    }
}