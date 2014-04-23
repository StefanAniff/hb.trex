using Trex.Core.Implemented;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.TaskAdministration.TimeEntryTypeScreen.TimeEntryTypeView
{
    public class TimeEntryTypeViewModel : ViewModelBase
    {
        public TimeEntryTypeViewModel(TimeEntryType timeEntryType)
        {
            TimeEntryType = timeEntryType;
        }

        public TimeEntryType TimeEntryType { get; private set; }

        public bool IsBillableByDefault
        {
            get { return TimeEntryType.IsBillableByDefault; }
            set
            {
                TimeEntryType.IsBillableByDefault = value;
                OnPropertyChanged("IsBillableByDefault");
                OnPropertyChanged("BillableStatus");
            }
        }

        public string BillableStatus
        {
            get
            {
                if (TimeEntryType.IsBillableByDefault)
                {
                    return "Billable";
                }
                else
                {
                    return "Not billable";
                }
            }
        }

        public string Name
        {
            get
            {
                if (IsDefault)
                {
                    return string.Concat(TimeEntryType.Name, " (Default)");
                }

                return TimeEntryType.Name;
            }
            set
            {
                TimeEntryType.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public bool IsDefault
        {
            get { return TimeEntryType.IsDefault; }
            set
            {
                TimeEntryType.IsDefault = value;
                OnPropertyChanged("IsDefault");
                OnPropertyChanged("Name");
            }
        }

        public void Update()
        {
            OnPropertyChanged("IsBillableByDefault");
            OnPropertyChanged("BillableStatus");
            OnPropertyChanged("Name");
            OnPropertyChanged("IsDefault");
        }
    }
}