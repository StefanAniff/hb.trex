using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.Resources;

namespace Trex.TaskAdministration.Dialogs.EditTimeEntryTypeView
{
    public class EditTimeEntryTypeViewModel : ViewModelBase
    {
        private readonly bool _isNew;
        
        private readonly TimeEntryType _timeEntryType;
        private IDataService _dataService;

        public EditTimeEntryTypeViewModel(IDataService dataService, TimeEntryType timeEntryType)
        {
        
            _timeEntryType = timeEntryType;

            _dataService = dataService;
            SaveTimeEntryType = new DelegateCommand<object>(ExecuteSaveTimeEntryType, CanSaveTimeEntryType);
        }

        public bool IsBillableByDefault
        {
            get { return _timeEntryType.IsBillableByDefault; }
            set
            {
                _timeEntryType.IsBillableByDefault = value;
                OnPropertyChanged("IsBillableByDefault");
            }
        }

        public string Name
        {
            get { return _timeEntryType.Name; }
            set
            {
                _timeEntryType.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public bool IsDefault
        {
            get { return _timeEntryType.IsDefault; }
            set
            {
                _timeEntryType.IsDefault = value;
                OnPropertyChanged("IsDefault");
            }
        }

        public string Title
        {
            get
            {
                if (_isNew)
                {
                    return EditTimeEntryTypeResources.WindowTitle;
                }
                else
                {
                    return _timeEntryType.Name;
                }
            }
        }

        public DelegateCommand<object> SaveTimeEntryType { get; set; }

        private bool CanSaveTimeEntryType(object arg)
        {
            return true;
        }

        private void ExecuteSaveTimeEntryType(object obj)
        {
            _dataService.SaveTimeEntryType(_timeEntryType);
            InternalCommands.EditTimeEntryTypeCompleted.Execute(_timeEntryType);
        }
    }
}