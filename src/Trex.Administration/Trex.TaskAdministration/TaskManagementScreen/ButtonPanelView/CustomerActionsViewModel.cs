using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView
{
    public class CustomerActionsViewModel : ViewModelBase
    {
        private readonly Customer _customer;

        public CustomerActionsViewModel(Customer customer)
        {
            _customer = customer;
            

            EditCustomer = new DelegateCommand<object>(ExecuteEditCustomer, CanEditCustomer);
            AddProject = new DelegateCommand<object>(ExecuteAddProject, CanAddProject);
            AddCustomer = new DelegateCommand<object>(ExecuteAddCustomer, CanAddCustomer);
            DeleteCustomer = new DelegateCommand<object>(ExecuteDeleteCustomer, CanDeleteCustomer);
            EditTimeEntryTypes = new DelegateCommand<object>(ExecuteEditTimeEntryTypes, CanEditTimeEntryTypes);
            AddCIG = new DelegateCommand<object>(ExecuteAddCIG, canAddCIG);

        }

        public DelegateCommand<object> EditCustomer { get; set; }
        public DelegateCommand<object> AddProject { get; set; }
        public DelegateCommand<object> AddCustomer { get; set; }
        public DelegateCommand<object> DeleteCustomer { get; set; }
        public DelegateCommand<object> EditTimeEntryTypes { get; set; }
        public DelegateCommand<object> AddCIG { get; set; }

        private void ExecuteAddCIG(object obj)
        {
            InternalCommands.CustomerInvoiceGroupAddStart.Execute(_customer);
        }

        private bool canAddCIG(object obj)
        {
            return true;
        }

        private bool CanEditTimeEntryTypes(object arg)
        {
            return true;
        }

        private void ExecuteEditTimeEntryTypes(object obj)
        {
            InternalCommands.EditCustomerTimeEntryTypesStart.Execute(_customer);
        }

        private bool CanDeleteCustomer(object arg)
        {
            return _customer.Projects.Count == 0;
        }

        private void ExecuteDeleteCustomer(object obj)
        {
            InternalCommands.CustomerDeleteStart.Execute(_customer);
        }

        private bool CanAddCustomer(object arg)
        {
            return true;
        }

        private void ExecuteAddCustomer(object obj)
        {
            InternalCommands.CustomerAddStart.Execute(null);
        }

        private bool CanAddProject(object arg)
        {
            return !_customer.Inactive;
        }

        private bool CanEditCustomer(object arg)
        {
            return true;
        }

        private void ExecuteAddProject(object obj)
        {
            InternalCommands.ProjectAddStart.Execute(_customer);
        }

        private void ExecuteEditCustomer(object obj)
        {
            InternalCommands.CustomerEditStart.Execute(_customer);
        }

    }
}