using System;
using System.Collections.ObjectModel;
using Trex.ServiceContracts;

namespace Trex.Core.Services
{
    public interface ICustomerRepository
    {
        ObservableCollection<Customer> Customers { get; }
        
        event EventHandler DataLoaded;
        bool IsDataLoaded { get; }
        void Initialize();
        void ReloadCustomer(Customer customer, bool includeProjects, bool includeTasks, bool includeTimeEntries, Action<Customer> callbackAction);
        void ReloadCustomer(GetCustomerByIdCriterias criterias, Action<Customer> callbackAction);
    }
}