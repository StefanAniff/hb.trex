using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.Dialog.SelectTask.Interfaces;
using Trex.Dialog.SelectTask.Viewmodels.Itemviewmodel;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.TaskModule.Interfaces;
using Trex.SmartClient.TaskModule.SettingsScreen.WlanBinding;

namespace Trex.SmartClient.TaskModule.SettingsScreen.SettingsView
{
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        private DelegateCommand<Task> favoriteTaskSelectedCommand;

        private readonly IAppSettings _appSettings;
        private readonly IUserWlanSettingsService _userWlanSettingsService;
        private readonly ITimeEntryTypeRepository _timeEntryTypeRepository;
        private bool _inActiveTasksLayoutChanged;
        private IConnectivityService _connectivityService;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly ITaskRepository _taskRepository;
        public DelegateCommand<object> ResetInactiveTaskLayout { get; set; }
        public DelegateCommand<object> ResetActiveTaskLayout { get; set; }

        public DelegateCommand<object> Resync { get; set; }
        public DelegateCommand<object> SaveCommand { get; set; }
        public DelegateCommand<object> CancelCommand { get; set; }
        public DelegateCommand<TaskListViewModel> DeleteFavoriteTask { get; set; }


        public DelegateCommand<object> DeleteLocalData { get; set; }

        public DelegateCommand<object> ShowDataLocationFolder { get; set; }

        public ISelectTaskViewModel SelectTaskViewModel { get; set; }

        private ObservableCollection<TaskListViewModel> _favoriteTasks;
        //todo: move to own viewmodel
        public ObservableCollection<TaskListViewModel> FavoriteTasks
        {
            get { return _favoriteTasks; }
            set
            {
                _favoriteTasks = value;
                OnPropertyChanged(() => FavoriteTasks);
            }
        }

        public SettingsViewModel(IAppSettings appSettings, IUserWlanSettingsService userWlanSettingsService,
                                 ITimeEntryTypeRepository timeEntryTypeRepository,
                                 IConnectivityService connectivityService,
                                 ITimeEntryRepository timeEntryRepository, ISelectTaskViewModel selectTaskViewModel,
                                 ITaskRepository taskRepository)
        {
            _appSettings = appSettings;
            _userWlanSettingsService = userWlanSettingsService;
            _timeEntryTypeRepository = timeEntryTypeRepository;
            _connectivityService = connectivityService;
            _timeEntryRepository = timeEntryRepository;
            _taskRepository = taskRepository;
            SelectTaskViewModel = selectTaskViewModel;


            SaveCommand = new DelegateCommand<object>(ExecuteSave);
            CancelCommand = new DelegateCommand<object>(ExecuteCancel);
            Resync = new DelegateCommand<object>(ExecuteResync);
            ResetActiveTaskLayout = new DelegateCommand<object>(ResetActiveTaskLayoutExecute);
            ResetInactiveTaskLayout = new DelegateCommand<object>(ResetInactiveTaskLayoutExecute);
            DeleteLocalData = new DelegateCommand<object>(DeleteLocalDataExecute);
            ShowDataLocationFolder = new DelegateCommand<object>(ShowDataLocationFolderExecute);
            DeleteFavoriteTask = new DelegateCommand<TaskListViewModel>(DeleteFavoriteTaskExecute);

            favoriteTaskSelectedCommand = new DelegateCommand<Task>(FavoriteTaskSeleced);
            TaskCommands.TaskSelectCompleted.RegisterCommand(favoriteTaskSelectedCommand);
            FavoriteTasks = new ObservableCollection<TaskListViewModel>();
            FavoriteTasks.CollectionChanged += FavoriteTasksOnCollectionChanged;
            GetFavoriteTasks();
        }


