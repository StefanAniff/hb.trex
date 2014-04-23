using System.Collections.ObjectModel;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Model.Consts;

namespace Trex.SmartClient.TaskModule.TaskScreen.TaskToolTipView
{
    public class TaskToolTipViewModel : ViewModelBase, ITaskToolTipViewModel
    {
        private TimeEntry _timeEntry;

        public TaskToolTipViewModel(TimeEntry timeEntry)
        {
            Update(timeEntry);
        }

        private ObservableCollection<TimeInterval> _taskHistory;

        public ObservableCollection<TimeInterval> TaskHistory
        {
            get { return _taskHistory; }
            private set
            {
                _taskHistory = value;
                OnPropertyChanged(() => TaskHistory);
            }
        }


        public string CustomerName
        {
            get
            {
                if (_timeEntry.Task != null)
                {
                    if (_timeEntry.Task.Project != null)
                    {
                        if (_timeEntry.Task.Project.Company != null)
                        {
                            return _timeEntry.Task.Project.Company.Name;
                        }
                    }
                }
                return string.Empty;
            }
        }


        public string TaskName
        {
            get 
            {
                return _timeEntry.Task != null ? _timeEntry.Task.Name : HelperTextConsts.UnassignedTask;
            }
        }

        public string ProjectName
        {
            get
            {
                if (_timeEntry.Task != null)
                {
                    if (_timeEntry.Task.Project != null)
                    {
                        return _timeEntry.Task.Project.Name;
                    }
                }
                return string.Empty;
            }
        }

        public bool IsAssigned
        {
            get { return _timeEntry.Task != null; }
        }

        public void Update(TimeEntry timeEntry)
        {
            _timeEntry = timeEntry;
            OnPropertyChanged(() => TaskName);
            OnPropertyChanged(() => ProjectName);
            OnPropertyChanged(() => CustomerName);

            if (_timeEntry.TimeEntryHistory != null)
            {
                TaskHistory = new ObservableCollection<TimeInterval>(_timeEntry.TimeEntryHistory.TimeLog);
            }
        }
    }
}