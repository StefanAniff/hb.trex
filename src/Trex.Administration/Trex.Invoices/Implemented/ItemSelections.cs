using System;
using Trex.Core.Model;
using Trex.Invoices.Interfaces;
using Trex.ServiceContracts;

namespace Trex.Invoices.Implemented
{

    public class ItemSelections : IItemSelections
    {

        public Customer SelectedCustomer { get; set; }
        public Invoice SelectedInvoice { get; set; }
        public TimeEntry SelectedTimeEntry { get; set; }
        public InvoiceLine SelectedInvoiceLine { get; set; }
        public DateTime FilterdDate { get; set; }

    }

}
