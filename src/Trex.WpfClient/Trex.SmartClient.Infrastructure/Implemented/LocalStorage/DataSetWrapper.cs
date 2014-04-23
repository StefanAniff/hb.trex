using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Data;
using log4net;
using Task = Trex.SmartClient.Core.Model.Task;

namespace Trex.SmartClient.Infrastructure.Implemented.LocalStorage
{
    public class DataSetWrapper
    {
        private readonly IIsolatedStorageFileProvider _isolatedStorageFileProvider;
        private static readonly object LockObj = new object();
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Collections

        public DataSetWrapper(IIsolatedStorageFileProvider isolatedStorageFileProvider)
        {
            _isolatedStorageFileProvider = isolatedStorageFileProvider;
        }

        public virtual List<TimeTrackerDataSet.CustomersRow> Customers
        {
            get
            {
                lock (LockObj)
                {
                    return DataSet.Customers.ToList();
                }
            }
        }

        public virtual IEnumerable<TimeTrackerDataSet.ProjectsRow> Projects
        {
            get
            {
                lock (LockObj)
                {
                    return DataSet.Projects.ToList();
                }
            }
        }

        public List<TimeTrackerDataSet.TasksRow> Tasks
        {
            get
            {
                lock (LockObj)
                {
                    return DataSet.Tasks.ToList();
                }
            }
        }

        public IEnumerable<TimeTrackerDataSet.TimeEntryTypesRow> TimeEntryTypes
        {
            get
            {
                lock (LockObj)
                {
                    return DataSet.TimeEntryTypes.ToList();
                }
            }
        }

        public IEnumerable<TimeTrackerDataSet.TimeEntriesRow> TimeEntries
        {
            get
            {
                lock (LockObj)
                {
                    return DataSet.TimeEntries.ToList();
                }
            }
        }
        #endregion

        private TimeTrackerDataSet _dataSet;

        private TimeTrackerDataSet DataSet
        {
            get
            {
                lock (LockObj)
                {
                    if (_dataSet == null)
                    {
                        Load();
                    }
                    return _dataSet;
                }
            }
            set { _dataSet = value; }
        }

        public Task<object> Save()
        {
            var sw = new Stopwatch();
            sw.Start();
            var taskCompletionSource = new TaskCompletionSource<object>();
            lock (LockObj)
            {
                using (var appScope = _isolatedStorageFileProvider.GetIsolatedStorage())
                {
                    using (var fs = new IsolatedStorageFileStream(appSettings.Default.TimeTrackerDataFile, FileMode.OpenOrCreate, appScope))
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(fs, DataSet);
                        taskCompletionSource.SetResult(null);
                    }
                }
            }
            sw.Stop();
            Logger.Debug("Save(): " + sw.Elapsed);
            return taskCompletionSource.Task;
        }

        private void Load()
        {
            var sw = new Stopwatch();
            sw.Start();
            lock (LockObj)
            {
                try
                {
                    using (var appScope = _isolatedStorageFileProvider.GetIsolatedStorage())
                    {
                        using (var fs = new IsolatedStorageFileStream(appSettings.Default.TimeTrackerDataFile, FileMode.OpenOrCreate, appScope))
                        {
                            var formatter = new BinaryFormatter();
                            DataSet = (TimeTrackerDataSet)formatter.Deserialize(fs);
                        }
                    }
                }
                catch (Exception)
                {
                    DataSet = new TimeTrackerDataSet();
                }
            }
            sw.Stop();
            Logger.Debug("Load(): " + sw.Elapsed);
        }

        public void Delete()
        {
            var sw = new Stopwatch();
            sw.Start();
            using (var storage = _isolatedStorageFileProvider.GetIsolatedStorage())
            {
                if (storage.FileExists(appSettings.Default.TimeTrackerDataFile))
                {
                    try
                    {
                        storage.DeleteFile(appSettings.Default.TimeTrackerDataFile);
                    }
                    catch (IsolatedStorageException ex)
                    {
                        Logger.Error(ex);
                        DataSet = new TimeTrackerDataSet();
                        Save();
                    }
                }
            }
            DataSet = null;
            sw.Stop();
            Logger.Debug("Delete(): " + sw.Elapsed);
        }

