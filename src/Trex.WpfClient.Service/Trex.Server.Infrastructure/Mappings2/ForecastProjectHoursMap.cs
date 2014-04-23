using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class ForecastProjectHoursMap : ClassMap<ForecastProjectHours>
    {
        public ForecastProjectHoursMap()
        {
            Id(x => x.Id).Column("ForecastProjectHoursId");
            Map(x => x.Hours).Not.Nullable();
            References(x => x.Project).Column("ProjectId").Not.Nullable();
            References(x => x.Parent).Not.Nullable()
                .Column("ForecastId")
                .LazyLoad()
                .Cascade.None();

            Table(ObjectNames.TableForecastProjectHours);
        }
    }
}