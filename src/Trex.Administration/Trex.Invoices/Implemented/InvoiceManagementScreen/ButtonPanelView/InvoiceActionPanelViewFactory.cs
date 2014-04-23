using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.Invoices.Interfaces;
using Trex.ServiceContracts;

namespace Trex.Invoices.InvoiceManagementScreen.ButtonPanelView
{
    public class InvoiceActionPanelViewFactory : IActionPanelViewFactory
    {
        private readonly InvoiceListItemView _invoice;

        public InvoiceActionPanelViewFactory(InvoiceListItemView task)
        {
            _invoice = task;
        }

        public IView CreateActionPanelView()
        {
            var taskActionsViewModel = new InvoiceActionsViewModel(_invoice);
            var taskActionsView = new InvoiceActionsView();
            taskActionsView.ViewModel = taskActionsViewModel;

            return taskActionsView;
        }
    }
}
