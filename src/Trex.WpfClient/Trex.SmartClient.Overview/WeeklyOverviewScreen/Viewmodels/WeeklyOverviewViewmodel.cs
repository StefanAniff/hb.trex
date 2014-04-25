using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Data;
using Trex.SmartClient.Core.Exceptions;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Overview.Interfaces;
using Trex.SmartClient.Overview.WeeklyOverviewScreen.Viewmodels.Itemviewmodel;
using log4net;
using Task = Trex.SmartClient.Core.Model.Task;

namespace Trex.SmartClient.Overview.WeeklyOverviewScreen.Viewmodels
{
    public class WeeklyOverviewViewmodel : ViewModelBase, IWeeklyOverviewViewmodel
    {
        private readonly ITimeEntryService _timeEntryService;
        //private readonly IBusyService _busyService;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly ITaskItemViewmodelFactory _taskItemViewmodelFactory;
        private readonly ITimeEntryTypeRepository _timeEntryTypeRepository;
        private readonly ISyncService _syncService;
        private readonly ICommonDialogs _commonDialogs;
        private bool _isSelectingNewTask;
        private List<TimeEntry> _timeEntriesForThisWeek;
        private bool _isShowingTextbox;

        private readonly List<TaskItemViewmodel> _deletedRows;
        private readonly DelegateCommand<object> _syncCompletedCommand;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DelegateCommand<object> PreviousDateCommand { get; set; }
        public DelegateCommand<object> NextDayCommand { get; set; }
        public DelegateCommand<object> SaveCommand { get; set; }
        public DelegateCommand<object> SwitchToDayViewCommand { get; set; }
        public DelegateCommand<object> CopyPreviousTasksToSelectedDate { get; set; }
        public DelegateCommand<object> DeleteTimeEntry { get; set; }
        public DelegateCommand<object> AddTaskCommand { get; set; }
        public DelegateCommand<object> TodayCommand { get; set; }              

        private bool _isSyncing;

        public bool IsSyncing
        {
            get { return _isSyncing; }
            private set
            {
                _isSyncing = value;
                OnPropertyChanged(() => IsSyncing);
            }
        }

        private ObservableItemCollection<TaskItemViewmodel> _rows;

