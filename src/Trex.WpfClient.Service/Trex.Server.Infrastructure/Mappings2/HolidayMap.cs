using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class HolidayMap : ClassMap<Holiday>
    {
        public HolidayMap()
        {
            Id(x => x.Id).Column("HolidayId");
            Map(x => x.Date)
                .Access.CamelCaseField(Prefix.Underscore)
                .Not
                .Nullable();

            Map(x => x.Description);            
            Table(ObjectNames.TableHolidays);
        }
    }
}