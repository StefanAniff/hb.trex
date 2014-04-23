using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Trex.SmartClient.Core.Model
{
    [DataContract]
    public class ApplicationState : INotifyPropertyChanged
    {
        public ApplicationState()
        {
            OpenTimeEntries = new ObservableCollection<TimeEntry>();
        }

        private ObservableCollection<TimeEntry> _openTimeEntries;
        [DataMember]
        public ObservableCollection<TimeEntry> OpenTimeEntries
        {
            get { return _openTimeEntries; }
            set
            {
                _openTimeEntries = value;
                OnPropertyChanged("OpenTimeEntries");
            }
        }

        private TimeEntry _activeTimeEntry;

        [DataMember]
        public TimeEntry ActiveTimeEntry
        {
            get { return _activeTimeEntry; }
            set
            {
                _activeTimeEntry = value;
                OnPropertyChanged("ActiveTimeEntry");
            }
        }

        private TimerState _activeTaskState;
        public TimerState ActiveTaskState
        {
            get { return _activeTaskState; }
            set
            {
                _activeTaskState = value;
                OnPropertyChanged("ActiveTimeEntry");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