        public ObservableItemCollection<TaskItemViewmodel> Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
                OnPropertyChanged(() => Rows);
            }
        }

        public List<TimeEntryType> TimeEntryTypes { get; set; }

        private bool _hasChanges;

        public bool HasChanges
        {
            get { return _hasChanges; }
            private set
            {
                _hasChanges = value;
                OnPropertyChanged(() => HasChanges);
            }
        }

        public bool CanCopyPreviousTimesheet
        {
            get { return !Rows.Any() && StartDate > DateTime.Today.AddDays(-7); }

        }

        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate == value)
                {
                    return;
                }

                if (HasChanges)
                {
                    var answer = MessageBox.Show("You have unsaved changes. Continue?", "Unsaved changes", MessageBoxButton.YesNo);
                    if (answer == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                var localDate = value.FirstDayOfWeek();
                _startDate = localDate;
                OnPropertyChanged(() => StartDate);
                OnPropertyChanged(() => StartDate);
                OnPropertyChanged(() => SelectedWeekNumber);
                LoadTimeEntries();
            }
        }

        private async void LoadTimeEntries()
        {
            IsSyncing = true;
            try
            {
                _timeEntriesForThisWeek = await _timeEntryService.GetTimeEntriesByDateIgnoreEmptyTimeEntries(StartDate, EndDate);
            }
            catch (Exception ex)
            {
                HandleSyncError(ex);
                _timeEntriesForThisWeek = new List<TimeEntry>();
            }
            UpdateLayout();
        }

        /// <summary>
        /// It is up to AND enddate when searching. Not up to. <=
        /// </summary>
        public DateTime EndDate
        {
            get { return StartDate.AddDays(6); }
            set { }
        }

        public string Text
        {
            get { return string.Format("{0:dd} – {1:dd} {0:MMM} {0:yyyy}", StartDate, EndDate); }
        }

        public DayItemHeaderViewmodel Day1 { get; private set; }
        public DayItemHeaderViewmodel Day2 { get; private set; }
        public DayItemHeaderViewmodel Day3 { get; private set; }
        public DayItemHeaderViewmodel Day4 { get; private set; }
        public DayItemHeaderViewmodel Day5 { get; private set; }
        public DayItemHeaderViewmodel Day6 { get; private set; }
        public DayItemHeaderViewmodel Day7 { get; private set; }

        public double Total
        {
            get { return Rows.Sum(x => x.RealTotal); }
        }

        public string ToolTip
        {
            get
            {
                return string.Format("Billable: {0:N2}. Non-billable: {1:N2}",
                    Rows.SelectMany(x => x.AllDays).Where(x => x.Billable).Sum(x => x.RegisteredHours.GetValueOrDefault()),
                                     Rows.SelectMany(x => x.AllDays).Where(x => !x.Billable).Sum(x => x.RegisteredHours.GetValueOrDefault()));
            }
        }

        public string SelectedWeekNumber
        {
            get { return StartDate.WeeknumberDk().ToString(CultureInfo.InvariantCulture); }
        }

        public WeeklyOverviewViewmodel(ITimeEntryService timeEntryService, IBusyService busyService, ITimeEntryRepository timeEntryRepository,
                                       ITaskItemViewmodelFactory taskItemViewmodelFactory, ITimeEntryTypeRepository timeEntryTypeRepository,
                                       ISyncService syncService, ICommonDialogs commonDialogs)
        {
            _timeEntryService = timeEntryService;
            //_busyService = busyService;
            _timeEntryRepository = timeEntryRepository;
            _taskItemViewmodelFactory = taskItemViewmodelFactory;
            _timeEntryTypeRepository = timeEntryTypeRepository;
            _syncService = syncService;
            _commonDialogs = commonDialogs;
            _syncCompletedCommand = new DelegateCommand<object>(SyncCompletedExecute);
            PreviousDateCommand = new DelegateCommand<object>(a => StartDate = StartDate.AddDays(-7));
            NextDayCommand = new DelegateCommand<object>(a => StartDate = StartDate.AddDays(7));
            TodayCommand = new DelegateCommand<object>(a => TodayCommandExecute());
            SwitchToDayViewCommand = new DelegateCommand<object>(SwitchToDayViewCommandExecute);
            CopyPreviousTasksToSelectedDate = new DelegateCommand<object>(CopyPreviousTasksToSelectedDateExecute);
            AddTaskCommand = new DelegateCommand<object>(AddTaskCommandExecute);
            SaveCommand = new DelegateCommand<object>(SaveCommandExecute);
            DeleteTimeEntry = new DelegateCommand<object>(DeleteTimeEntryExecute);
            ApplicationCommands.SyncCompleted.RegisterCommand(_syncCompletedCommand);
            TaskCommands.TaskSelectCompleted.RegisterCommand(new DelegateCommand<Task>(TaskSelectCompleted));
            TaskCommands.SaveTaskCancelled.RegisterCommand(new DelegateCommand<TimeEntry>(TaskSelectCompleted));

            Rows = new ObservableItemCollection<TaskItemViewmodel>();
            Rows.ItemChanged += TasksOnItemChanged;
            _deletedRows = new List<TaskItemViewmodel>();
            _timeEntriesForThisWeek = new List<TimeEntry>();

            TimeEntryTypes = _timeEntryTypeRepository.GetGlobal();
            StartDate = DateTime.Today.FirstDayOfWeek();

        }


        private void TodayCommandExecute()
        {
            var firstDayOfWeek = DateTime.Today.FirstDayOfWeek();
            StartDate = firstDayOfWeek;
        }

        private void TaskSelectCompleted(Task task)
        {
            if (_isSelectingNewTask && task != null)
            {
                Rows.Add(_taskItemViewmodelFactory.CreateEmptyTaskItemViewmodel(task, StartDate));
                UpdateBottom();
            }
            _isSelectingNewTask = false;
        }

        private void TaskSelectCompleted(TimeEntry timeentry)
        {
            if (timeentry != null)
            {
                TaskSelectCompleted(timeentry.Task);
            }
        }

        private void AddTaskCommandExecute(object obj)
        {
            _isSelectingNewTask = true;
            OverviewCommands.AddNewTask.Execute(null);
        }

        private async void SyncCompletedExecute(object obj)
        {
            try
            {
                _timeEntriesForThisWeek = await _timeEntryService.GetTimeEntriesByDateIgnoreEmptyTimeEntries(StartDate, EndDate);
            }
            catch (Exception ex)
            {
                HandleSyncError(ex);
            }

            UpdateLayout();
        }

        private void SaveCommandExecute(object obj)
        {
            //_busyService.ShowBusy();
            IsSyncing = true;
            var dayItemViewmodels = Rows.Where(x => x.HasChanges).SelectMany(x => x.AllDays).Where(x => x.HasChanges).ToList();
            var willConsolidate = dayItemViewmodels.Any(x => x.TimeEntries.Count() > 1);
            var consolidatedTimeEntries = _taskItemViewmodelFactory.ExtractConsolidatedTimeEntries(dayItemViewmodels);
            consolidatedTimeEntries.AddRange(_taskItemViewmodelFactory.ResetTimeEntries(_deletedRows));

            var result = MessageBoxResult.Yes;
            if (willConsolidate)
            {
                result = MessageBox.Show("Some tasks have multiple timeentries and will be consolidated." +
                                         " Are you sure you want to save?", "Confirm", MessageBoxButton.YesNo);
            }

            if (result == MessageBoxResult.Yes)
            {
                _timeEntryRepository.AddOrUpdateRange(consolidatedTimeEntries);
                ApplicationCommands.StartSync.Execute(null);
                HasChanges = false;
            }
            else
            {
                //_busyService.HideBusy();
                IsSyncing = false;
            }
        }

        private async void DeleteTimeEntryExecute(object obj)
        {
            var timeEntry = obj as TaskItemViewmodel;

            if (MessageBox.Show("Delete the timeentry?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                timeEntry.IsDeleted = true;
                await System.Threading.Tasks.Task.Run(() => Thread.Sleep(TimeSpan.FromSeconds(1)));

                Rows.Remove(timeEntry);
                _deletedRows.Add(timeEntry);
                HasChanges = true;
                UpdateBottom();
            }
        }

        private void HandleSyncError(Exception ex)
        {
            if (ex is TimeoutException)
            {
                Logger.Info(ex);
            }
            else if (ex is EndpointNotFoundException)
            {
                Logger.Info(ex);
            }
            else if (ex is ServiceAccessException)
            {
                Logger.Info(ex);
            }
            else
            {
                Logger.Error(ex);
            }
            if (!_isShowingTextbox)
            {
                _isShowingTextbox = true;
                var result = MessageBox.Show("Error loading timeentries", "Error", MessageBoxButton.OK);
                _isShowingTextbox = result == MessageBoxResult.None;
            }
        }

        private async void CopyPreviousTasksToSelectedDateExecute(object obj)
        {
            var localtimeEntriesForThisWeek = new List<TimeEntry>();
            IsSyncing = true;

            try
            {
                var localStartDate = StartDate;
                do
                {
                    var localEnddate = localStartDate.AddDays(6);
                    localtimeEntriesForThisWeek = await _timeEntryService.GetTimeEntriesByDateIgnoreEmptyTimeEntries(localStartDate, localEnddate);
                    localStartDate = localStartDate.AddDays(-7);
                } while (!localtimeEntriesForThisWeek.Any() && localStartDate >= DateTime.Today.AddMonths(-3));
            }
            catch (Exception ex)
            {
                HandleSyncError(ex);
            }

            if (localtimeEntriesForThisWeek.Any())
            {
                _timeEntriesForThisWeek.Clear();
                Rows.AddRange(_taskItemViewmodelFactory.ExtractEmptyItemTaskitems(localtimeEntriesForThisWeek, StartDate));
            }
            HasChanges = false;
            UpdateBottom();
        }

        private static void SwitchToDayViewCommandExecute(object obj)
        {
            OverviewCommands.GoToDaySubMenu.Execute(obj);
        }

        private void TasksOnItemChanged(object sender, ItemChangedEventArgs<TaskItemViewmodel> itemChangedEventArgs)
        {
            if (itemChangedEventArgs.Item.HasChanges)
            {
                HasChanges = true;
                UpdateBottom();
            }
        }

        private void UpdateLayout()
        {
            HasChanges = false;
            Rows.Clear();

            var localRows = _taskItemViewmodelFactory.ExtractItemTaskitems(_timeEntriesForThisWeek, StartDate);

            //todo: find a good way to order items
            //localRows = localRows.OrderBy(x => x.CustomerName).ThenBy(x => x.ProjectName).ThenBy(x => x.TaskName).ToList();
            Rows.AddRange(localRows);
            UpdateBottom();
        }

        private void UpdateBottom()
        {
            /** 
             * If full resync is in progress, repositories can be emtpy.
             * Will be invoked again, af complete sync
             * **/
            if (ResyncInProgress())
                return;

            UpdateDay1();
            UpdateDay2();
            UpdateDay3();
            UpdateDay4();
            UpdateDay5();
            UpdateDay6();
            UpdateDay7();

            OnPropertyChanged(() => CanCopyPreviousTimesheet);
            IsSyncing = false;
            OnPropertyChanged(() => HasChanges);
            IsSyncing = false;
            OnPropertyChanged(() => Total);
            OnPropertyChanged(() => ToolTip);
            OnPropertyChanged(() => Day1);
            OnPropertyChanged(() => Day2);
            OnPropertyChanged(() => Day3);
            OnPropertyChanged(() => Day4);
            OnPropertyChanged(() => Day5);
            OnPropertyChanged(() => Day6);
            OnPropertyChanged(() => Day7);
        }

        private bool ResyncInProgress()
        {
            return _syncService.ForceResync;
        }

        private void UpdateDay1()
        {
            Day1 = new DayItemHeaderViewmodel(StartDate, Rows.Select(x => x.Monday));
            Day1.SelectTimeEntryType = GetMostUsedTimeEntryType(Day1);
        }


        private void UpdateDay2()
        {
            Day2 = new DayItemHeaderViewmodel(StartDate.AddDays(1), Rows.Select(x => x.Tuesday));
            Day2.SelectTimeEntryType = GetMostUsedTimeEntryType(Day2);
        }

        private void UpdateDay3()
        {
            Day3 = new DayItemHeaderViewmodel(StartDate.AddDays(2), Rows.Select(x => x.Wednesday));
            Day3.SelectTimeEntryType = GetMostUsedTimeEntryType(Day3);
        }

        private void UpdateDay4()
        {
            Day4 = new DayItemHeaderViewmodel(StartDate.AddDays(3), Rows.Select(x => x.Thursday));
            Day4.SelectTimeEntryType = GetMostUsedTimeEntryType(Day4);
        }

        private void UpdateDay5()
        {
            Day5 = new DayItemHeaderViewmodel(StartDate.AddDays(4), Rows.Select(x => x.Friday));
            Day5.SelectTimeEntryType = GetMostUsedTimeEntryType(Day5);
        }

        private void UpdateDay6()
        {
            Day6 = new DayItemHeaderViewmodel(StartDate.AddDays(5), Rows.Select(x => x.Saturday));
            Day6.SelectTimeEntryType = GetMostUsedTimeEntryType(Day6);
        }

        private void UpdateDay7()
        {
            Day7 = new DayItemHeaderViewmodel(StartDate.AddDays(6), Rows.Select(x => x.Sunday));
            Day7.SelectTimeEntryType = GetMostUsedTimeEntryType(Day7);
        }


        private TimeEntryType GetMostUsedTimeEntryType(DayItemHeaderViewmodel dayItemHeaderViewmodel)
        {
            var timeEntries = dayItemHeaderViewmodel.TimeEntries
                .ToList();

            if (!timeEntries.Any())
            {
                return TimeEntryTypes.SingleOrDefault(x => x.IsDefault); // Can be empty if resync is in progress
            }
            var mostUsedTimeEntryType = timeEntries
                .GroupBy(x => x.TimeEntryType.Id)
                .OrderByDescending(x => x.Count())
                .First().Key;
            return TimeEntryTypes.SingleOrDefault(x => x.Id == mostUsedTimeEntryType); // Can be empty if resync is in progress
        }
    }
}