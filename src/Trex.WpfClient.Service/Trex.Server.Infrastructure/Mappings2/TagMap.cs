using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class TagMap : ClassMap<Tag>
    {
        public TagMap()
        {
            Id(x => x.Id);
            References(x => x.Company).Not.Nullable().Column("CustomerID");
            Map(x => x.Text).Not.Nullable();
        }
    }
}