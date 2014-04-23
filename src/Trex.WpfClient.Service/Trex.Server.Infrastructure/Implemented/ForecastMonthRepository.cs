using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;
using System.Linq;

namespace Trex.Server.Infrastructure.Implemented
{
    public class ForecastMonthRepository : GenericRepository<ForecastMonth>, IForecastMonthRepository
    {
        public ForecastMonthRepository(ISession openSession) : base(openSession)
        {
        }

        public ForecastMonthRepository(IActiveSessionManager activeSessionManager) : base(activeSessionManager)
        {
        }

        /// <summary>
        /// Gets forecast month by user id, month number and year
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public ForecastMonth GetByUserAndMonth(int userId, int month, int year)
        {
            return Session
                .QueryOver<ForecastMonth>()
                .Where(x => x.User.UserID == userId)
                .And(x => x.Month == month)
                .And(x => x.Year == year)
                .SingleOrDefault();
        }

        /// <summary>
        /// Gets forecast month by user-ids, month number and year
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public IEnumerable<ForecastMonth> GetByUsersAndMonth(IEnumerable<int> userIds, int month, int year)
        {
            ForecastMonth forecastMonthAlias = null;
            return Session
                .QueryOver(() => forecastMonthAlias)
                .Where(x => forecastMonthAlias.Month == month)
                .And(x => forecastMonthAlias.Year == year)
                .And(Restrictions.On(() => forecastMonthAlias.User.UserID).IsIn(userIds.ToList()))
                .List<ForecastMonth>();
        }

        /// <summary>
        /// Query builder for forecastMonth querying
        /// </summary>
        /// <param name="criterias"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public IEnumerable<ForecastMonth> GetBySearchCriterias(IEnumerable<IForecastMonthQueryCriteria> criterias, int month, int year)
        {
            ForecastMonth monthAlias = null;
            Forecast forecastAlias = null;

            var queryOver = Session
                            .QueryOver(() => monthAlias)
                            .JoinAlias(x => x.Forecasts, () => forecastAlias)
                            .Where(x => x.Month == month)
                            .And(x => x.Year == year);            

            queryOver = criterias.Aggregate(queryOver, (currentQuery, customQueryCriteria) => customQueryCriteria.Apply(currentQuery, monthAlias, forecastAlias));
            return queryOver
                .List<ForecastMonth>()
                .Distinct();
        }


        /// <summary>
        /// Handle for external flushing
        /// </summary>
        public void SessionFlush()
        {
            Session.Flush();
        }        
    }    
}