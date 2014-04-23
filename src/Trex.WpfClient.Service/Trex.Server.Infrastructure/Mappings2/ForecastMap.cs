using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class ForecastMap : ClassMap<Forecast>
    { 
        public ForecastMap()
        {
            Id(x => x.Id).Column("ForecastId");
            Map(x => x.Date)
                .Access.CamelCaseField(Prefix.Underscore)
                .Not.Nullable();
            Map(x => x.DedicatedForecastTypeHours)
                .Nullable();
            References(x => x.ForecastType)
                .Not.Nullable()
                .Column("ForecastTypeInt")
                .Not.LazyLoad();
            References(x => x.ForecastMonth)
                .Access.CamelCaseField(Prefix.Underscore)
                .Not.Nullable()
                .Column("ForecastMonthId")
                .LazyLoad()
                .Cascade.None();
            HasMany(x => x.ProjectRegistrations)
                .AsBag()
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Inverse()
                .KeyColumns.Add("ForecastId");

            Table(ObjectNames.TableForecasts);
        }
    }
}