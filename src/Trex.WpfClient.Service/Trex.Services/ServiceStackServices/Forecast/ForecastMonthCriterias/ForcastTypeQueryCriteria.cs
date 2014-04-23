using NHibernate;
using NHibernate.Criterion;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;

namespace TrexSL.Web.ServiceStackServices.Forecast.ForecastMonthCriterias
{
    public class ForcastTypeQueryCriteria : IForecastMonthQueryCriteria
    {
        private readonly int _forecastTypeId;

        public ForcastTypeQueryCriteria(int forecastTypeId)
        {
            _forecastTypeId = forecastTypeId;
        }

        public IQueryOver<ForecastMonth, ForecastMonth> Apply(IQueryOver<ForecastMonth, ForecastMonth> queryOver, ForecastMonth monthAlias, Trex.Server.Core.Model.Forecast.Forecast forecastAlias)
        {
            var forecastTypeCrit = QueryOver
                                    .Of<Trex.Server.Core.Model.Forecast.Forecast>()
                                    .Where(x => x.ForecastType.Id == _forecastTypeId)
                                    .And(x => x.ForecastMonth.Id == monthAlias.Id)
                                    .Select(x => x.Id)
                                    .Take(1);

            return queryOver.Where(Subqueries.WhereExists(forecastTypeCrit));
        }
    }
}