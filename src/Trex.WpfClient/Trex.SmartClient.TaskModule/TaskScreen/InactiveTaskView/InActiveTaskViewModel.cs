using System;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Infrastructure.Extensions;
using Trex.SmartClient.TaskModule.TaskScreen.TaskToolTipView;

namespace Trex.SmartClient.TaskModule.TaskScreen.InactiveTaskView
{
    public class InActiveTaskViewModel : ViewModelBase, IInActiveTaskViewModel
    {
        private readonly TimeEntry _timeEntry;
        private readonly bool _saved;

        public DelegateCommand<object> Activate { get; set; }
        public DelegateCommand<object> CloseInactiveTask { get; set; }
        public DelegateCommand<object> CloseAllInactiveTask { get; set; }

        public double TimeSpentFontSize { get; private set; }
        public double DetailsFontSize { get; private set; }
        public double TaskNameFontSize { get; private set; }
        public double Height { get; private set; }
        public double Width { get; private set; }

        public InActiveTaskViewModel(TimeEntry timeEntry, bool exists, double width, double height, double timeSpentFontSize, double taskNameFontSize, double detailsFontSize)
        {
            Width = width;
            Height = height;
            TimeSpentFontSize = timeSpentFontSize;
            TaskNameFontSize = taskNameFontSize;
            DetailsFontSize = detailsFontSize;

            _saved = exists;
            _timeEntry = timeEntry.Copy();
            Activate = new DelegateCommand<object>(ExecuteActivate);
            CloseInactiveTask = new DelegateCommand<object>(CloseInactiveTaskExecute);
            CloseAllInactiveTask = new DelegateCommand<object>(CloseAllInactiveTaskExecute);
        }

        private void CloseAllInactiveTaskExecute(object obj)
        {
            var messageBoxResult = MessageBox.Show("Are you sure you want to remove all inactive saved tasks?",
                                                   "Confirm", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                TaskCommands.CloseAllInactiveTasks.Execute(null);
            }
        }

        private void CloseInactiveTaskExecute(object obj)
        {
            TaskCommands.CloseInactiveTask.Execute(this);
        }

        private void ExecuteActivate(object obj)
        {
            TaskCommands.ActivateTask.Execute(_timeEntry);
        }

        public TimeEntry TimeEntry
        {
            get { return _timeEntry; }
        }

        public bool IsAssigned
        {
            get { return _timeEntry.Task != null; }
        }

        public string StartDate
        {
            get { return _timeEntry.StartTime.ToShortDateAndTimeString(); }

        }

        public string PauseDate
        {
            get { return string.Empty; }
        }

        public string TaskName
        {
            get { return _timeEntry.Task != null ? _timeEntry.Task.Name : _timeEntry.TempoaryTaskName; }
        }

        public bool IsSaved
        {
            get { return _saved || _timeEntry.TimeSpent == TimeSpan.Zero; }
        }

        public string ProjectName
        {
            get
            {
                if (_timeEntry.Task != null && _timeEntry.Task.Project != null)
                {
                    return _timeEntry.Task.Project.Name;
                }
                return string.Empty;
            }
        }

        public string CustomerName
        {
            get
            {
                if (_timeEntry.Task != null && _timeEntry.Task.Project != null)
                {
                    return _timeEntry.Task.Project.Company.Name;
                }
                return string.Empty;
            }
        }

        public TimeSpan TimeSpent
        {
            get { return _timeEntry.TimeSpent; }
        }

        public TaskToolTipViewModel ToolTipViewModel
        {
            get { return new TaskToolTipViewModel(_timeEntry); }
        }

        public void UpdateLayout(double height, double width, double timeSpentFontSize, double taskNameFontSize, double detailsFontSize)
        {
            Height = height;
            Width = width;
            TimeSpentFontSize = timeSpentFontSize;
            TaskNameFontSize = taskNameFontSize;
            DetailsFontSize = detailsFontSize;
            OnPropertyChanged(() => Height);
            OnPropertyChanged(() => Width);

            OnPropertyChanged(() => TimeSpentFontSize);
            OnPropertyChanged(() => TaskNameFontSize);
            OnPropertyChanged(() => DetailsFontSize);
        }
    }
}