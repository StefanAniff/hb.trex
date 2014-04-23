using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.Invoices.Interfaces;
using Trex.ServiceContracts;

namespace Trex.Invoices.InvoiceManagementScreen.ButtonPanelView
{
    public class CustomerActionPanelViewFactory : IActionPanelViewFactory
    {

        private readonly Customer _customer;


        public CustomerActionPanelViewFactory(Customer customer)
        {
            _customer = customer;
        }


        public IView CreateActionPanelView()
        {
            var customerActionsView = new CustomerActionsView();
            var customerActionsViewModel = new CustomerActionsViewModel(_customer);
            customerActionsView.ApplyViewModel(customerActionsViewModel);

            return customerActionsView;
        }


    }
}