        public void AddTimeEntryType(TimeEntryType timeEntryType)
        {
            lock (LockObj)
            {
                if (DataSet.TimeEntryTypes.SingleOrDefault(t => timeEntryType.Id == t.Id) == null)
                {
                    var newRow = DataSet.TimeEntryTypes.NewTimeEntryTypesRow();
                    newRow.Id = timeEntryType.Id;
                    newRow.Name = timeEntryType.Name;
                    newRow.IsBillableByDefault = timeEntryType.IsBillableByDefault;
                    newRow.IsDefault = timeEntryType.IsDefault;
                    if (timeEntryType.CustomerId.HasValue)
                    {
                        newRow.CustomerId = timeEntryType.CustomerId.Value;
                    }

                    DataSet.TimeEntryTypes.AddTimeEntryTypesRow(newRow);
                    Save();
                }
            }
        }

        public void SaveProject(Project project)
        {
            lock (LockObj)
            {
                var proj = DataSet.Projects.SingleOrDefault(p => p.Id == project.Id);
                if (proj == null)
                {
                    var newProject = DataSet.Projects.NewProjectsRow();

                    newProject.Name = project.Name;
                    newProject.Id = project.Id;
                    newProject.CustomerId = project.Company.Id;

                    DataSet.Projects.AddProjectsRow(newProject);
                }
                else
                {
                    proj.CustomerId = project.Company.Id;
                    proj.Name = project.Name;
                }
            }
        }

        public void SaveCustomer(Company company)
        {
            lock (LockObj)
            {
                var comp = DataSet.Customers.SingleOrDefault(c => c.Id == company.Id);

                if (comp == null)
                {
                    var newCustomer = DataSet.Customers.NewCustomersRow();

                    newCustomer.Name = company.Name;
                    newCustomer.Id = company.Id;
                    newCustomer.InheritsTimeEntryTypes = company.InheritsTimeEntryTypes;

                    DataSet.Customers.AddCustomersRow(newCustomer);
                }
                else
                {
                    comp.Name = company.Name;
                    comp.InheritsTimeEntryTypes = company.InheritsTimeEntryTypes;
                }
            }

        }

        public void RemoveTimeEntry(TimeEntry timeEntry)
        {
            var row = DataSet.TimeEntries.FindByGuid(timeEntry.Guid);
            if (row != null)
            {
                DataSet.TimeEntries.RemoveTimeEntriesRow(row);
            }
            Save();
        }

        public void SaveTimeEntry(TimeEntry timeEntry)
        {

            lock (LockObj)
            {
                var row = DataSet.TimeEntries.FindByGuid(timeEntry.Guid);
                if (row != null)
                {
                    //row.CreateDate = entry.CreatedDate;
                    row.Description = timeEntry.Description;
                    row.EntryStartDate = timeEntry.StartTime;
                    row.TimeEntryTypeId = timeEntry.TimeEntryType.Id;

                    row.BillableTime = timeEntry.BillableTime;
                    if (timeEntry.PricePrHour.HasValue)
                    {
                        row.PricePrHour = timeEntry.PricePrHour.Value;
                    }
                    row.Synced = timeEntry.IsSynced;
                    row.SyncInfo = timeEntry.SyncResponse;
                    row.TaskId = timeEntry.Task.Guid;
                    row.TimeSpent = timeEntry.TimeSpent;
                    row.Billable = timeEntry.Billable;
                }
                else
                {
                    var newTimeEntryRow = DataSet.TimeEntries.NewTimeEntriesRow();
                    newTimeEntryRow.Guid = timeEntry.Guid;
                    newTimeEntryRow.Description = timeEntry.Description;
                    newTimeEntryRow.CreateDate = timeEntry.CreateDate;
                    newTimeEntryRow.ClientSourceId = timeEntry.ClientSourceId;
                    newTimeEntryRow.EntryStartDate = timeEntry.StartTime;
                    newTimeEntryRow.TimeSpent = timeEntry.TimeSpent;
                    newTimeEntryRow.BillableTime = timeEntry.BillableTime;
                    newTimeEntryRow.TaskId = timeEntry.Task.Guid;
                    newTimeEntryRow.Billable = timeEntry.Billable;
                    newTimeEntryRow.Synced = timeEntry.IsSynced;
                    newTimeEntryRow.SyncInfo = timeEntry.SyncResponse;
                    newTimeEntryRow.TimeEntryTypeId = timeEntry.TimeEntryType.Id;

                    if (timeEntry.PricePrHour.HasValue)
                    {
                        newTimeEntryRow.PricePrHour = timeEntry.PricePrHour.Value;
                    }

                    DataSet.TimeEntries.AddTimeEntriesRow(newTimeEntryRow);
                }
            }
        }

