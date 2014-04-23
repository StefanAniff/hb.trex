#region

using System;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Invoices.Commands;
using Trex.ServiceContracts;

#endregion

namespace Trex.Invoices.InvoiceManagementScreen.ButtonPanelView
{
    public class InvoiceActionsViewModel : ViewModelBase
    {
        private readonly InvoiceListItemView _invoice;

        public InvoiceActionsViewModel(InvoiceListItemView invoice)
        {
            _invoice = invoice;

            EditInvoice = new DelegateCommand<object>(ExecuteEditInvoice, CanEditInvoice);
            ManageInvoiceLines = new DelegateCommand<object>(ExecuteManageInvoiceLines, CanManageInvoiceLines);

            ViewInvoice = new DelegateCommand<object>(ExecuteViewInvoice, CanViewInvoice);
            ViewSpecification = new DelegateCommand<object>(ExecuteViewSpecification, CanViewSpecification);

            ViewNonBillableSpecification = new DelegateCommand<object>(ExecuteViewNonBillableSpecification,
                                                                       CanViewNonBillableSpecification);
        }

        public DelegateCommand<object> CreateNewInvoice { get; set; }
        public DelegateCommand<object> EditInvoice { get; set; }
        public DelegateCommand<object> ManageInvoiceLines { get; set; }

        public DelegateCommand<object> ViewInvoice { get; set; }
        public DelegateCommand<object> ViewSpecification { get; set; }
        public DelegateCommand<object> ViewNonBillableSpecification { get; set; }

        public InvoiceListItemView Invoice
        {
            get { return _invoice; }
        }

        private void ExecuteEditInvoice(object obj)
        {
            InternalCommands.InvoiceEditStart.Execute(_invoice);
        }

        private void ExecuteManageInvoiceLines(object obj)
        {
            InternalCommands.ManageInvoiceLinesStart.Execute(_invoice);
        }

        private bool CanManageInvoiceLines(object arg)
        {
            return _invoice != null && !_invoice.Closed;
            //return false;
        }

        private bool CanViewNonBillableSpecification(object arg)
        {
            //return _invoice != null;
            return false;
        }

        private bool CanViewSpecification(object arg)
        {
            return _invoice != null;
        }

        private bool CanViewInvoice(object arg)
        {
            return _invoice != null;
        }

        private bool CanEditInvoice(object arg)
        {
            return _invoice != null && !_invoice.Closed;
        }

        private void ExecuteViewNonBillableSpecification(object obj)
        {
            throw new NotImplementedException();
        }

        private void ExecuteViewSpecification(object obj)
        {
            throw new NotImplementedException();
        }

        private void ExecuteViewInvoice(object obj)
        {
            try
            {
                InternalCommands.FinalizeInvoice.Execute(1);
            }
            catch (Exception)
            {
                throw new ArgumentException("Failed in InvoiceActionsViewModel.cs");
            }
        }
    }
}