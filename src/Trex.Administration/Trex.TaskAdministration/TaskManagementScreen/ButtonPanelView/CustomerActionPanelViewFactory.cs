using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView
{
    public class CustomerActionPanelViewFactory : IActionPanelViewFactory
    {
        private readonly Customer _customer;

        public CustomerActionPanelViewFactory(Customer customer)
        {
            _customer = customer;
        }

        #region IActionPanelViewFactory Members

        public IView CreateActionPanelView()
        {
            var customerActionsView = new CustomerActionsView();
            var customerActionsViewModel = new CustomerActionsViewModel(_customer);
            customerActionsView.ViewModel = customerActionsViewModel;

            return customerActionsView;
        }

        #endregion
    }
}