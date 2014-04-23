using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class ForecastTypeMap : ClassMap<ForecastType>
    {
        public ForecastTypeMap()
        {
            Id(x => x.Id)
                .Column("Id");
            Map(x => x.Name)
                .Not
                .Nullable();
            Map(x => x.Description)
                .Nullable();
            Map(x => x.SupportsDedicatedHours)
                .Not
                .Nullable();
            Map(x => x.SupportsProjectHours)
                .Not
                .Nullable();
            Map(x => x.ColorStringHex)
                .Not
                .Nullable();
            Map(x => x.StatisticsInclusion)
                .Not
                .Nullable();

            Table(ObjectNames.TableForecastTypes);
        }
    }
}