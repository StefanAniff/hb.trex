using System.Collections.Generic;
using NHibernate;
using Trex.Server.Core.Model.Forecast;

namespace Trex.Server.Core.Services
{
    public interface IForecastMonthRepository : IRepository<ForecastMonth>
    {
        ForecastMonth GetByUserAndMonth(int userId, int month, int year);
        void SessionFlush();

        /// <summary>
        /// Query builder for forecastMonth querying
        /// </summary>
        /// <param name="criterias"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        IEnumerable<ForecastMonth> GetBySearchCriterias(IEnumerable<IForecastMonthQueryCriteria> criterias, int month, int year);

        /// <summary>
        /// Gets forecast month by user-ids, month number and year
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        IEnumerable<ForecastMonth> GetByUsersAndMonth(IEnumerable<int> userIds, int month, int year);
    }

    public interface IForecastMonthQueryCriteria
    {
        IQueryOver<ForecastMonth, ForecastMonth> Apply(IQueryOver<ForecastMonth, ForecastMonth> queryOver, ForecastMonth monthAlias, Forecast forecastAlias);
    }
}