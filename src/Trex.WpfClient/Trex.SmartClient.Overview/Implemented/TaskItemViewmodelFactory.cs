using System;
using System.Collections.Generic;
using System.Linq;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Overview.Interfaces;
using Trex.SmartClient.Overview.WeeklyOverviewScreen.Viewmodels.Itemviewmodel;

namespace Trex.SmartClient.Overview.Implemented
{
    public class TaskItemViewmodelFactory : ITaskItemViewmodelFactory
    {
        private readonly ITimeEntryTypeRepository _timeEntryTypeRepository;

        public TaskItemViewmodelFactory(ITimeEntryTypeRepository timeEntryTypeRepository)
        {
            _timeEntryTypeRepository = timeEntryTypeRepository;
        }

        public TaskItemViewmodel CreateEmptyTaskItemViewmodel(Task task, DateTime startdate)
        {
            return new TaskItemViewmodel(task, new List<TimeEntry>(), startdate, true, false);
        }

        public List<TimeEntry> ResetTimeEntries(List<TaskItemViewmodel> deletedRows)
        {
            var consolidatedTimeEntries = new List<TimeEntry>();
            foreach (var deletedTimeEntry in deletedRows.SelectMany(x => x.AllDays).SelectMany(x => x.TimeEntries))
            {
                deletedTimeEntry.Reset();
                consolidatedTimeEntries.Add(deletedTimeEntry);
            }
            return consolidatedTimeEntries;
        }

        public List<TaskItemViewmodel> ExtractItemTaskitems(List<TimeEntry> timeEntriesForThisWeek, DateTime startDate)
        {
            var localRows = new List<TaskItemViewmodel>();
            foreach (var timeEntryByTask in timeEntriesForThisWeek.GroupBy(x => x.Task))
            {
                var task = timeEntryByTask.Key;
                foreach (var timeEntryBybillableType in timeEntryByTask.GroupBy(x => x.Billable))
                {
                    localRows.Add(new TaskItemViewmodel(task, timeEntryBybillableType.ToList(), startDate));
                }
            }
            return localRows;
        }

        public List<TaskItemViewmodel> ExtractEmptyItemTaskitems(List<TimeEntry> localtimeEntriesForThisWeek, DateTime startDate)
        {
            var rows = new List<TaskItemViewmodel>();
            foreach (var task in localtimeEntriesForThisWeek.Select(x => x.Task).Distinct())
            {
                var timeEntryType = _timeEntryTypeRepository.GetByCompany(task.Project.Company)
                                                            .FirstOrDefault(tt => tt.IsDefault);
                var isBillable = false;
                if (timeEntryType != null)
                {
                    isBillable = timeEntryType.IsBillableByDefault;
                }
                rows.Add(new TaskItemViewmodel(task, new List<TimeEntry>(), startDate, isBillable));
            }
            return rows;
        }

        public List<TimeEntry> ExtractConsolidatedTimeEntries(List<DayItemViewmodel> dayItemViewmodels)
        {
            var consolidatedTimeEntries = new List<TimeEntry>();

            foreach (var day in dayItemViewmodels)
            {
                consolidatedTimeEntries.AddRange(ConsolidateDay(day));
            }

            return consolidatedTimeEntries;
        }

        private IEnumerable<TimeEntry> ConsolidateDay(DayItemViewmodel dayItemViewmodel)
        {
            var consolidatedTimeEntries = new List<TimeEntry>();
            var registeredHours = dayItemViewmodel.RegisteredHours;
            var timeEntries = dayItemViewmodel.TimeEntries.ToList();

            var description = dayItemViewmodel.Comment;
            foreach (var localTimeEntry in timeEntries)
            {
                localTimeEntry.Reset();
                consolidatedTimeEntries.Add(localTimeEntry);
            }

            var timeEntry = timeEntries.FirstOrDefault();
            if (timeEntry == null)
            {
                timeEntry = TimeEntry.Create();
                timeEntry.Task = dayItemViewmodel.Task;
                timeEntry.StartTime = dayItemViewmodel.Date;

                timeEntry.TimeEntryType = _timeEntryTypeRepository.GetByCompany(timeEntry.Task.Project.Company)
                                                                  .FirstOrDefault(tt => tt.IsDefault);

                consolidatedTimeEntries.Add(timeEntry);
            }

            timeEntry.Billable = dayItemViewmodel.Billable;
            timeEntry.EndTime = timeEntry.StartTime.AddHours(registeredHours.GetValueOrDefault());
            timeEntry.BillableTime = timeEntry.TimeSpent = (timeEntry.EndTime - timeEntry.StartTime);
            timeEntry.Description = description;
            return consolidatedTimeEntries;
        }

    }
}