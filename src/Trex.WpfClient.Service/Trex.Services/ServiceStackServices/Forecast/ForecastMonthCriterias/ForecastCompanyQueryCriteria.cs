using NHibernate;
using NHibernate.Criterion;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;

namespace TrexSL.Web.ServiceStackServices.Forecast.ForecastMonthCriterias
{
    public class ForecastCompanyQueryCriteria : IForecastMonthQueryCriteria
    {
        private readonly int _companyId;

        public ForecastCompanyQueryCriteria(int companyId)
        {
            _companyId = companyId;
        }

        public IQueryOver<ForecastMonth, ForecastMonth> Apply(IQueryOver<ForecastMonth, ForecastMonth> queryOver, ForecastMonth monthAlias, Trex.Server.Core.Model.Forecast.Forecast forecastAlias)
        {
            Company companyAlias = null;
            Project projectAlias = null;
            ForecastProjectHours projectRegAlias = null;

            var projectCrit = QueryOver
                                .Of<Company>()
                                .Where(x => x.CustomerID == _companyId)
                                .And(x => x.CustomerID == companyAlias.CustomerID)
                                .Select(x => x.CustomerID)
                                .Take(1);

            return queryOver
                    .JoinAlias(() => forecastAlias.ProjectRegistrations, () => projectRegAlias)
                    .JoinAlias(() => projectRegAlias.Project, () => projectAlias)
                    .JoinAlias(() => projectAlias.Company, () => companyAlias)
                    .Where(Subqueries.WhereExists(projectCrit));            
        }
    }
}