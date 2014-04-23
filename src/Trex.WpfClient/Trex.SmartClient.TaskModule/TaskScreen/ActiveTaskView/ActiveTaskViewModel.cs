using System;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Infrastructure.Extensions;
using Trex.SmartClient.Infrastructure.Implemented;
using Trex.SmartClient.TaskModule.TaskScreen.TaskToolTipView;

namespace Trex.SmartClient.TaskModule.TaskScreen.ActiveTaskView
{
    public class ActiveTaskViewModel : ViewModelBase
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IApplicationStateService _applicationStateService;

        public DelegateCommand<object> Start { get; set; }
        public DelegateCommand<object> Stop { get; set; }
        public DelegateCommand<object> Pause { get; set; }
        public DelegateCommand<object> CloseActiveTask { get; set; }
        public DelegateCommand<object> DeactivateActiveTask { get; set; }
        public DelegateCommand<object> ReAssign { get; set; }
        public DelegateCommand<object> OpenDesktopTask { get; set; }
        public DelegateCommand<object> SaveCommand { get; set; }
        public DelegateCommand<object> ToggleState { get; set; }
        public DelegateCommand<TimeEntry> TaskAssigned { get; set; }
        public DelegateCommand<TimeEntry> TaskSaved { get; set; }

        public TimeEntry TimeEntry { get; private set; }

        public ActiveTaskViewModel(TimeEntry timeEntry, bool startImmediately, ITimeEntryRepository timeEntryRepository,
            IApplicationStateService applicationStateService)
        {
            _timeEntryRepository = timeEntryRepository;
            _applicationStateService = applicationStateService;

            Application.Current.Exit += OnApplicationShutdown;

            Start = new DelegateCommand<object>(ExecuteStart, CanStart);
            Stop = new DelegateCommand<object>(ExecuteStop, CanStop);
            Pause = new DelegateCommand<object>(ExecutePause, CanPause);
            ReAssign = new DelegateCommand<object>(ExecuteReassign);
            OpenDesktopTask = new DelegateCommand<object>(ExecuteOpenDesktopTask);
            CloseActiveTask = new DelegateCommand<object>(ExecuteCloseActiveTask);
            DeactivateActiveTask = new DelegateCommand<object>(ExecuteDeactivateTask);
            SaveCommand = new DelegateCommand<object>(ExecuteSave);
            ToggleState = (new DelegateCommand<object>(ExecuteToggleState));
            TaskAssigned = (new DelegateCommand<TimeEntry>(ExecuteTaskAssigned));
            TaskSaved = new DelegateCommand<TimeEntry>(ExecuteTaskSaved);



            TaskCommands.PauseActiveTask.RegisterCommand(Pause);
            //TaskCommands.SaveActiveTask.RegisterCommand(new DelegateCommand<object>(SaveTask, CanSaveTask));
            TaskCommands.ToggleActiveTask.RegisterCommand(ToggleState);
            TaskCommands.TaskAssigned.RegisterCommand(TaskAssigned);
            TaskCommands.StopActiveTask.RegisterCommand(Stop);
            TaskCommands.SaveTaskCompleted.RegisterCommand(TaskSaved);
            TaskCommands.AssignTask.RegisterCommand(ReAssign);

            InitializeTimer(timeEntry);
            Update();

            if (startImmediately)
            {
                ExecuteStart(null);
            }
            else
            {
                Console.WriteLine(TimerService.State);
                _applicationStateService.CurrentState.ActiveTaskState = TimerService.State;
            }
        }

        /// <summary>
        /// Pause if task is running when application is shutting down.
        /// Or else time will be lost when task is restarted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationShutdown(object sender, ExitEventArgs e)
        {
            if (TimerService.State == TimerState.Running)
                ExecutePause(null);
        }

        private void ExecuteSave(object obj)
        {
            TaskCommands.SaveTaskStart.Execute(TimeEntry);
        }

