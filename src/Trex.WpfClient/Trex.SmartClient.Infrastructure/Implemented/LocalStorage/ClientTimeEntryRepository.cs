using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Trex.SmartClient.Core.Exceptions;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Data;
using log4net;

namespace Trex.SmartClient.Infrastructure.Implemented.LocalStorage
{
    public class ClientTimeEntryRepository : ITimeEntryRepository
    {
        private readonly DataSetWrapper _dataWrapper;
        private readonly ITaskRepository _taskRepository;
        private readonly ITimeEntryTypeRepository _timeEntryTypeRepository;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ClientTimeEntryRepository(ITaskRepository taskRepository, ITimeEntryTypeRepository timeEntryTypeRepository, DataSetWrapper dataSetWrapper)
        {
            _dataWrapper = dataSetWrapper;
            _taskRepository = taskRepository;
            _timeEntryTypeRepository = timeEntryTypeRepository;

        }

        public void AddOrUpdate(TimeEntry timeEntry)
        {
            var exists = _dataWrapper.TimeEntries.ToList().Exists(te => te.Guid == timeEntry.Guid);
            if (exists)
            {
                Update(timeEntry);
            }
            else
            {
                Add(timeEntry);
            }
            _dataWrapper.Save();
        }


        public void AddOrUpdateRange(List<TimeEntry> timeEntries)
        {
            if (!timeEntries.Any())
            {
                return;
            }
            foreach (var timeEntry in timeEntries)
            {
                var exists = _dataWrapper.TimeEntries.ToList().Exists(te => te.Guid == timeEntry.Guid);
                if (exists)
                {
                    Update(timeEntry);
                }
                else
                {
                    Add(timeEntry);
                }
            }

            _dataWrapper.Save();
        }

        public void RemoveTimeEntiresWithErrors()
        {
            var entries = GetUnsyncedTimeEntries()
                .Where(x => x.HasSyncError);

            foreach (var timeEntriesRow in entries)
            {
                _dataWrapper.RemoveTimeEntry(timeEntriesRow);
            }
        }


        public bool Exists(TimeEntry timeEntry)
        {
            return _dataWrapper.TimeEntries.SingleOrDefault(te => te.Guid == timeEntry.Guid) != null;
        }

        public List<TimeEntry> GetUnsyncedTimeEntries()
        {
            var entries = _dataWrapper.TimeEntries
                                      .Where(row => row.Synced == false)
                                      .ToList();

            return ConvertFromXMLToTimeEntry(entries);
        }

        private List<TimeEntry> ConvertFromXMLToTimeEntry(IEnumerable<TimeTrackerDataSet.TimeEntriesRow> rows)
        {
            return rows.Select(t =>
                {
                    var task = _taskRepository.GetByGuid(t.TaskId);
                    var timeEntryType = _timeEntryTypeRepository.GetById(t.TimeEntryTypeId);
                    return TimeEntry.Create(t.Guid, task, timeEntryType, t.TimeSpent, t.BillableTime,
                                            t.Description, t.EntryStartDate, t.EntryStartDate, t.PricePrHour, t.Synced, t.SyncInfo, t.Billable,
                                            new TimeEntryHistory(), true, t.CreateDate, t.ClientSourceId, t.Invoiced);
                })
                       .ToList();
        }


        private void Add(TimeEntry timeEntry)
        {
            if (timeEntry.Task == null)
            {
                throw new UnassignedTimeEntryException("Unable to save timeentry without assigned task");
            }

            if (!_taskRepository.Exists(timeEntry.Task.Guid))
            {
                _taskRepository.AddOrUpdate(timeEntry.Task);
            }

            _dataWrapper.SaveTimeEntry(timeEntry);
        }
        private void Update(TimeEntry timeEntry)
        {
            if (timeEntry.Task == null)
            {
                throw new UnassignedTimeEntryException("Unable to save timeentry without assigned task");
            }

            if (!_taskRepository.Exists(timeEntry.Task.Guid))
            {
                _taskRepository.AddOrUpdate(timeEntry.Task);
            }
            _dataWrapper.SaveTimeEntry(timeEntry);
        }

        public void DeleteAllRepositories()
        {
            var unsyncedEntries = _dataWrapper.TimeEntries
                                              .Where(row => row.Synced == false)
                                              .ToList();
            _dataWrapper.Delete();
            try
            {
                _dataWrapper.AddUnsyncedTimeEntries(unsyncedEntries);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}