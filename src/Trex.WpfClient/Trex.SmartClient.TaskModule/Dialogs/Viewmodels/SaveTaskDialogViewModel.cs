using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.Dialog.SelectTask.Interfaces;
using Trex.Dialog.SelectTask.Viewmodels;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Model.Consts;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.TaskModule.Dialogs.DesignData;

namespace Trex.SmartClient.TaskModule.Dialogs.Viewmodels
{
    public class SaveTaskDialogViewModel : ViewModelBase, ISaveTaskDialogViewModel
    {
        private readonly TimeEntry _timeEntry;
        private readonly ITimeEntryTypeRepository _timeEntryTypeRepository;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IAppSettings _appSettings;
        private readonly IUserWlanSettingsService _userWlanSettingsService;
        private readonly bool _isEditing;

        public ISelectTaskViewModel SelectTaskViewModel { get; set; }

        private DelegateCommand<Task> executeSelectTaskCommand;
        private DelegateCommand<object> taskStoppedCommand;


        public DelegateCommand<object> SaveTask { get; set; }
        public DelegateCommand<object> CloseCommand { get; set; }

        public SaveTaskDialogViewModel(TimeEntry timeEntry,
                                       ITimeEntryTypeRepository timeEntryTypeRepository,
                                       ITimeEntryRepository timeEntryRepository,
                                       ITaskSearchService taskSearchService,
                                       IUserSession userSession,
                                       IAppSettings appSettings,
            IUserWlanSettingsService userWlanSettingsService, bool isEditing, ITaskRepository taskRepository)
        {
            _timeEntry = timeEntry;
            _timeEntryTypeRepository = timeEntryTypeRepository;
            _timeEntryRepository = timeEntryRepository;
            _appSettings = appSettings;
            _userWlanSettingsService = userWlanSettingsService;
            _isEditing = isEditing;

            SaveTask = new DelegateCommand<object>(ExecuteSaveTask, CanSaveTask);
            executeSelectTaskCommand = new DelegateCommand<Task>(ExecuteSelectTask);
            taskStoppedCommand = new DelegateCommand<object>(TaskStopped);
            CloseCommand = new DelegateCommand<object>(ExecuteClose);

            TaskCommands.TaskIdle.RegisterCommand(taskStoppedCommand);
            TaskCommands.TaskSelectCompleted.RegisterCommand(executeSelectTaskCommand);

            SelectTaskViewModel = new SelectTaskViewModel(taskSearchService, userSession, appSettings, taskRepository);

            LoadTimeEntryTypes();
            _isInSelectionMode = _timeEntry.Task == null;
            MinSelectedDate = appSettings.MinTimeEntryDate;
        }


        private void ExecuteClose(object obj)
        {
            TaskCommands.SaveTaskCancelled.Execute(_timeEntry);
        }

        private void LoadTimeEntryTypes()
        {
            List<TimeEntryType> timeEntryTypes;

            if (_timeEntry.Task == null)
            {
                TimeEntryTypes = null;
                timeEntryTypes = _timeEntryTypeRepository.GetGlobal();
            }
            else
            {
                var selectedCompany = _timeEntry.Task.Project.Company;
                timeEntryTypes = _timeEntryTypeRepository.GetByCompany(selectedCompany);
            }
            TimeEntryTypes = new ObservableCollection<TimeEntryType>(timeEntryTypes);
            SetSelectedTimeEntryType();
        }

        private void TaskStopped(object obj)
        {
            SaveTask.RaiseCanExecuteChanged();
        }

        private void ExecuteSaveTask(object obj)
        {
            _timeEntry.IsSynced = false;
            _timeEntry.BillableTime = _timeEntry.TimeSpent;
            _timeEntryRepository.AddOrUpdate(_timeEntry);
            TaskCommands.SaveTaskCompleted.Execute(_timeEntry);
            ApplicationCommands.StartSync.Execute(_timeEntry);
        }

        private bool CanSaveTask(object arg)
        {
            return _timeEntry.Task != null
                   && SelectedTimeEntryType != null
                   && _timeEntry.IsStopped
                   && !Invoiced;
        }

        private void ExecuteSelectTask(Task task)
        {
            IsInSelectionMode = false;

            _timeEntry.Task = task;
            LoadTimeEntryTypes();
            SaveTask.RaiseCanExecuteChanged();

            OnPropertyChanged(() => AssignedTask);
            TaskCommands.TaskAssigned.Execute(_timeEntry);
            if (!_timeEntry.IsStopped)
            {
                ExecuteClose(null);
            }
        }