        private void ExecuteTaskSaved(TimeEntry timeEntry)
        {
            if (timeEntry.Guid == TimeEntry.Guid)
            {
                InitializeTimer(null);
                Update();
                ApplicationCommands.UpdateDesktopWindow.Execute(TimerService);
            }
        }

        private void ExecuteDeactivateTask(object obj)
        {
            TaskCommands.DeactivateActiveTask.Execute(null);
        }

        private void ExecuteCloseActiveTask(object obj)
        {
            //If task is stopped, but not saved, show dialog
            if ((!_timeEntryRepository.Exists(TimeEntry) && TimeEntry.IsStopped) || !TimeEntry.IsStopped && TimeEntry.TimeSpent != TimeSpan.Zero)
            {
                var result = MessageBox.Show("Current timeentry has not been saved. Close the task?", "Unsaved timeentry", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    TaskCommands.CloseActiveTask.Execute(null);
                }
            }
            else
            {
                TaskCommands.CloseActiveTask.Execute(null);
            }
        }

        private void ExecuteTaskAssigned(TimeEntry obj)
        {
            Update();
        }

        private void ExecuteToggleState(object obj)
        {
            if (TimerService.State == TimerState.Running)
            {
                ExecutePause(null);
            }
            else
            {
                ExecuteStart(null);
            }
        }

        private void InitializeTimer(TimeEntry timeEntry)
        {
            //if the given is null, create a new timer based on the existing timeentry (with same task and project)
            if (timeEntry == null)
            {
                TimeEntry = TimeEntry.Create(TimeEntry);
            }
            //If time is zero, start a new timer based on the given timeentry (with same task and project)
            else if (timeEntry.TimeSpent == TimeSpan.Zero)
            {
                TimeEntry = TimeEntry.Create(timeEntry);
            }
            //Else just continue the timer on the current timeentry
            else
            {
                TimeEntry = timeEntry;
            }
            if (TimerService != null)
            {
                TimerService.Dispose();
                TimerService = null;
            }
            if (ToolTipViewModel == null)
            {
                ToolTipViewModel = new TaskToolTipViewModel(TimeEntry);
            }
            else
            {
                ToolTipViewModel.Update(TimeEntry);
            }
            _applicationStateService.CurrentState.ActiveTimeEntry = TimeEntry;
            _applicationStateService.Save();

            TimerService = new TimeEntryTimerService(TimeEntry);
            TimerService.TimeEntryUpdated += _timeEntryTimerService_TimeEntryUpdated;
            TimerService.TimerStateChanged += _timeEntryTimerService_TimerStateChanged;
        }


        private void ExecuteOpenDesktopTask(object obj)
        {
            ApplicationCommands.LoadDesktopTask.Execute(TimerService);
        }

        private void _timeEntryTimerService_TimerStateChanged(object sender, EventArgs e)
        {
            Start.RaiseCanExecuteChanged();
            Stop.RaiseCanExecuteChanged();
            Pause.RaiseCanExecuteChanged();
        }

        private void ExecuteReassign(object obj)
        {
            TaskCommands.SaveTaskStart.Execute(TimeEntry);
        }

        private bool CanPause(object arg)
        {
            return TimerService.State == TimerState.Running;
        }

        private void ExecutePause(object obj)
        {
            TimerService.Pause();
            _applicationStateService.CurrentState.ActiveTimeEntry = TimeEntry;
            _applicationStateService.CurrentState.ActiveTaskState = TimerState.Paused;
            TaskCommands.TaskIdle.Execute(TaskName);
            Update();
        }

        private bool CanStop(object arg)
        {
            return TimerService.State == TimerState.Running ||
                   TimerService.State == TimerState.Paused;
        }

        private void ExecuteStop(object obj)
        {
            TimerService.Stop();
            _applicationStateService.Save();
            _applicationStateService.CurrentState.ActiveTaskState = TimerState.Stopped;
            TaskCommands.TaskIdle.Execute(TaskName);
            TaskCommands.SaveTaskStart.Execute(TimeEntry);
            Update();
        }

