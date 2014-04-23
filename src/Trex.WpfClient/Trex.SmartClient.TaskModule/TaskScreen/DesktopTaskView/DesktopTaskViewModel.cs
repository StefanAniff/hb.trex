using System;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Model.Consts;
using Trex.SmartClient.Infrastructure.Extensions;
using Trex.SmartClient.Infrastructure.Implemented;
using Trex.SmartClient.TaskModule.TaskScreen.TaskToolTipView;

namespace Trex.SmartClient.TaskModule.TaskScreen.DesktopTaskView
{
    public class DesktopTaskViewModel:ViewModelBase
    {
        private readonly TimeEntryTimerService _timeEntryTimerService;
        private readonly TimeEntry _timeEntry;

        public DesktopTaskViewModel(TimeEntryTimerService timeEntryTimerService)
        {
            _timeEntryTimerService = timeEntryTimerService;
            _timeEntryTimerService.TimeEntryUpdated += new EventHandler(_timeEntryTimerService_TimeEntryUpdated);
            _timeEntry = _timeEntryTimerService.TimeEntry;
            ToolTipViewModel = new TaskToolTipViewModel(_timeEntry);

            
            
        }


        void  Update()
        {
            OnPropertyChanged("StartDate");
            OnPropertyChanged("PauseDate");
            OnPropertyChanged("TaskName");
            OnPropertyChanged("TimeSpent");
        }

        void _timeEntryTimerService_TimeEntryUpdated(object sender, EventArgs e)
        {
          Update();
        }

        private TaskToolTipViewModel _toolTipViewModel;
        public TaskToolTipViewModel ToolTipViewModel
        {
            get { return _toolTipViewModel; }
            set
            {
                _toolTipViewModel = value;
                OnPropertyChanged("ToolTipViewModel");
            }
        }


        public string StartDate
        {
            get
            {
                return _timeEntry.StartTime.ToShortDateAndTimeString();
            }

        }

        public string PauseDate
        {
            get
            {
                return string.Empty;
            }
        }

        public string TaskName
        {
            get
            {
                if (_timeEntry.Task != null)
                    return _timeEntry.Task.Name;
                else
                {
                    return HelperTextConsts.UnassignedTask;
                }

            }
        }

        public TimeSpan TimeSpent
        {
            get
            {
                return _timeEntry.TimeSpent;
            }
        }
    }
}
