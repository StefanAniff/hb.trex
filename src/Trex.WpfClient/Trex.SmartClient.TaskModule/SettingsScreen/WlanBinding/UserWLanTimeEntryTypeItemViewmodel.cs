using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.TaskModule.SettingsScreen.WlanBinding
{
    public class UserWLanTimeEntryTypeItemViewmodel : ViewModelBase
    {
        public IUserWLanTimeEntryType UserWLanTimeEntryType { get; set; }

        private TimeEntryType _timeEntryTypes;

        public TimeEntryType SelectedTimeEntryType
        {
            get { return _timeEntryTypes; }
            set
            {
                _timeEntryTypes = value;
                UserWLanTimeEntryType.DefaultTimeEntryTypeId = !TimeEntryTypesIsSet ? 0 : _timeEntryTypes.Id;
                OnPropertyChanged(() => SelectedTimeEntryType);
            }
        }

        private bool TimeEntryTypesIsSet
        {
            get
            {
                return _timeEntryTypes != null;
            }
        }

        public string WifiName
        {
            get { return UserWLanTimeEntryType.WifiName; }
        }

        public bool Connected
        {
            get { return UserWLanTimeEntryType.Connected; }
        }

        public UserWLanTimeEntryTypeItemViewmodel(TimeEntryType timeEntryTypes, IUserWLanTimeEntryType userWLanTimeEntryType)
        {
            UserWLanTimeEntryType = userWLanTimeEntryType;
            _timeEntryTypes = timeEntryTypes;
        }
    }
}