        private bool _isOpen;

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
                OnPropertyChanged(() => IsOpen);
            }
        }

        public bool IsRunning
        {
            get { return !_timeEntry.IsStopped; }

        }

        public bool Invoiced
        {
            get { return _timeEntry.Invoiced
                || SelectedDate.Date <= MinSelectedDate;
            }

        }

        public DateTime MinSelectedDate { get; set; }

        public DateTime MinSelectedDatePlusDate
        {
            get { return MinSelectedDate.AddDays(1); }
        }

        public DateTime SelectedDate
        {
            get { return _timeEntry.StartTime; }
            set
            {
                _timeEntry.StartTime = value;
                OnPropertyChanged(() => SelectedDate);
                EndTime = SelectedDate.Add(TimeSpent);
            }
        }

        public TimeSpan TimeSpent
        {
            get { return _timeEntry.TimeSpent; }
            set
            {
                var time = value;
                _timeEntry.TimeSpent = time;
                _timeEntry.BillableTime = time;
                OnPropertyChanged(() => TimeSpent);
                EndTime = SelectedDate.Add(TimeSpent);
            }
        }


        public DateTime EndTime
        {
            get { return _timeEntry.EndTime; }
            set
            {
                _timeEntry.EndTime = value;
                OnPropertyChanged(() => EndTime);
                _timeEntry.TimeSpent = EndTime - SelectedDate;
                OnPropertyChanged(() => TimeSpent);
            }
        }

        private bool _isInSelectionMode;

        public bool IsInSelectionMode
        {
            get { return _isInSelectionMode; }
            set
            {
                _isInSelectionMode = value;
                OnPropertyChanged(() => IsInSelectionMode);
            }
        }

        public string AssignedTask
        {
            get { return _timeEntry.Task != null ? _timeEntry.Task.FullyQualifiedName : HelperTextConsts.UnassignedTask; }
        }




        public string Description
        {
            get { return _timeEntry.Description; }
            set
            {
                _timeEntry.Description = value;
                OnPropertyChanged(() => Description);
            }
        }

        public bool IsBillable
        {
            get { return _timeEntry.Billable; }
            set
            {
                _timeEntry.Billable = value;
                OnPropertyChanged(() => IsBillable);
            }
        }

        public string PricePrHour
        {
            get { return !_timeEntry.PricePrHour.HasValue ? "Auto" : _timeEntry.PricePrHour.Value.ToString("N2"); }
            set
            {
                if (value != "Auto")
                {
                    double price;
                    if (double.TryParse(value, out price))
                    {
                        if (price > 0)
                            _timeEntry.PricePrHour = price;
                        OnPropertyChanged(() => PricePrHour);
                        return;
                    }

                    _timeEntry.PricePrHour = null;
                    OnPropertyChanged(() => PricePrHour);
                }
            }
        }

        public TimeEntryType SelectedTimeEntryType
        {
            get { return _timeEntry.TimeEntryType; }
            set
            {
                _timeEntry.TimeEntryType = value;
                OnPropertyChanged(() => SelectedTimeEntryType);
                SaveTask.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<TimeEntryType> _timeEntryTypes;

        public ObservableCollection<TimeEntryType> TimeEntryTypes
        {
            get { return _timeEntryTypes; }
            set
            {
                _timeEntryTypes = value;
                OnPropertyChanged(() => TimeEntryTypes);
            }
        }

        private void SetSelectedTimeEntryType()
        {
            if (TimeEntryTypes == null)
            {
                return;
            }

            if (_userWlanSettingsService.BoundToWLan) //override always
            {
                SelectedTimeEntryType = TimeEntryTypes.FirstOrDefault(tt => tt.Id == _userWlanSettingsService.UserWLanTimeEntryTypeId);
            }
            else if (_isEditing && _timeEntry.TimeEntryType != null) //display its current value
            {
                SelectedTimeEntryType = TimeEntryTypes.FirstOrDefault(tt => tt.Id == _timeEntry.TimeEntryType.Id);
            }
            else if (_appSettings.UserDefaultTimeEntryTypeId.HasValue) //get user default
            {
                SelectedTimeEntryType = TimeEntryTypes.FirstOrDefault(tt => tt.Id == _appSettings.UserDefaultTimeEntryTypeId);
            }

            if (SelectedTimeEntryType == null) // get system default
            {
                SelectedTimeEntryType = TimeEntryTypes.First(tt => tt.IsDefault);
            }
            else //enforce reference
            {
                SelectedTimeEntryType = TimeEntryTypes.FirstOrDefault(tt => tt.Id == SelectedTimeEntryType.Id);
            }
        }

        public bool TimeEntryViewTimeSpentSelected
        {
            get { return _appSettings.TimeEntryViewTimeSpentSelected; }
            set
            {
                _appSettings.TimeEntryViewTimeSpentSelected = value;
                _appSettings.Save();
                OnPropertyChanged(() => TimeEntryViewTimeSpentSelected);
            }
        }

        public bool TimeEntryViewPeriodSelected
        {
            get { return _appSettings.TimeEntryViewPeriodSelected; }
            set
            {
                _appSettings.TimeEntryViewPeriodSelected = value;
                _appSettings.Save();
                OnPropertyChanged(() => TimeEntryViewPeriodSelected);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (taskStoppedCommand != null) TaskCommands.TaskIdle.UnregisterCommand(taskStoppedCommand);
            if (executeSelectTaskCommand != null) TaskCommands.TaskSelectCompleted.UnregisterCommand(executeSelectTaskCommand);
            base.Dispose(disposing);
        }
    }
}
