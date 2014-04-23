using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Infrastructure.Implemented;
using Trex.SmartClient.TaskModule.Interfaces;
using Trex.SmartClient.TaskModule.TaskScreen.ActiveTaskView;
using Trex.SmartClient.TaskModule.TaskScreen.DesktopTaskView;
using Trex.SmartClient.TaskModule.TaskScreen.InactiveTaskView;
using ViewModelBase = Trex.SmartClient.Core.Implemented.ViewModelBase;

namespace Trex.SmartClient.TaskModule.TaskScreen.TaskScreenMasterView
{
    public class TaskScreenMasterViewModel : ViewModelBase, ITaskScreenMasterViewModel
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IApplicationStateService _applicationStateService;
        private readonly IAppSettings _appSettings;

        public DelegateCommand<object> StartNewTask { get; set; }
        public ObservableCollection<InActiveTaskViewModel> InactiveTasks { get; set; }

        public double TabHistoryHeight
        {
            get { return _appSettings.TabHistoryHeight; }
            set
            {
                _appSettings.TabHistoryHeight = value;
                _appSettings.Save();
            }
        }

        public double ActiveTaskX
        {
            get { return _appSettings.ActiveTaskPositionX; }
            set
            {
                _appSettings.ActiveTaskPositionX = value;
                _appSettings.Save();
            }
        }

        public double ActiveTaskY
        {
            get { return _appSettings.ActiveTaskPositionY; }
            set
            {
                _appSettings.ActiveTaskPositionY = value;
                _appSettings.Save();
            }
        }

        public TaskScreenMasterViewModel(ITimeEntryRepository timeEntryRepository, IApplicationStateService applicationStateService, IAppSettings appSettings)
        {
            _appSettings = appSettings;
            StartNewTask = new DelegateCommand<object>(ExecuteStartNewTask);
            InactiveTasks = new ObservableCollection<InActiveTaskViewModel>();
            ApplicationCommands.LoadDesktopTask.RegisterCommand(new DelegateCommand<TimeEntryTimerService>(LoadDesktopTask));
            TaskCommands.ActivateTask.RegisterCommand(new DelegateCommand<TimeEntry>(ActivateTask));
            TaskCommands.TaskStarted.RegisterCommand(new DelegateCommand<TimeEntry>(TaskStarted));
            TaskCommands.StartNewTask.RegisterCommand(StartNewTask);
            TaskCommands.CloseInactiveTask.RegisterCommand(new DelegateCommand<InActiveTaskViewModel>(CloseInactiveTask));
            TaskCommands.CloseActiveTask.RegisterCommand(new DelegateCommand<object>(CloseActiveTask));
            TaskCommands.DeactivateActiveTask.RegisterCommand(new DelegateCommand<object>(DeactivateActiveTask));
            TaskCommands.TaskAssigned.RegisterCommand(new DelegateCommand<object>(TaskAssigned));
            TaskCommands.CloseAllInactiveTasks.RegisterCommand(new DelegateCommand<object>(CloseAllInactiveTasksExecute));
            ApplicationCommands.DeskTopWindowClosed.RegisterCommand(new DelegateCommand<object>(DesktopWindowClosed));
            ApplicationCommands.UpdateDesktopWindow.RegisterCommand(new DelegateCommand<TimeEntryTimerService>(UpdateDesktopWindow));
            ApplicationCommands.InActiveTaskLayoutChanged.RegisterCommand(new DelegateCommand(UpdateInactiveTaskLayout));

            _timeEntryRepository = timeEntryRepository;
            _applicationStateService = applicationStateService;
            LoadState();
        }

        private void UpdateInactiveTaskLayout()
        {
            foreach (var inActiveTaskViewModel in InactiveTasks)
            {
                var inActiveTaskWidth = _appSettings.InActiveTaskWidth;
                var inactiveTaskHeight = _appSettings.InactiveTaskHeight;
                var timeSpentFontSize = _appSettings.InactiveTaskTimeSpentFontSize;
                var taskNameFontSize = _appSettings.InactiveTaskTaskNameFontSize;
                var detailsFontSize = _appSettings.InactiveTaskDescriptionFontSize;
                inActiveTaskViewModel.UpdateLayout(inactiveTaskHeight, inActiveTaskWidth, timeSpentFontSize,
                    taskNameFontSize, detailsFontSize);
            }
            OnPropertyChanged(() => ActiveTaskX);
            OnPropertyChanged(() => ActiveTaskY);
        }

        private InActiveTaskViewModel Create(TimeEntry timeEntry)
        {
            var inActiveTaskWidth = _appSettings.InActiveTaskWidth;
            var inactiveTaskHeight = _appSettings.InactiveTaskHeight;
            double timeSpentFontSize = _appSettings.InactiveTaskTimeSpentFontSize;
            double taskNameFontSize = _appSettings.InactiveTaskTaskNameFontSize;
            double detailsFontSize = _appSettings.InactiveTaskDescriptionFontSize;
            var inActiveTaskViewModel = new InActiveTaskViewModel(timeEntry,
                                                                  _timeEntryRepository.Exists(timeEntry),
                                                                  inActiveTaskWidth,
                                                                  inactiveTaskHeight,
                                                                  timeSpentFontSize,
                                                                  taskNameFontSize,
                                                                  detailsFontSize);

            return inActiveTaskViewModel;
        }


