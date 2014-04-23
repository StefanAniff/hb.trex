using System.Collections.Generic;
using Trex.Common.DataTransferObjects;
using Trex.Server.Core.Model.Forecast;

namespace Trex.Server.Core.Services
{
    public interface IForecastRepository : IRepository<Forecast>
    {
        /// <summary>
        /// Gets forecasts supporting client hours
        /// for the current year
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="queryInternalCustomers"></param>
        /// <param name="dateSpan"></param>
        /// <returns></returns>
        decimal GetHourSumByCriteria(int userId, bool queryInternalCustomers, DateSpan dateSpan);

        /// <summary>
        /// Returns the forecast count constrained by user, forecastype 
        /// and datespan
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <param name="forecastTypeId">ForecastType's id</param>
        /// <param name="dateSpan">Datespan to search in</param>
        /// <returns></returns>
        int GetForecastCountByForecastType(int userId, int forecastTypeId, DateSpan dateSpan);
    }
}