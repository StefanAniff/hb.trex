using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class InvoiceLineMap : ClassMap<InvoiceLine>
    {
        public InvoiceLineMap()
        {
            Id(x => x.ID);
            References(x => x.Invoice);
            Map(x => x.Text);
            Map(x => x.PricePrUnit);
            Map(x => x.Units);
            Map(x => x.Unit);
            Map(x => x.UnitType).CustomType<int>();
            Map(x => x.SortIndex);
            Map(x => x.VatPercentage);
            Map(x => x.IsExpense);
        }
    }
}