using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class TimeEntryTypeMap : ClassMap<TimeEntryType>
    {
        public TimeEntryTypeMap()
        {
            Id(x => x.Id);
            References(x => x.Company).Column("CustomerID").Nullable();
            Map(x => x.IsDefault);
            Map(x => x.IsBillableByDefault);
            Map(x => x.Name);

            Table(ObjectNames.TableTimeEntryType);
        }
    }
}
