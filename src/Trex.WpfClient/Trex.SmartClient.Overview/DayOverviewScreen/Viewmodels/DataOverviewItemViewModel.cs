using System;
using System.Globalization;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Overview.DayOverviewScreen.Viewmodels
{
    public class DataOverviewItemViewModel
    {
        public TimeSpan Duration { get; private set; }
        public DateTime Date { get; private set; }

        public bool IsBillable
        {
            get { return TimeEntry.Billable; }
        }

        public string GroupName
        {
            get { return TimeEntry.Task.Name; }
        }

        public TimeEntry TimeEntry { get; private set; }

        public static string ToTimeFormat(DateTime time)
        {
            var hour = time.Hour.ToString(CultureInfo.InvariantCulture);
            hour = hour.Length == 1 ? "0" + hour : hour;

            var minute = time.Minute.ToString(CultureInfo.InvariantCulture);
            minute = minute.Length == 1 ? "0" + minute : minute;

            return string.Format("{0}:{1}", hour, minute);
        }

        public DataOverviewItemViewModel(TimeEntry timeEntry)
        {
            TimeEntry = timeEntry;
            Duration = TimeEntry.TimeSpent;
            Date = TimeEntry.StartTime;
        }

        public string ToolTip
        {
            get { return string.Format("Duration: {0:g}. StartTime: {1}. StopTime: {2}", Duration, ToTimeFormat(Date), ToTimeFormat(Date.Add(Duration))); }
        }
    }

    public class TotalDataOverviewItemViewModel : DataOverviewItemViewModel
    {
        public TotalDataOverviewItemViewModel(TimeEntry timeEntry)
            : base(timeEntry)
        {
        }

        public new string GroupName
        {
            get { return "Total"; }
        }

        public new string ToolTip
        {
            get { return string.Format("Task: {3}. Duration: {0:g}. StartTime: {1}. StopTime: {2}", Duration, ToTimeFormat(Date), ToTimeFormat(Date.Add(Duration)), TimeEntry.Task.Name); }
        }
    }
}