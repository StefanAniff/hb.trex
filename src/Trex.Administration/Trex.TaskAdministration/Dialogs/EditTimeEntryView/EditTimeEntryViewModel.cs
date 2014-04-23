using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Infrastructure.Implemented;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.Dialogs.EditTimeEntryView
{
    public class EditTimeEntryViewModel : ViewModelBase
    {
        private readonly Task _task;
        private readonly IDataService _dataService;
        private readonly bool _isNew;


        public EditTimeEntryViewModel(TimeEntry timeEntry, Task task, IDataService dataService)
        {
            _isNew = timeEntry.TimeEntryID == 0;

            _task = task;

            TimeEntry = timeEntry;

            if (_isNew)
            {
                StartDate = DateTime.Now.Date;
                IsBillable = true;
                TimeEntry.DocumentType = 1;
            }
            if (task.Project.FixedPriceProject && task.Project.EstimatedHours != null)
            {
                PricePrHour = Math.Round((double)(task.Project.FixedPrice/task.Project.EstimatedHours), 2);
            }
            else if (UserContext.Instance.User.GetRateForCustomer(_task.Project.Customer) != TimeEntry.Price && TimeEntry.Price != 0)
                PricePrHour = Math.Round(TimeEntry.Price, 2);
            else
                PricePrHour = UserContext.Instance.User.GetRateForCustomer(_task.Project.Customer);


            _dataService = dataService;
            SaveCommand = new DelegateCommand<TimeEntry>(SaveTimeEntry, CanSaveTimeEntry);
            CancelCommand = new DelegateCommand<object>(CancelEdit);

            TimeEntryTypes = new ObservableCollection<TimeEntryType>();

            LoadTimeEntryTypes(timeEntry);
        }

        private void LoadTimeEntryTypes(TimeEntry timeEntry)
        {
            if (_task.Project.Customer.InheritsTimeEntryTypes)
            {
                _dataService.GetGlobalTimeEntryTypes().Subscribe(
                    timeEntryTypes =>
                    {
                        foreach (var timeEntryType in timeEntryTypes)
                        {
                            TimeEntryTypes.Add(timeEntryType);
                        }

                        if (timeEntry.TimeEntryType == null)
                            SelectedTimeEntryType = TimeEntryTypes.FirstOrDefault(tt => tt.IsDefault);

                        //SelectedTimeEntryType =
                        //    TimeEntryTypes.SingleOrDefault(
                        //        tt => tt.TimeEntryTypeId == TimeEntry.TimeEntryTypeId);
                        OnPropertyChanged("SelectedTimeEntryType");


                    }
                    );
            }

            else
            {
                TimeEntryTypes = _task.Project.Customer.TimeEntryTypes;
            }
        }

        public TimeEntry TimeEntry { get; set; }

        public string DisplayTitle
        {
            get
            {
                if (_task != null)
                {
                    return string.Concat(_task.TaskName, " ", TimeEntry.StartTime.ToShortDateString());
                }
                return "Edit Task";
            }
        }

        public DateTime? StartDate
        {
            get { return TimeEntry.StartTime; }
            set
            {
                if (value.HasValue)
                {
                    if (value.Value != TimeEntry.StartTime)
                    {
                        TimeEntry.StartTime = value.Value;
                        TimeEntry.EndTime = value.Value;
                        OnPropertyChanged("StartDate");
                    }
                }
            }
        }

        public string TimeSpentFormatted
        {
            get { return TimeEntry.TimeSpent.ToString("N2"); }
        }

        public string BillableTime
        {
            get
            {
                if (TimeEntry.BillableTime != TimeEntry.TimeSpent)
                {
                    return TimeEntry.BillableTime.ToString("N2");
                }
                return "Auto";
            }
            set
            {
                double billableTime;
                if (!double.TryParse(value, out billableTime)) return;
                TimeEntry.BillableTime = billableTime;
                OnPropertyChanged("BillableTime");
            }
        }

        public string TimeSpent
        {
            get { return TimeEntry.TimeSpent.ToString("N2"); }
            set
            {
                double timeSpent;
                if (double.TryParse(value, out timeSpent))
                {
                    TimeEntry.TimeSpent = timeSpent;
                    TimeEntry.BillableTime = timeSpent;
                    OnPropertyChanged("TimeSpent");
                    OnPropertyChanged("TimeSpentNumeric");
                    OnPropertyChanged("TimeSpentFormatted");
                    OnPropertyChanged("BillableTime");
                }
            }
        }

        public double TimeSpentNumeric
        {
            get { return TimeEntry.TimeSpent; }
            set
            {
                TimeEntry.TimeSpent = value;
                TimeSpent = value.ToString();
                OnPropertyChanged("TimeSpentNumeric");
            }
        }

        public string Description
        {
            get { return TimeEntry.Description; }
            set
            {
                TimeEntry.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public bool IsBillable
        {
            get { return TimeEntry.Billable; }
            set
            {
                TimeEntry.Billable = value;
                OnPropertyChanged("IsBillable");
            }
        }

        public double PricePrHour
        {
            get { return TimeEntry.Price; }
            set
            {
                TimeEntry.Price = value;
                OnPropertyChanged("PricePrHour");
            }
        }

        public TimeEntryType SelectedTimeEntryType
        {
            get
            {
                return

                    TimeEntryTypes.SingleOrDefault(tt => tt.TimeEntryTypeId == TimeEntry.TimeEntryTypeId);
            }
            set
            {
                if (TimeEntry.TimeEntryTypeId != value.TimeEntryTypeId)
                    TimeEntry.TimeEntryTypeId = value.TimeEntryTypeId;
                //if (value != null)
                //    IsBillable = value.IsBillableByDefault;
                OnPropertyChanged("SelectedTimeEntryType");
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<TimeEntryType> TimeEntryTypes { get; set; }

        public DelegateCommand<TimeEntry> SaveCommand { get; set; }
        public DelegateCommand<object> CancelCommand { get; set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        private bool CanSaveTimeEntry(TimeEntry arg)
        {
            return TimeEntry.TimeEntryTypeId != 0;
        }

        private void SaveTimeEntry(TimeEntry obj)
        {
            //Clear the object graph, in order to save bandwidth
            var task = TimeEntry.Task;
            TimeEntry.Task = null;
            TimeEntry.ChangeTracker.OriginalValues.Clear();

            _dataService.SaveTimeEntry(TimeEntry).Subscribe(
                timeEntry =>
                {

                    if (_isNew)
                    {

                        TimeEntry.TimeEntryID = timeEntry.TimeEntryID;
                        TimeEntry.CreateDate = timeEntry.CreateDate;
                        TimeEntry.ChangeDate = timeEntry.ChangeDate;
                        TimeEntry.Task = task;
                        TimeEntry.AcceptChanges();
                        InternalCommands.TimeEntryAddCompleted.Execute(TimeEntry);

                    }
                    else
                    {
                        TimeEntry.ChangeDate = timeEntry.ChangeDate;
                        TimeEntry.Task = task;
                        TimeEntry.AcceptChanges();

                        InternalCommands.TimeEntryEditCompleted.Execute(TimeEntry);
                    }

                    _dataService.UpdateTimeEntryPrice(TimeEntry.Task.ProjectID).Subscribe();
                }


                );
        }

        private void CancelEdit(object obj)
        {
            TimeEntry.CancelChanges();
            InternalCommands.TimeEntryEditCompleted.Execute(null);
        }
    }
}