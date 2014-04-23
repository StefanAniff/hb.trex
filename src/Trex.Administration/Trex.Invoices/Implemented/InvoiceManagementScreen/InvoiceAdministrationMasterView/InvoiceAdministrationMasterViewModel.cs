#region

using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Invoices.Commands;

#endregion

namespace Trex.Invoices.InvoiceManagementScreen.InvoiceAdministrationMasterView
{
    public class InvoiceAdministrationMasterViewModel : ViewModelBase, IInvoiceAdministrationMasterViewModel
    {
        public InvoiceAdministrationMasterViewModel()
        {
            RefreshCommand = new DelegateCommand<object>(ExecuteRefresh);
            CreateInvoiceCommand = new DelegateCommand<object>(ExecuteCreateInvoice);
            EditInvoiceCommand = new DelegateCommand<object>(ExecuteEditInvoice);
            ManageInvoiceLinesCommand = new DelegateCommand<object>(ExecuteManageInvoiceLinesCommand);
        }

        public DelegateCommand<object> RefreshCommand { get; set; }
        public DelegateCommand<object> CreateInvoiceCommand { get; set; }
        public DelegateCommand<object> EditInvoiceCommand { get; set; }
        public DelegateCommand<object> ManageInvoiceLinesCommand { get; set; }

        private void ExecuteManageInvoiceLinesCommand(object obj)
        {
            InternalCommands.InvoiceLinesEditStart.Execute(null);
        }

        private void ExecuteEditInvoice(object obj)
        {
            InternalCommands.InvoiceEditStart.Execute(null);
        }

        private void ExecuteCreateInvoice(object obj)
        {
            InternalCommands.InvoiceEditStart.Execute(null);
        }

        private void ExecuteRefresh(object obj)
        {
            InternalCommands.RefreshCustomers.Execute(null);
        }
    }
}