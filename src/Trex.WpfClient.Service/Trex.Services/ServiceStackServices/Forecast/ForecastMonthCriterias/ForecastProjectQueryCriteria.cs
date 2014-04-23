using NHibernate;
using NHibernate.Criterion;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;

namespace TrexSL.Web.ServiceStackServices.Forecast.ForecastMonthCriterias
{
    public class ForecastProjectQueryCriteria : IForecastMonthQueryCriteria
    {
        private readonly int _projectId;

        public ForecastProjectQueryCriteria(int projectId)
        {
            _projectId = projectId;
        }

        public IQueryOver<ForecastMonth, ForecastMonth> Apply(IQueryOver<ForecastMonth, ForecastMonth> queryOver, ForecastMonth monthAlias, Trex.Server.Core.Model.Forecast.Forecast forecastAlias)
        {
            ForecastProjectHours projectRegAlias = null;

            var projectCrit = QueryOver
                                .Of<ForecastProjectHours>()
                                .Where(x => x.Project.ProjectID == _projectId)
                                .And(x => x.Parent.Id == forecastAlias.Id)
                                .Select(x => x.Id)
                                .Take(1);

            return queryOver
                    .JoinAlias(() => forecastAlias.ProjectRegistrations, () => projectRegAlias)
                    .Where(Subqueries.WhereExists(projectCrit));            
        }
    }
}