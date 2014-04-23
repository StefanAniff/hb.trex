using System;
using Trex.Core.Services;
using Trex.Infrastructure.Implemented;
using Trex.Invoices.Commands;
using Trex.Invoices.Interfaces;
using Trex.ServiceContracts;

namespace Trex.Invoices.Implemented
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IDataService _dataService;

        public InvoiceService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void RecalculateInvoice(InvoiceListItemView invoice)
        {
            RecalculateInvoice(invoice, () => { });
        }

        public void RecalculateInvoice(InvoiceListItemView invoice, Action action)
        {
            _dataService.RecalculateInvoice(invoice.ID, invoice.StartDate, invoice.EndDate, invoice.CustomerInvoiceGroupId)
                                                .Subscribe(r => Execute.InUIThread(() =>
                                                {
                                                    InternalCommands.ReloadInvoices.Execute(null);
                                                    InternalCommands.RefreshCustomers.Execute(null);
                                                    
                                                    action.Invoke();
                                                }));
        }
    }
}
