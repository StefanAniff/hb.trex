using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.ServiceContracts;

namespace Trex.Infrastructure.Implemented
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDataService _dataService;
        private readonly IExceptionHandlerService _exceptionHandlerService;
        private readonly TaskManagementFilters _fetchFilters;

        public CustomerRepository(IDataService dataService, IExceptionHandlerService exceptionHandlerService, TaskManagementFilters fetchFilters)
        {

            Customers = new ObservableCollection<Customer>();
            _dataService = dataService;
            _exceptionHandlerService = exceptionHandlerService;
            _fetchFilters = fetchFilters;

            ApplicationCommands.RefreshData.RegisterCommand(new DelegateCommand<object>(ExecuteRefresh));
        }

        #region ICustomerRepository Members

        public event EventHandler DataLoaded;
        public ObservableCollection<Customer> Customers { get; private set; }

        public void ReloadCustomer(Customer customer, bool includeProjects, bool includeTasks, bool includeTimeEntries, Action<Customer> callbackAction)
        {
            _dataService.GetCustomerById(customer.CustomerID, _fetchFilters.ShowInactive, true, includeProjects, includeTasks, includeTimeEntries).Subscribe(callbackAction);
        }

        public void ReloadCustomer(GetCustomerByIdCriterias criterias, Action<Customer> callbackAction)
        {
            _dataService.GetCustomerByCriteria(criterias).Subscribe(callbackAction);
        }

        public bool IsDataLoaded { get; private set; }

        public void Initialize()
        {
            ExecuteRefresh(null);
        }

        #endregion

        private void ExecuteRefresh(object obj)
        {
            ApplicationCommands.SystemBusy.Execute("Loading data");
            _dataService.GetAllCustomers(false, _fetchFilters.ShowInactive, false, false, false).Subscribe(
                customers =>
                {
                    Customers = customers;
                   
                    ApplicationCommands.SystemIdle.Execute(null);
                    OnDataLoaded();
                },
                _exceptionHandlerService.OnError

                );
        }

        private void OnDataLoaded()
        {
            if (DataLoaded != null)
            {
                DataLoaded(this, null);
            }
            IsDataLoaded = true;
        }
    }
}