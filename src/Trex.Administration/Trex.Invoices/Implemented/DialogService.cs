#region

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Controls.GridView;
using Trex.Core.Interfaces;
using Trex.Core.Services;
using Trex.Invoices.Commands;
using Trex.Invoices.Dialogs.EditInvoiceLineView;
using Trex.Invoices.Dialogs.EditInvoiceLinesView;
using Trex.Invoices.Dialogs.EditInvoiceTemplateView;
using Trex.Invoices.Dialogs.EditInvoiceView;
using Trex.Invoices.Dialogs.InvoiceComments;
using Trex.Invoices.Interfaces;
using Trex.Invoices.InvoiceManagementScreen.InvoiceView;
using Trex.ServiceContracts;
using DelegateCommand = Telerik.Windows.Controls.DelegateCommand;

#endregion

namespace Trex.Invoices.Implemented
{
    public class DialogService : IDialogService
    {
        private readonly IDataService _dataService;
        private readonly IUserRepository _userRepository;
        private readonly IUserSession _userSession;

        public DialogService(IDataService dataService, IUserSession userSession, IUserRepository userRepository)
        {
            _dataService = dataService;
            _userSession = userSession;
            _userRepository = userRepository;

            InternalCommands.InvoiceEditStart.RegisterCommand(new DelegateCommand<Invoice>(InvoiceEditStart));

            InternalCommands.InvoiceAddStart.RegisterCommand(new DelegateCommand<CustomersInvoiceView>(InvoiceAddStart));

            InternalCommands.ManageInvoiceLinesStart.RegisterCommand(
                new DelegateCommand<Invoice>(ManageInvoiceLinesStart));

            InternalCommands.ManageInvoiceLinesCompleted.RegisterCommand(new DelegateCommand(ManageInvoiceLinesCompleted));

            InternalCommands.InvoiceLineEditStart.RegisterCommand(new DelegateCommand(InvoiceLineEditStart));

            InternalCommands.InvoiceLineAddStart.RegisterCommand(new DelegateCommand(InvoiceLineAddStart));

            InternalCommands.CreateNewInvoiceTemplateStart.RegisterCommand(
                new DelegateCommand(ExecuteCreateNewInvoiceTemplateStart));

            InternalCommands.SeeCommentsStart.RegisterCommand(new DelegateCommand<InvoiceListItemViewModel>(ExecuteSeeCommentsStart));

        }

        private void ExecuteSeeCommentsStart(InvoiceListItemViewModel invoice)
        {
            var commentView = new InvoiceComments();
            var invoiceCommentViewModel = new InvoiceCommentViewModel(invoice, _dataService, _userRepository, _userSession);
            commentView.ViewModel = invoiceCommentViewModel;
            commentView.Show();
        }

        private void ExecuteCreateNewInvoiceTemplateStart(object obj)
        {
            var invoiceTemplate = new InvoiceTemplate
                                      {
                                          CreateDate = DateTime.Now,
                                          CreatedBy = _userSession.CurrentUser.Name
                                      };

            var editInvoiceTemplateView = new EditInvoiceTemplateView();
            editInvoiceTemplateView.Show();
        }

        private void InvoiceLineAddStart(object obj)
        {
            //var invoiceLine = new InvoiceLine()
            //                      {
            //                          InvoiceID = _itemSelections.SelectedInvoice.ID
            //                      };

            //var editInvoiceLineView = new EditInvoiceLineView();
            //var editInvoiceLineViewModel = new EditInvoiceLineViewModel(invoiceLine);
            //editInvoiceLineView.ApplyViewModel(editInvoiceLineViewModel);
            //editInvoiceLineView.Show();
        }

        private void InvoiceLineEditStart(object obj)
        {
            var editInvoiceLineView = new EditInvoiceLineView();
            var editInvoiceLineViewModel = new EditInvoiceLineViewModel((InvoiceLine)obj);
            editInvoiceLineView.ViewModel = editInvoiceLineViewModel;
            editInvoiceLineView.Show();
        }

        private void ManageInvoiceLinesCompleted(object obj)
        {
            //InternalCommands.CustomerSelected.Execute(typeof(Customer));
            ////TODO invoked by?
            //InternalCommands.UpdateScreens.Execute(typeof(Customer));
        }

        private void ManageInvoiceLinesStart(Invoice invoice)
        {
            var editInvoiceLinesView = new EditInvoiceLinesView();
            var editInvoiceLinesViewModel = new EditInvoiceLinesViewModel(invoice, _dataService, _userRepository);
            editInvoiceLinesView.ViewModel = editInvoiceLinesViewModel;
            editInvoiceLinesView.Show();
        }


        private Customer cus;
        private void InvoiceAddStart(CustomersInvoiceView customerInvoiceView)
        {
            cus = new Customer();

            if (customerInvoiceView != null)
                _dataService.GetCustomerById(customerInvoiceView.CustomerID, false, false, false, false, false)
                    .Subscribe(c =>
                                   {
                                       cus = c;
                                       var invoice = new Invoice
                                                         {
                                                             CreatedBy = _userSession.CurrentUser.UserID,
                                                             Closed = false,
                                                             CreateDate = DateTime.Now,
                                                             InvoiceDate = DateTime.Now.Date,
                                                             StartDate = DateTime.Now.Date,
                                                             EndDate = DateTime.Now.Date,
                                                             DueDate = DateTime.Now.Date,
                                                             VAT = 0.25,
                                                             Guid = Guid.NewGuid()
                                                         };
                                       InvoiceEditStart(invoice);
                                   });

            //Customer customer =
            //    _customerRepository.Customers.SingleOrDefault(c => c.CustomerID == customerInvoiceView.CustomerID);

            //var invoice = new Invoice
            //                  {
            //                      //CustomerId = customer.Id,
            //                      CreatedBy = _userSession.CurrentUser.UserID,
            //                      Closed = false,
            //                      CreateDate = DateTime.Now,
            //                      InvoiceDate = DateTime.Now.Date,
            //                      StartDate = DateTime.Now.Date,
            //                      EndDate = DateTime.Now.Date,
            //                      DueDate = DateTime.Now.Date
            //                  };

            //InvoiceEditStart(invoice);
        }

        private void InvoiceEditStart(Invoice invoice)
        {
            var editInvoiceView = new EditInvoiceView();
            var editInvoiceViewModel = new EditInvoiceViewModel(invoice, cus, _dataService);
            editInvoiceView.ViewModel = editInvoiceViewModel;
            editInvoiceView.Show();
        }


    }
}