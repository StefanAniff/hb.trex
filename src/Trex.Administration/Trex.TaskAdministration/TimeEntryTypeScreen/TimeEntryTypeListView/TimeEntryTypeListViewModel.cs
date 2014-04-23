using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.Resources;
using Trex.TaskAdministration.TimeEntryTypeScreen.TimeEntryTypeView;

namespace Trex.TaskAdministration.TimeEntryTypeScreen.TimeEntryTypeListView
{
    public class TimeEntryTypeListViewModel : ViewModelBase
    {
        private readonly Customer _customer;
        private readonly IDataService _dataService;
        private string _listHeader;
        private TimeEntryTypeViewModel _selectedItem;

        public TimeEntryTypeListViewModel(IDataService dataService)
            : this(dataService, null) {}

        public TimeEntryTypeListViewModel(IDataService dataService, Customer customer)
        {
            _customer = customer;

            _dataService = dataService;
            TimeEntryTypes = new ObservableCollection<TimeEntryTypeViewModel>();

            CreateTimeEntry = new DelegateCommand<object>(CreateTimeEntryTypeStart, CanCreateTimeEntryType);
            EditTimeEntry = new DelegateCommand<object>(EditTimeEntryTypeStart, CanEditTimeEntryType);
            InternalCommands.EditTimeEntryTypeCompleted.RegisterCommand(
                new DelegateCommand<TimeEntryType>(EditTimeEntryTypeCompleted));

            LoadData();
        }

        public ObservableCollection<TimeEntryTypeViewModel> TimeEntryTypes { get; set; }

        public string ListHeader
        {
            get
            {
                if (_customer == null)
                {
                    return EditTimeEntryTypeResources.GlobalListHeader;
                }

                return string.Empty;
            }
        }

        public DelegateCommand<object> CreateTimeEntry { get; set; }

        public DelegateCommand<object> EditTimeEntry { get; set; }

        public TimeEntryTypeViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                EditTimeEntry.RaiseCanExecuteChanged();
            }
        }

        public event EventHandler DataReady;
        public event EventHandler DataCommitted;

        public void Reload()
        {
            TimeEntryTypes.Clear();
            LoadData();
            EditTimeEntry.RaiseCanExecuteChanged();
            CreateTimeEntry.RaiseCanExecuteChanged();
        }

        private void LoadData()
        {
            if (_customer == null || (_customer != null && _customer.InheritsTimeEntryTypes))
            {
              
                _dataService.GetGlobalTimeEntryTypes().Subscribe(
                    timeEntryTypes =>
                        {

                            var timeEntryTypeViewModels = timeEntryTypes.Select(tt => new TimeEntryTypeViewModel(tt)).ToList();

                            foreach (var timeEntryTypeViewModel in timeEntryTypeViewModels)
                            {
                                TimeEntryTypes.Add(timeEntryTypeViewModel);
                            }
                            if (DataReady != null)
                            {
                                DataReady(this, null);
                            }
                        }
                    );
            }
            else
            {
                LoadCustomerTimeEntryTypes();
            }
        }

        private bool CanCreateTimeEntryType(object arg)
        {
            if (_customer != null && _customer.InheritsTimeEntryTypes)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void LoadCustomerTimeEntryTypes()
        {
            var timeEntryTypeViewModels = _customer.TimeEntryTypes.Select(t => new TimeEntryTypeViewModel(t)).ToList();

            foreach (var timeEntryTypeViewModel in timeEntryTypeViewModels)
            {
                TimeEntryTypes.Add(timeEntryTypeViewModel);
            }
            if (DataReady != null)
            {
                DataReady(this, null);
            }
        }

        //private void CreateTimeEntryTypeCompleted(TimeEntryType newTimeEntryType)
        //{
        //    var timeEntryTypeViewModel = new TimeEntryTypeViewModel(newTimeEntryType);

        //    //control the default type
        //    if (newTimeEntryType.IsDefault)
        //        SetAsDefault(newTimeEntryType);

        //    TimeEntryTypes.Add(timeEntryTypeViewModel);

        //    //If its a global type, commit immediately
        //    if (newTimeEntryType.IsGlobal)
        //        Commit();

        //}

        private void EditTimeEntryTypeCompleted(TimeEntryType timeEntryType)
        {
            //control the default type
            if (timeEntryType.IsDefault)
            {
                SetAsDefault(timeEntryType);
            }

            var timeEntryTypeViewModel = TimeEntryTypes.SingleOrDefault(t => t.TimeEntryType.TimeEntryTypeId == timeEntryType.TimeEntryTypeId);

            //If its a new one, we´ll add it
            if (timeEntryTypeViewModel == null)
            {
                timeEntryTypeViewModel = new TimeEntryTypeViewModel(timeEntryType);
                TimeEntryTypes.Add(timeEntryTypeViewModel);
                if (DataReady != null)
                {
                    DataReady(this, null);
                }
            }

            //If its a global type, commit immediately
            if (timeEntryType.Customer == null)
            {
                Commit();
            }
        }

        private bool CanEditTimeEntryType(object arg)
        {
            if (SelectedItem != null)
            {
                if (_customer != null && _customer.InheritsTimeEntryTypes)
                {
                    return false;
                }

                return true;
            }
            return false;
        }

    

        private void EditTimeEntryTypeStart(object obj)
        {
            InternalCommands.EditTimeEntryTypeStart.Execute(SelectedItem.TimeEntryType);
        }

        private void CreateTimeEntryTypeStart(object obj)
        {
            InternalCommands.CreateTimeEntryTypeStart.Execute(_customer);
        }

        private void SetAsDefault(TimeEntryType timeEntryType)
        {
            TimeEntryTypes.ToList().ForEach(t => t.IsDefault = false);
            timeEntryType.IsDefault = true;
        }

        public void     Commit()
        {
            foreach (var timeEntryTypeModel in TimeEntryTypes)
            {

                _dataService.SaveTimeEntryType(timeEntryTypeModel.TimeEntryType).Subscribe(
                    timeEntryType =>
                        {
                            var timeEntryTypeViewModel =
                                TimeEntryTypes.SingleOrDefault(
                                    t => t.TimeEntryType.TimeEntryTypeId == timeEntryType.TimeEntryTypeId);

                            if (timeEntryTypeViewModel != null)
                            {
                                timeEntryTypeViewModel.TimeEntryType.TimeEntryTypeId = timeEntryType.TimeEntryTypeId;
                            }

                            timeEntryTypeModel.Update();
                        });

            }

            if (DataCommitted != null)
            {
                DataCommitted(this, null);
            }
        }

      
    }
}