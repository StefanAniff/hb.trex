using Trex.Core.Model;

namespace Trex.Core.EventArgs
{
    public class InvoiceSaveEventArgs : System.EventArgs
    {
        public InvoiceSaveEventArgs(Invoice invoice)
        {
            Invoice = invoice;
        }

        public Invoice Invoice { get; set; }
    }
}