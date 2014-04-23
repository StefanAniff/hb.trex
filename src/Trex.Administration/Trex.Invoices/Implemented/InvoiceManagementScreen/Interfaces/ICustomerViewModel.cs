using System;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Invoices.InvoiceManagementScreen.Interfaces
{
    public interface ICustomerViewModel
    {
        Customer Customer { get; set; }
        int DistinctPrices { get; set; }
        double InventoryValue { get; set; }
        DateTime FirstTimeEntryDate { get; set; }

    }
}
