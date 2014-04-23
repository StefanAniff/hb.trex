using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IInvoiceLineFactory
    {
        InvoiceLine Create(Invoice invoice, double units, double pricePrUnit, string unit, string text, InvoiceLine.UnitTypes unitType, bool isExpense, double vat);
    }
}