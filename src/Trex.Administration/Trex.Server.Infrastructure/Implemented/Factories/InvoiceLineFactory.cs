using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented.Factories
{
    public class InvoiceLineFactory : IInvoiceLineFactory
    {
        #region IInvoiceLineFactory Members

        public InvoiceLine Create(Invoice invoice, double units, double pricePrUnit, string unit, string text, InvoiceLine.UnitTypes unitType, bool isExpense, double vat)
        {
            if (invoice == null)
            {
                throw new ParameterNullOrEmptyException("Invoice cannot be null");
            }

            var invoiceLine = new InvoiceLine
                                  {
                                      Units = units,
                                      PricePrUnit = pricePrUnit,
                                      Text = text,
                                      Unit = unit,
                                      Invoice = invoice,
                                      UnitType = unitType,
                                      IsExpense = isExpense,
                                      VatPercentage = vat
                                  };

            return invoiceLine;
        }

        #endregion
    }
}