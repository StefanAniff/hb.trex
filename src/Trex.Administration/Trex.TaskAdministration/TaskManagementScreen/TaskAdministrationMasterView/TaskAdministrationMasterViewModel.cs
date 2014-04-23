using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.SearchView;

namespace Trex.TaskAdministration.TaskManagementScreen.TaskAdministrationMasterView
{
    public class TaskAdministrationMasterViewModel : ViewModelBase, ITaskAdministrationMasterViewModel
    {
        private readonly IDataService _dataService;
        private readonly TaskManagementFilters _fetchFilters;

        public TaskAdministrationMasterViewModel(IDataService dataService, TaskManagementFilters fetchFilters)
        {
            _dataService = dataService;
            _fetchFilters = fetchFilters;

            RefreshCommand = new DelegateCommand<object>(ExecuteRefresh);
            CreateCustomerCommand = new DelegateCommand<object>(ExecuteCreateCustomer);
            SearchViewModel = new SearchViewModel(_dataService);
        }

        public SearchViewModel SearchViewModel { get; set; }

        
        public bool ShowInactive
        {
            get { return _fetchFilters.ShowInactive; }
            set
            {
                _fetchFilters.ShowInactive = value;
                ExecuteRefresh(null);
                OnPropertyChanged("ShowInactive");
            }
        }

        public DelegateCommand<object> RefreshCommand { get; set; }
        public DelegateCommand<object> CreateCustomerCommand { get; set; }

        private void ExecuteCreateCustomer(object obj)
        {
            InternalCommands.CustomerAddStart.Execute(null);
        }

        private void ExecuteRefresh(object obj)
        {
            ApplicationCommands.RefreshData.Execute(null);
        }

        public DateTime? TimeEntryCreationDateFrom
        {
            get { return _fetchFilters.TimeEntryFilterFrom; }
            set
            {
                if (!value.HasValue)
                    throw new ValidationException("Date can't be empty");
                _fetchFilters.TimeEntryFilterFrom = value.Value;
                OnPropertyChanged("TimeEntryCreationDateFrom");
            }
        }

        public DateTime? TimeEntryCreationDateTo
        {
            get { return _fetchFilters.TimeEntryFilterTo; }
            set
            {
                if (!value.HasValue)
                    throw new ValidationException("Date can't be empty");
                _fetchFilters.TimeEntryFilterTo = value.Value;
                OnPropertyChanged("TimeEntryCreationDateTo");
            }
        }

        public DateTime? TaskCreationDateFrom
        {
            get { return _fetchFilters.TaskFilterFrom; }
            set
            {
                if (!value.HasValue)
                    throw new ValidationException("Date can't be empty");
                _fetchFilters.TaskFilterFrom = value.Value;
                OnPropertyChanged("TaskCreationDateFrom");
            }
        }

        public DateTime? TaskCreationDateTo
        {
            get { return _fetchFilters.TaskFilterTo; }
            set
            {
                if (!value.HasValue)
                    throw new ValidationException("Date can't be empty");
                _fetchFilters.TaskFilterTo = value.Value;
                OnPropertyChanged("TaskCreationDateFrom");
            }
        }
    }
}