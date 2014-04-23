using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.Resources;
using Trex.TaskAdministration.TimeEntryTypeScreen.TimeEntryTypeListView;

namespace Trex.TaskAdministration.Dialogs.EditCustomerTimeEntryTypesView
{
    public class EditCustomerTimeEntryTypesViewModel : ViewModelBase
    {
        private readonly Customer _customer;
        private readonly IDataService _dataService;
       

        private TimeEntryTypeListViewModel _timeEntryTypeListViewModel;

        public EditCustomerTimeEntryTypesViewModel(IDataService dataService, Customer customer)
        {
            _customer = customer;
            
            _dataService = dataService;
            TimeEntryTypeListViewModel = new TimeEntryTypeListViewModel(dataService, _customer);
            TimeEntryTypeListViewModel.DataReady += TimeEntryTypeListViewModel_DataReady;
            SaveTimeEntryTypes = new DelegateCommand<object>(ExecuteSaveTimeEntryTypes, CanSaveTimeEntryTypes);
            InternalCommands.CreateTimeEntryTypeCompleted.RegisterCommand(new DelegateCommand<TimeEntryType>(CreateTimeEntryTypeCompleted));
        }

        public TimeEntryTypeListViewModel TimeEntryTypeListViewModel
        {
            get { return _timeEntryTypeListViewModel; }
            set
            {
                _timeEntryTypeListViewModel = value;
                OnPropertyChanged("TimeEntryTypeListViewModel");
            }
        }

        public bool IsInheritedTypes
        {
            get { return _customer.InheritsTimeEntryTypes; }
            set
            {
                _customer.InheritsTimeEntryTypes = value;
                OnPropertyChanged("IsInheritedTypes");
                OnPropertyChanged("IsCustomTypes");

                TimeEntryTypeListViewModel.Reload();
                SaveTimeEntryTypes.RaiseCanExecuteChanged();
            }
        }

        public bool IsCustomTypes
        {
            get { return !_customer.InheritsTimeEntryTypes; }
        }

        public string WindowTitle
        {
            get { return string.Concat(EditTimeEntryTypeResources.CustomerListHeader, " ", _customer.CustomerName); }
        }

        public DelegateCommand<object> SaveTimeEntryTypes { get; set; }

        private void TimeEntryTypeListViewModel_DataReady(object sender, System.EventArgs e)
        {
            SaveTimeEntryTypes.RaiseCanExecuteChanged();
        }

        private void CreateTimeEntryTypeCompleted(object obj)
        {
            SaveTimeEntryTypes.RaiseCanExecuteChanged();
        }

        private bool CanSaveTimeEntryTypes(object arg)
        {
            //TODO: Refactor this
            //if (_customer.InheritsTimeEntryTypes != _originalCustomer.InheritsTimeEntryTypes)
            //{
            //    if (!_customer.InheritsTimeEntryTypes)
            //    {
            //        return (TimeEntryTypeListViewModel.TimeEntryTypes.Count > 0);
            //    }
            //    else
            //    {
            //        return true;
            //    }
            //}
            return false;
        }

        private void ExecuteSaveTimeEntryTypes(object obj)
        {
           
            _dataService.SaveCustomer(_customer);

            if (!_customer.InheritsTimeEntryTypes)
            {
                TimeEntryTypeListViewModel.Commit();
            }

            InternalCommands.EditCustomerTimeEntryTypesCompleted.Execute(null);
        }
    }
}