        #region favorites
        private void FavoriteTasksOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            _appSettings.FavoriteTaskGuids = FavoriteTasks.Select(x => x.Task.Guid);
        }


        private void DeleteFavoriteTaskExecute(TaskListViewModel task)
        {
            var taskListViewModel = FavoriteTasks.SingleOrDefault(x => x.Task.Guid == task.Task.Guid);
            FavoriteTasks.Remove(taskListViewModel);
        }


        private void GetFavoriteTasks()
        {
            var favoriteTaskGuids = _appSettings.FavoriteTaskGuids;
            foreach (var guid in favoriteTaskGuids)
            {
                if (_taskRepository.Exists(guid))
                {
                    var task = _taskRepository.GetByGuid(guid);
                    FavoriteTasks.Add(new TaskListViewModel(task));
                }
            }
        }

        private void FavoriteTaskSeleced(Task task)
        {
            FavoriteTasks.Add(new TaskListViewModel(task));
        }
        #endregion


        private void ShowDataLocationFolderExecute(object obj)
        {
            Process.Start(_appSettings.DataDirectory);
        }

        private void DeleteLocalDataExecute(object obj)
        {
            _timeEntryRepository.DeleteAllRepositories();
        }

        private void ResetActiveTaskLayoutExecute(object obj)
        {
            _appSettings.ActiveTaskPositionX = -1;
            _appSettings.ActiveTaskPositionY = -1;
            ApplicationCommands.InActiveTaskLayoutChanged.Execute(null);
            ApplicationCommands.InActiveTaskLayoutChanged.Execute(null);
        }

        private void ResetInactiveTaskLayoutExecute(object obj)
        {
            InActiveTaskWidth = 126;
            InactiveTaskHeight = 84;
            InactiveTaskTimeSpentFontSize = 18;
            InactiveTaskTaskNameFontSize = 12;
            InactiveTaskDescriptionFontSize = 11;
        }

        private void ExecuteResync(object obj)
        {
            ApplicationCommands.Resync.Execute(null);
        }

        private void ExecuteCancel(object obj)
        {
            _appSettings.Reset();
            ApplicationCommands.SettingsSaved.Execute(null);
            if (_inActiveTasksLayoutChanged)
            {
                ApplicationCommands.InActiveTaskLayoutChanged.Execute(null);
            }
        }

        private void ExecuteSave(object obj)
        {
            _appSettings.Save();
            ApplicationCommands.SettingsSaved.Execute(null);
            if (_inActiveTasksLayoutChanged)
            {
                ApplicationCommands.InActiveTaskLayoutChanged.Execute(null);
            }
        }

        public int IdleTimeInterval
        {
            get { return (int)_appSettings.IdleTimeNotificationInterval.TotalMinutes; }
            set { _appSettings.IdleTimeNotificationInterval = TimeSpan.FromMinutes(value); }
        }

        public int RunTimeInterval
        {
            get { return (int)_appSettings.ActiveTimeNotificationInterval.TotalMinutes; }
            set { _appSettings.ActiveTimeNotificationInterval = TimeSpan.FromMinutes(value); }
        }

        public bool ShowNotifications
        {
            get { return _appSettings.NotificationEnabled; }
            set { _appSettings.NotificationEnabled = value; }
        }

        public bool HideWhenMinimized
        {
            get { return _appSettings.HideWhenMinimized; }
            set { _appSettings.HideWhenMinimized = value; }
        }

        public bool StartTaskWhenApplicationStarts
        {
            get { return _appSettings.StartTaskWhenApplicationStarts; }
            set { _appSettings.StartTaskWhenApplicationStarts = value; }
        }

        public bool StartTaskWhenTaskIsActivated
        {
            get { return _appSettings.StartTaskWhenActivated; }
            set { _appSettings.StartTaskWhenActivated = value; }
        }


        public bool AdvancedSettingsEnabled
        {
            get { return _appSettings.AdvancedSettingsEnabled; }
            set { _appSettings.AdvancedSettingsEnabled = value; }
        }


        public int SyncInterval
        {
            get { return (int)_appSettings.SyncInterval.TotalMinutes; }
            set { _appSettings.SyncInterval = TimeSpan.FromMinutes(value); }
        }

        public int HistoryNumOfDaysBack
        {
            get { return _appSettings.HistoryNumOfDaysBack; }
            set { _appSettings.HistoryNumOfDaysBack = value; }
        }


        public double InActiveTaskWidth
        {
            get { return _appSettings.InActiveTaskWidth; }
            set
            {
                _appSettings.InActiveTaskWidth = value;
                _inActiveTasksLayoutChanged = true;
                ApplicationCommands.InActiveTaskLayoutChanged.Execute(null);
                OnPropertyChanged(() => InActiveTaskWidth);
            }
        }

        public double InactiveTaskHeight
        {
            get { return _appSettings.InactiveTaskHeight; }
            set
            {
                _appSettings.InactiveTaskHeight = value;
                _inActiveTasksLayoutChanged = true;
                ApplicationCommands.InActiveTaskLayoutChanged.Execute(null);
                OnPropertyChanged(() => InactiveTaskHeight);
            }
        }

        public double InactiveTaskDescriptionFontSize
        {
            get { return _appSettings.InactiveTaskDescriptionFontSize; }
            set
            {
                _appSettings.InactiveTaskDescriptionFontSize = value;
                _inActiveTasksLayoutChanged = true;
                ApplicationCommands.InActiveTaskLayoutChanged.Execute(null);
                OnPropertyChanged(() => InactiveTaskDescriptionFontSize);
            }
        }

        public double InactiveTaskTaskNameFontSize
        {
            get { return _appSettings.InactiveTaskTaskNameFontSize; }
            set
            {
                _appSettings.InactiveTaskTaskNameFontSize = value;
                _inActiveTasksLayoutChanged = true;
                ApplicationCommands.InActiveTaskLayoutChanged.Execute(null);
                OnPropertyChanged(() => InactiveTaskTaskNameFontSize);
            }
        }

        public double InactiveTaskTimeSpentFontSize
        {
            get { return _appSettings.InactiveTaskTimeSpentFontSize; }
            set
            {
                _appSettings.InactiveTaskTimeSpentFontSize = value;
                _inActiveTasksLayoutChanged = true;
                ApplicationCommands.InActiveTaskLayoutChanged.Execute(null);
                OnPropertyChanged(() => InactiveTaskTimeSpentFontSize);
            }
        }

        public bool StartScreenIsRegistration
        {
            get { return _appSettings.StartScreenIsRegistration; }
            set { _appSettings.StartScreenIsRegistration = value; }
        }

        public bool StartScreenIsWeekOverview
        {
            get { return _appSettings.StartScreenIsWeekOverview; }
            set { _appSettings.StartScreenIsWeekOverview = value; }
        }

        public bool ShowListBoxviewSelector
        {
            get { return !_appSettings.ShowTreeviewSelector; }
            set { _appSettings.ShowTreeviewSelector = !value; }
        }

        public bool ShowTreeviewSelector
        {
            get { return _appSettings.ShowTreeviewSelector; }
            set { _appSettings.ShowTreeviewSelector = value; }
        }

        public IWlanBindingViewmodel WlanBindingViewModel
        {
            get
            {
                return new WlanBindingViewmodel(_userWlanSettingsService, _timeEntryTypeRepository.GetGlobal(),
                                                _connectivityService);
            }
        }

        public bool WorkPlanRealizedShowEverything
        {
            get { return !_appSettings.WorkPlanRealizedHourBillableOnly; }
            set
            {
                _appSettings.WorkPlanRealizedHourBillableOnly = !value;
            }
        }

        public bool WorkPlanRealizedShowBillableOnly
        {
            get { return _appSettings.WorkPlanRealizedHourBillableOnly; }
            set
            {
                _appSettings.WorkPlanRealizedHourBillableOnly = value;
            }
        }



        protected override void Dispose(bool disposing)
        {
            TaskCommands.TaskSelectCompleted.UnregisterCommand(favoriteTaskSelectedCommand);
            FavoriteTasks.CollectionChanged -= FavoriteTasksOnCollectionChanged;
            base.Dispose(disposing);
        }

    }
}