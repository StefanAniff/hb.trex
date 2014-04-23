using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class InvoiceMap : ClassMap<Invoice>
    {
        public InvoiceMap()
        {
            Id(x => x.ID);
            Map(x => x.CreateDate).Not.Nullable();
            Map(x => x.InvoiceDate).Not.Nullable();
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.EndDate).Not.Nullable();
            Map(x => x.DueDate).Nullable();
            References(x => x.Company).Column("CustomerID").Not.Nullable();
            References(x => x.CreatedBy).Not.Nullable();
            Map(x => x.Attention).Nullable().Not.Nullable();
            Map(x => x.CustomerName).Nullable().Not.Nullable();
            Map(x => x.StreetAddress).Nullable().Not.Nullable();
            Map(x => x.ZipCode).Nullable().Not.Nullable();
            Map(x => x.City).Nullable().Not.Nullable();
            Map(x => x.Country).Nullable().Not.Nullable();
            Map(x => x.VATPercentage).Not.Nullable();
            Map(x => x.FooterText).Nullable().Not.Nullable();
            HasMany(x => x.InvoiceLines).Cascade.All().OrderBy("SortIndex");
            Map(x => x.Closed).Not.Nullable();
            Map(x => x.Regarding).Nullable();
            Map(x => x.Address2).Nullable();
        }
    }
}