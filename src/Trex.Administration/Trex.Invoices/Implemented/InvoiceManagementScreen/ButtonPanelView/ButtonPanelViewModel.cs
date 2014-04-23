#region

using System;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.Core.Services;
using Trex.Infrastructure.Implemented;
using Trex.Invoices.Commands;
using Trex.Invoices.Interfaces;
using Trex.ServiceContracts;

#endregion

namespace Trex.Invoices.InvoiceManagementScreen.ButtonPanelView
{
    public class ButtonPanelViewModel : ViewModelBase, IButtonPanelViewModel
    {
        private readonly IDataService _dataService;
        private readonly IInvoiceService _invoiceService;
        private CustomersInvoiceView _customerInvoiceView;
        private InvoiceListItemView _invoiceListItemView;

        public ButtonPanelViewModel(IDataService dataservice, IInvoiceService invoiceService)
        {
            _dataService = dataservice;
            _invoiceService = invoiceService;
            ActionPanels = new ObservableCollection<IView>();

            CreateNewInvoiceCommand = new DelegateCommand<object>(ExecuteCreateNewInvoice, CanExecuteCreateNewInvoice);
            PreviewInvoiceCommand = new DelegateCommand<object>(ExecutePreviewInvoice, CanExecuteDownloadInvoice);
            PreviewSpecificationCommand = new DelegateCommand<object>(ExecutePreviewSpecification, CanExecuteDownloadSpecification);
            CreditNoteCommand = new DelegateCommand<object>(ExecuteCreditNoteCommand, CanExecuteCreditNoteCommand);
            Recalculateinvoice = new DelegateCommand<object>(ExecuteRecalculateinvoice, CanExecuteRecalculateinvoice);
            EmailInvoice = new DelegateCommand<object>(ExecuteEmailInvoice, CanExecuteEmailInvoice);
            RegenerateFiles = new DelegateCommand<object>(ExecuteRegenerate, CanExecuteRegenerate);



            InternalCommands.CustomerSelected.RegisterCommand(new DelegateCommand<CustomersInvoiceView>(CustomerSelected));
            InternalCommands.InvoiceSelected.RegisterCommand(new DelegateCommand<InvoiceListItemView>(InvoiceSelected));
            InternalCommands.RaiseSendEmailCanExecute.RegisterCommand(new DelegateCommand(RaiseSendEmailCanExecute));
        }

        private void RaiseSendEmailCanExecute()
        {
            EmailInvoice.RaiseCanExecuteChanged();
        }


        public ObservableCollection<IView> ActionPanels { get; set; }
        public DelegateCommand<object> CreateNewInvoiceCommand { get; set; }
        public DelegateCommand<object> PreviewInvoiceCommand { get; set; }
        public DelegateCommand<object> PreviewSpecificationCommand { get; set; }
        public DelegateCommand<object> PrintInvoiceCommand { get; set; }
        public DelegateCommand<object> CreditNoteCommand { get; set; }
        public DelegateCommand<object> Recalculateinvoice { get; set; }
        public DelegateCommand<object> EmailInvoice { get; set; }
        public DelegateCommand<object> RegenerateFiles { get; set; }

        private void InvoiceSelected(InvoiceListItemView obj)
        {
            ActionPanels.Clear();
            var invoiceActions = new InvoiceActionPanelViewFactory(obj);
            ActionPanels.Add(invoiceActions.CreateActionPanelView());
            _invoiceListItemView = obj;
            PreviewInvoiceCommand.RaiseCanExecuteChanged();
            PreviewSpecificationCommand.RaiseCanExecuteChanged();
            CreditNoteCommand.RaiseCanExecuteChanged();
            Recalculateinvoice.RaiseCanExecuteChanged();
            EmailInvoice.RaiseCanExecuteChanged();
            RegenerateFiles.RaiseCanExecuteChanged();
        }

        private void CustomerSelected(CustomersInvoiceView obj)
        {
            _customerInvoiceView = obj;
            CreateNewInvoiceCommand.RaiseCanExecuteChanged();
            _invoiceListItemView = null;
            PreviewInvoiceCommand.RaiseCanExecuteChanged();
            PreviewSpecificationCommand.RaiseCanExecuteChanged();
            CreditNoteCommand.RaiseCanExecuteChanged();
            Recalculateinvoice.RaiseCanExecuteChanged();
            EmailInvoice.RaiseCanExecuteChanged();
            RegenerateFiles.RaiseCanExecuteChanged();
        }

        private bool CanExecuteCreateNewInvoice(object arg)
        {
            if (_customerInvoiceView != null)
                return true;

            return false;
        }

        private bool CanExecuteDownloadSpecification(object arg)
        {
            if (_invoiceListItemView != null)
                return true;

            return false;
        }

        private bool CanExecuteDownloadInvoice(object arg)
        {
            if (_invoiceListItemView != null)
                return true;

            return false;
        }

        private bool CanExecuteCreditNoteCommand(object arg)
        {
            if (_invoiceListItemView != null && _invoiceListItemView.InvoiceLinkId == null && _invoiceListItemView.InvoiceID != null && _invoiceListItemView.ExclVAT != 0)
                return true;

            return false;
        }

        private bool CanExecuteRecalculateinvoice(object arg)
        {
    #if DEBUG
            return true;
    #endif   
            if (_invoiceListItemView != null && !_invoiceListItemView.IsCreditNote && _invoiceListItemView.InvoiceID == null)
                return true;

            return false;
        }

        private bool CanExecuteEmailInvoice(object arg)
        {
            if (_invoiceListItemView != null && !_invoiceListItemView.Delivered && _invoiceListItemView.InvoiceID != null)
                return true;
            return false;
        }

        private bool CanExecuteRegenerate(object arg)
        {
            if (_invoiceListItemView != null)
                return true;

            return false;
        }

        private void ExecuteCreditNoteCommand(object obj)
        {
            if (MessageBox.Show("Are you sure you want to make a credit note of the selected invoice?", "Wait", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                InternalCommands.GenerateCreditnote.Execute(null);
        }

        private void ExecuteCreateNewInvoice(object obj)
        {
            InternalCommands.InvoiceAddStart.Execute(_customerInvoiceView);
        }

        private void ExecutePreviewInvoice(object obj)
        {
            InternalCommands.FinalizeInvoice.Execute(1);
        }

        private void ExecutePreviewSpecification(object obj)
        {
            InternalCommands.FinalizeInvoice.Execute(2);
        }

        private void ExecuteRecalculateinvoice(object obj)
        {
            _invoiceService.RecalculateInvoice(_invoiceListItemView);
        }

        private void ExecuteEmailInvoice(object obj)
        {
            InternalCommands.SendInvoiceEmail.Execute(null);
        }

        private void ExecuteRegenerate(object obj)
        {
            InternalCommands.RegenerateInvoiceFiles.Execute(null);
        }
    }
}