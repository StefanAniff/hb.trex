using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class ForecastMonthMap : ClassMap<ForecastMonth>
    {
        public ForecastMonthMap()
        {
            Id(x => x.Id).Column("ForecastMonthId");
            Map(x => x.Month)
                .Access.CamelCaseField(Prefix.Underscore)
                .Not.Nullable();
            Map(x => x.Year)
                .Access.CamelCaseField(Prefix.Underscore)
                .Not.Nullable();
            Map(x => x.UnLocked)
                .Access.CamelCaseField(Prefix.Underscore);
            Map(x => x.LockedFrom)
                .Access.CamelCaseField(Prefix.Underscore);
            References(x => x.User)
                .Not.Nullable()
                .Not.LazyLoad()
                .Access.CamelCaseField(Prefix.Underscore);
            HasMany(x => x.Forecasts)
                .AsBag()
                .LazyLoad()
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Inverse()
                .KeyColumns.Add("ForecastMonthId");

            References(x => x.CreatedBy)
                .Not.Nullable()
                .Not.LazyLoad()
                .Column("CreatedById")
                .Access.CamelCaseField(Prefix.Underscore);
            Map(x => x.CreatedDate)
                .Access.CamelCaseField(Prefix.Underscore)
                .Not.Nullable();

            Table(ObjectNames.TableForecastMonths);
        }
    }
}