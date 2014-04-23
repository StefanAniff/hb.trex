using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Trex.ServiceContracts;

namespace Trex.ServiceContracts
{
    public static class InvoiceExtensions
    {
        public static double TotalExclVat(this Invoice invoice)
        {
            return invoice.InvoiceLines.Sum(i => i.TotalExclVat());
        }

        public static double TotalInclVat(this Invoice invoice)
        {
            return invoice.InvoiceLines.Sum(i => i.TotalInclVat());
        }

        public static double TotalExclVat(this InvoiceLine invoiceLine)
        {
            return invoiceLine.PricePrUnit*invoiceLine.Units;
        }

        public static double TotalInclVat(this InvoiceLine invoiceLine)
        {
            return (invoiceLine.PricePrUnit*invoiceLine.Units)*(1 + invoiceLine.VatPercentage);
        }
    }

    public partial class Invoice
    {

        public void CancelChanges()
        {
            ChangeTracker.SetParentObject(this);
            ChangeTracker.CancelChanges();
        }
    }
}
