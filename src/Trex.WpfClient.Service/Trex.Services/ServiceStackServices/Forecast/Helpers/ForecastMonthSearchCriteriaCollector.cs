using System;
using System.Collections.Generic;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Services;
using TrexSL.Web.ServiceStackServices.Forecast.ForecastMonthCriterias;

namespace TrexSL.Web.ServiceStackServices.Forecast.Helpers
{
    public class ForecastMonthSearchCriteriaCollector
    {
        public virtual IEnumerable<IForecastMonthQueryCriteria> Collect(ForecastSearchByRegistrationRequest searchRequest)
        {
            EnsureCombinationIsValid(searchRequest);
            var result = new List<IForecastMonthQueryCriteria>();

            if (searchRequest.ForecastTypeId.HasValue)
                result.Add(new ForcastTypeQueryCriteria(searchRequest.ForecastTypeId.Value));
            
            if (searchRequest.ProjectId.HasValue)
                result.Add(new ForecastProjectQueryCriteria(searchRequest.ProjectId.Value));

            if (searchRequest.CompanyId.HasValue)
                result.Add(new ForecastCompanyQueryCriteria(searchRequest.CompanyId.Value));

            return result;
        }

        /// <summary>
        /// Ensures search value combination is valid
        /// </summary>
        /// <param name="request"></param>
        private void EnsureCombinationIsValid(ForecastSearchByRegistrationRequest request)
        {
            // Company and project combination does not make sence, since project is child of company
            if (request.CompanyId.HasValue
                && request.ProjectId.HasValue)
                throw new Exception(string.Format("CompanyId and ProjectId search combination does not make sence"));
        }
    }
}