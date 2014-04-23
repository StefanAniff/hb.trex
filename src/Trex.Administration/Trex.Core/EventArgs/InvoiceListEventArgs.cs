using System.Collections.Generic;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class InvoiceListEventArgs : System.EventArgs
    {
        public InvoiceListEventArgs(List<Invoice> invoices)
        {
            if (invoices != null)
            {
                Invoices = invoices;
            }
            else
            {
                Invoices = new List<Invoice>();
            }
        }

        public List<Invoice> Invoices { get; set; }
    }
}