        private void CloseAllInactiveTasksExecute(object obj)
        {
            foreach (var inactiveTask in InactiveTasks.Where(x => x.IsSaved).ToList())
            {
                RemoveFromInactive(inactiveTask.TimeEntry);
            }
        }

        private void UpdateDesktopWindow(TimeEntryTimerService obj)
        {
            if (ActiveDeskTopTask != null)
            {
                ActiveDeskTopTask.ApplyViewModel(new DesktopTaskViewModel(obj));
            }
        }

        private void TaskAssigned(object obj)
        {
            _applicationStateService.Save();
        }

        private void DeactivateActiveTask(object obj)
        {
            InactiveTasks.Add(Create(ActiveTask.TimeEntry));

            _applicationStateService.AddOpenTimeEntry(ActiveTask.TimeEntry);
            TaskCommands.PauseActiveTask.Execute(null);
            CloseActiveTask(null);
        }

        private void CloseActiveTask(object obj)
        {
            ActiveTask.Dispose();
            ActiveTask = null;
            //if(ActiveDeskTopTask != null)
            //{

            //    ActiveDeskTopTask.Close();
            //    ActiveDeskTopTask = null;
            //}
        }

        private void LoadState()
        {
            var timeEntries = _applicationStateService.CurrentState.OpenTimeEntries;
            InactiveTasks = new ObservableCollection<InActiveTaskViewModel>(timeEntries.Select(Create));

            var activeTimeEntry = _applicationStateService.CurrentState.ActiveTimeEntry;
            if (activeTimeEntry != null)
            {
                var startImmediately = activeTimeEntry.TimeSpent == TimeSpan.Zero && _appSettings.StartTaskWhenApplicationStarts;
                ExecuteStartTaskInternal(activeTimeEntry, startImmediately);
            }
            else if (_appSettings.StartTaskWhenApplicationStarts)
            {
                ExecuteStartNewTask(null);

            }
        }

        private void DesktopWindowClosed(object obj)
        {
            ActiveDeskTopTask = null;

        }

        private void CloseInactiveTask(InActiveTaskViewModel obj)
        {
            RemoveFromInactive(obj.TimeEntry);
        }


        private void ActivateTask(TimeEntry obj)
        {
            ExecuteStartTaskInternal(obj, _appSettings.StartTaskWhenActivated);
            RemoveFromInactive(obj);


        }

        private void RemoveFromInactive(TimeEntry entry)
        {
            var itemToRemove = InactiveTasks.FirstOrDefault(vm => vm.TimeEntry.Guid == entry.Guid);
            if (itemToRemove != null)
            {
                InactiveTasks.Remove(itemToRemove);
                _applicationStateService.RemoveOpenTimeEntry(itemToRemove.TimeEntry);
            }
        }


        private void LoadDesktopTask(TimeEntryTimerService timeEntryTimerService)
        {

            var isOpen = ActiveDeskTopTask != null;


            if (!isOpen)
            {
                ActiveDeskTopTask = new DesktopTaskView.DesktopTaskView();
                ActiveDeskTopTask.Show();
            }
            ActiveDeskTopTask.ApplyViewModel(new DesktopTaskViewModel(timeEntryTimerService));
        }



        private void ExecuteStartNewTask(object obj)
        {

            ExecuteStartTaskInternal(null, true);
        }

        private void ExecuteStartTaskInternal(TimeEntry timeEntry, bool startImmediately)
        {
            if (ActiveTask != null)
            {
                DeactivateActiveTask(null);

            }

            ActiveTask = new ActiveTaskViewModel(timeEntry, false, _timeEntryRepository, _applicationStateService);
            if (startImmediately)
            {
                ActiveTask.Start.Execute(null);
            }

        }

        private void TaskStarted(TimeEntry obj)
        {
            if (ActiveTask == null)
            {
                ExecuteStartTaskInternal(obj, true);
            }

            //If desktoptask is open, load the new task
            if (ActiveDeskTopTask != null)
            {
                LoadDesktopTask(ActiveTask.TimerService);
            }
        }


        private DesktopTaskView.DesktopTaskView ActiveDeskTopTask { get; set; }


        private ActiveTaskViewModel _activeTask;

        public ActiveTaskViewModel ActiveTask
        {
            get { return _activeTask; }
            set
            {
                _activeTask = value;
                _applicationStateService.CurrentState.ActiveTimeEntry = value != null ? value.TimeEntry : null;
                _applicationStateService.Save();
                OnPropertyChanged(() => ActiveTask);
                OnPropertyChanged(() => IsActiveTaskVisible);
            }
        }

        public bool IsActiveTaskVisible
        {
            get { return ActiveTask != null; }
        }
    }
}