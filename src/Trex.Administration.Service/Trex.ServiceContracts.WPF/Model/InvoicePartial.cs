#region Namespaces

using System.Linq;

#endregion

namespace Trex.ServiceContracts.Model
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

        public static double TotalVat(this Invoice invoice)
        {
            return invoice.InvoiceLines.Sum(i => i.VatPercentage*i.Units*i.PricePrUnit);
        }
    }
}