        private bool CanStart(object arg)
        {
            return TimerService.State == TimerState.Stopped
                || TimerService.State == TimerState.Paused;
        }

        private void ExecuteStart(object obj)
        {
            if (TimerService.State == TimerState.Stopped)
            {
                //If task is stopped, but not saved, show dialog
                if (!_timeEntryRepository.Exists(TimeEntry) && TimeEntry.IsStopped && TimeEntry.TimeSpent != TimeSpan.Zero)
                {
                    var result = MessageBox.Show("Current timeentry has not been saved. Start new task?", "Unsaved timeentry", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        TaskCommands.SaveTaskStart.Execute(TimeEntry);
                        return;
                    }
                }
                InitializeTimer(null);
            }
            else if (TimeEntry.TimeSpent == TimeSpan.Zero)
            {
                InitializeTimer(null);
            }

            TimerService.Start();
            _applicationStateService.CurrentState.ActiveTaskState = TimerState.Running;
            TaskCommands.TaskStarted.Execute(TimeEntry);
        }

        private void _timeEntryTimerService_TimeEntryUpdated(object sender, EventArgs e)
        {
            OnPropertyChanged(() => TimeSpent);
            OnPropertyChanged(() => StartedText);
            OnPropertyChanged(() => IsStopped);
            OnPropertyChanged(() => IsRunning);
            if (IsStopped)
            {
                Update();
            }

            ToolTipViewModel.Update(TimeEntry);
            OnPropertyChanged(() => ToolTipViewModel);
        }

        private void Update()
        {
            OnPropertyChanged(() => TimeSpent);
            OnPropertyChanged(() => Project);
            OnPropertyChanged(() => StartedText);
            OnPropertyChanged(() => IsStopped);
            OnPropertyChanged(() => IsRunning);
            OnPropertyChanged(() => IsSaved);
            OnPropertyChanged(() => TaskName);

            ToolTipViewModel.Update(TimeEntry);
            OnPropertyChanged(() => ToolTipViewModel);
        }

        public string StartedText
        {
            get { return TimeEntry.StartTime.ToShortDateAndTimeString(); }
        }

        public string Project
        {
            get
            {
                return TimeEntry.Task != null ? TimeEntry.Task.Project.Name : string.Empty;
            }
        }

        public bool IsSaved
        {
            get { return TimeEntry.Task != null; }
        }

        public string TaskName
        {
            get
            {
                return IsSaved ? TimeEntry.Task.Name : TimeEntry.TempoaryTaskName;
            }
            set
            {
                TimeEntry.TempoaryTaskName = value;
                OnPropertyChanged(() => TaskName);
            }
        }

        public bool IsRunning
        {
            get { return !IsStopped; }
        }

        public bool IsStopped
        {
            get { return TimeEntry.IsStopped; }
        }

        public TimeSpan TimeSpent
        {
            get { return TimeEntry.TimeSpent; }
        }

        public TimeEntryTimerService TimerService { get; private set; }


        private TaskToolTipViewModel _toolTipViewModel;

        public TaskToolTipViewModel ToolTipViewModel
        {
            get { return _toolTipViewModel; }
            set
            {
                _toolTipViewModel = value;
                OnPropertyChanged(() => ToolTipViewModel);
            }
        }


        protected override void Dispose(bool disposing)
        {
            TaskCommands.PauseActiveTask.UnregisterCommand(Pause);
            TaskCommands.ToggleActiveTask.UnregisterCommand(ToggleState);
            TaskCommands.StopActiveTask.UnregisterCommand(Stop);
            TaskCommands.TaskAssigned.UnregisterCommand(TaskAssigned);
            TaskCommands.SaveTaskCompleted.UnregisterCommand(TaskAssigned);
            TaskCommands.AssignTask.UnregisterCommand(ReAssign);

            TimerService.Dispose();
        }
    }
}