        public TimeTrackerDataSet.TasksRow GetTaskByGuid(Guid guid)
        {
            lock (LockObj)
            {
                return DataSet.Tasks.FindByGuid(guid);
            }
        }

        public void SaveTask(Task task)
        {
            lock (LockObj)
            {
                AddOrUpdate(task);
            }
        }

        public void SaveTask(IEnumerable<Task> tasks)
        {
            lock (LockObj)
            {
                foreach (var task in tasks)
                {
                    AddOrUpdate(task);
                }
            }
        }

        private void AddOrUpdate(Task task)
        {
            if (DataSet.Tasks.All(x => x.Guid != task.Guid))
            {
                var newTask = DataSet.Tasks.NewTasksRow();

                newTask.Name = task.Name;
                newTask.Id = task.Id;
                newTask.Guid = task.Guid;
                newTask.ProjectId = task.Project.Id;
                newTask.CreateDate = task.CreateDate;
                newTask.Description = task.Description;
                newTask.Synced = task.IsSynced;
                newTask.SyncInfo = task.SyncResponse;
                newTask.TimeRegistionTypeId = (int)task.TimeRegistrationType;
                DataSet.Tasks.AddTasksRow(newTask);
            }
            else
            {
                var existingTask = DataSet.Tasks.First(t => t.Guid == task.Guid);

                existingTask.Name = task.Name;
                existingTask.Id = task.Id;
                existingTask.Guid = task.Guid;
                existingTask.ProjectId = task.Project.Id;
                existingTask.CreateDate = task.CreateDate;
                existingTask.Description = task.Description;
                existingTask.Synced = task.IsSynced;
                existingTask.SyncInfo = task.SyncResponse;
                existingTask.TimeRegistionTypeId = (int)task.TimeRegistrationType;
            }
        }

        public void AddUnsyncedTimeEntries(IEnumerable<TimeTrackerDataSet.TimeEntriesRow> unsyncedEntries)
        {
            if (!unsyncedEntries.Any())
            {
                return;
            }
            foreach (var timeEntriesRow in unsyncedEntries)
            {
                var newTimeEntryRow = DataSet.TimeEntries.NewTimeEntriesRow();
                newTimeEntryRow.Guid = timeEntriesRow.Guid;
                newTimeEntryRow.Description = timeEntriesRow.Description;
                newTimeEntryRow.CreateDate = timeEntriesRow.CreateDate;
                newTimeEntryRow.ClientSourceId = timeEntriesRow.ClientSourceId;
                newTimeEntryRow.EntryStartDate = timeEntriesRow.EntryStartDate;
                newTimeEntryRow.TimeSpent = timeEntriesRow.TimeSpent;
                newTimeEntryRow.BillableTime = timeEntriesRow.BillableTime;
                newTimeEntryRow.TaskId = timeEntriesRow.TaskId;
                newTimeEntryRow.Billable = timeEntriesRow.Billable;
                newTimeEntryRow.Synced = timeEntriesRow.Synced;
                newTimeEntryRow.SyncInfo = timeEntriesRow.SyncInfo;
                newTimeEntryRow.TimeEntryTypeId = timeEntriesRow.TimeEntryTypeId;

                newTimeEntryRow.PricePrHour = timeEntriesRow.PricePrHour;

                DataSet.TimeEntries.AddTimeEntriesRow(newTimeEntryRow);
            }

            Save();
        }
    }
}