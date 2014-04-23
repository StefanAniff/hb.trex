using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Overview.WeeklyOverviewScreen.Viewmodels.Itemviewmodel;

namespace Trex.SmartClient.Overview.Interfaces
{
    public interface ITaskItemViewmodelFactory
    {
        List<TimeEntry> ExtractConsolidatedTimeEntries(List<DayItemViewmodel> dayItemViewmodels);
        List<TaskItemViewmodel> ExtractEmptyItemTaskitems(List<TimeEntry> localtimeEntriesForThisWeek, DateTime startDate);
        List<TaskItemViewmodel> ExtractItemTaskitems(List<TimeEntry> timeEntriesForThisWeek, DateTime startDate);
        List<TimeEntry> ResetTimeEntries(List<TaskItemViewmodel> deletedRows);
        TaskItemViewmodel CreateEmptyTaskItemViewmodel(Task task, DateTime startdate);
    }
}
