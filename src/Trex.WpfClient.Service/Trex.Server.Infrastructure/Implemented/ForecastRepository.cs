using NHibernate;
using NHibernate.Criterion;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;

namespace Trex.Server.Infrastructure.Implemented
{
    public class ForecastRepository : GenericRepository<Forecast>, IForecastRepository
    {
        public ForecastRepository(ISession openSession) 
            : base(openSession)
        {
        }

        public ForecastRepository(IActiveSessionManager activeSessionManager) 
            : base(activeSessionManager)
        {
        }

        /// <summary>
        /// Gets forecasts supporting project hours
        /// for the current year
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="queryInternalCustomers"></param>
        /// <param name="dateSpan"></param>
        /// <returns></returns>
        public decimal GetHourSumByCriteria(int userId, bool queryInternalCustomers, DateSpan dateSpan)
        {
            Forecast forecastAlias = null;
            ForecastType forecastTypeAlias = null;
            User userAlias = null;
            ForecastProjectHours regAlias = null;
            Company companyAlias = null;
            Project projectAlias = null;
            ForecastMonth monthAlias = null;

            var query = Session.QueryOver(() => regAlias)
                               .JoinAlias(() => regAlias.Parent, () => forecastAlias)
                               .JoinAlias(() => forecastAlias.ForecastMonth, () => monthAlias)
                               .JoinAlias(() => monthAlias.User, () => userAlias)
                               .JoinAlias(() => forecastAlias.ForecastType, () => forecastTypeAlias)
                               .JoinAlias(() => regAlias.Project, () => projectAlias)
                               .JoinAlias(() => projectAlias.Company, () => companyAlias)
                               .Where(() => userAlias.UserID == userId)
                               .And(() => forecastAlias.Date >= dateSpan.From)
                               .And(() => forecastAlias.Date <= dateSpan.To)
                               .And(() => forecastTypeAlias.SupportsProjectHours)
                               .And(() => companyAlias.Internal == queryInternalCustomers)
                               .Select(Projections.Sum<ForecastProjectHours>(x => x.Hours));

            var result = query
                            .UnderlyingCriteria
                            .UniqueResult<decimal>();                

            // Dedicatedhours are accounted for internally
            if (queryInternalCustomers)
            {
                var typeDedicatedHours = Session.QueryOver(() => forecastAlias)
                                                .JoinAlias(() => forecastAlias.ForecastMonth, () => monthAlias)
                                                .JoinAlias(() => monthAlias.User, () => userAlias)
                                                .JoinAlias(() => forecastAlias.ForecastType, () => forecastTypeAlias)
                                                .Where(() => forecastAlias.Date >= dateSpan.From)
                                                .And(() => forecastAlias.Date <= dateSpan.To)
                                                .And(() => forecastTypeAlias.StatisticsInclusion)
                                                .And(() => userAlias.UserID == userId)
                                                .Select(Projections.Sum<Forecast>(x => x.DedicatedForecastTypeHours))
                                                .UnderlyingCriteria
                                                .UniqueResult<decimal>();
                result = result + typeDedicatedHours;
            }

            return result;
        }  
     
        /// <summary>
        /// Returns the forecast count constrained by user, forecastype 
        /// and datespan
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <param name="forecastTypeId">ForecastType's id</param>
        /// <param name="dateSpan">Datespan to search in</param>
        /// <returns></returns>
        public int GetForecastCountByForecastType(int userId, int forecastTypeId, DateSpan dateSpan)
        {
            ForecastMonth monthAlias = null;

            var result = Session
                        .QueryOver<Forecast>()
                        .JoinAlias(x => x.ForecastMonth, () => monthAlias)
                        .Where(() => monthAlias.User.UserID == userId)
                        .And(x => x.ForecastType.Id == forecastTypeId)
                        .And(x => x.Date >= dateSpan.From)
                        .And(x => x.Date <= dateSpan.To)
                        .Select(Projections.RowCount())
                        .UnderlyingCriteria
                        .UniqueResult<int>();

            return result;
        }
    }
}