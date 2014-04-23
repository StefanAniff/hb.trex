using System;
using System.Collections.Generic;

namespace Trex.Server.Core.Services
{
    public interface IDomainSettings
    {
        /// <summary>
        /// Vacation ForecastType id.
        /// </summary>
        int VacationForecastTypeId { get; }

        /// <summary>
        /// Day in current month indicated when previous months are locked.
        /// Fx. if value is 3 and the date today is 4-11-2013, 
        /// all prevoius months will be locked for update
        /// </summary>
        int PastMonthsDayLock { get; }

        /// <summary>
        /// Project ForecastType id
        /// </summary>
        int ProjectForecastTypeId { get; }

        /// <summary>
        /// Minimum startdate for timeentriess
        /// </summary>
        DateTime TimeEntryMinStartDate { get; }
    }
}