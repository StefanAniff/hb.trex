using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.TaskModule.TaskScreen.HistoryFeedView
{
    public class HistoryFeedRowViewModel : ViewModelBase
    {
        public TimeEntry TimeEntry { get; set; }

        public HistoryFeedRowViewModel(DateTime date, string projectName)
        {
            Date = date;
            TaskName = projectName;
            StatusColor = new SolidColorBrush(Colors.Black);
        }
        public HistoryFeedRowViewModel(TimeEntry timeEntry)
        {
            TimeEntry = timeEntry;
            Date = TimeEntry.StartTime;
            CustomerName = TimeEntry.Task.Project.Company.Name;
            ProjectName = TimeEntry.Task.Project.Name;
            TaskName = TimeEntry.Task.Name;
            TimeSpent = TimeEntry.TimeSpent;
            TimeEntryType = TimeEntry.TimeEntryType.Name;
            Description = TimeEntry.HasSyncError ? TimeEntry.SyncResponse : TimeEntry.Description;
            Billable = TimeEntry.Billable;

            StatusColor = TimeEntry.IsSynced ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.Red);
        }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }
        public string ProjectName { get; set; }

        public string TaskName { get; set; }

        public TimeSpan TimeSpent { get; set; }

        public string TimeEntryType { get; set; }

        public string Description { get; set; }

        public bool Billable { get; set; }

        public Brush StatusColor { get; set; }

        public override string ToString()
        {
            return TaskName;
        }
    }
}