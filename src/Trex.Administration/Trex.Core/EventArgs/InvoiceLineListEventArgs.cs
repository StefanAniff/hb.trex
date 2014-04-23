using System.Collections.Generic;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class InvoiceLineListEventArgs : System.EventArgs
    {
        public InvoiceLineListEventArgs(List<InvoiceLine> invoiceLines)
        {
            if (invoiceLines != null)
            {
                InvoiceLines = invoiceLines;
            }
            else
            {
                InvoiceLines = new List<InvoiceLine>();
            }
        }

        public List<InvoiceLine> InvoiceLines { get; set; }
    }
}