using System;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Invoices.Commands;
using Trex.ServiceContracts;

namespace Trex.Invoices.InvoiceManagementScreen.ButtonPanelView
{
    public class CustomerActionsViewModel : ViewModelBase
    {


        public DelegateCommand<object> AutoInvoice { get; set; }
        public DelegateCommand<object> CreateNewInvoice { get; set; }
        public DelegateCommand<object> EditBillingDetails { get; set; }

        private readonly CustomersInvoiceView _customer;


        public CustomerActionsViewModel(CustomersInvoiceView customer)
        {
            _customer = customer;

            AutoInvoice = new DelegateCommand<object>(ExecuteAutoInvoice, CanAutoInvoice);
            CreateNewInvoice = new DelegateCommand<object>(ExecuteCreateNewInvoice, CanCreateNewInvoice);
            EditBillingDetails = new DelegateCommand<object>(ExecuteEditBillingDetails, CanEditBillingDetails);
        }


        private void ExecuteCreateNewInvoice(object obj)
        {
            InternalCommands.InvoiceAddStart.Execute(_customer);
        }


        private void ExecuteAutoInvoice(object obj)
        {
            InternalCommands.AutoInvoice.Execute(_customer);
        }


        private bool CanEditBillingDetails(object arg)
        {
            return _customer != null;
        }


        private bool CanCreateNewInvoice(object arg)
        {
            return _customer != null;
        }


        private bool CanAutoInvoice(object arg)
        {
            return _customer != null;
        }


        private void ExecuteEditBillingDetails(object obj)
        {
            throw new NotImplementedException();
        }


    }
}
