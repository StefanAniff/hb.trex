using System;
using Trex.ServiceContracts;

namespace Trex.Invoices.Interfaces
{
    public interface IInvoiceService
    {
        void RecalculateInvoice(InvoiceListItemView invoice);
        void RecalculateInvoice(InvoiceListItemView invoice, Action action);
    }
}
