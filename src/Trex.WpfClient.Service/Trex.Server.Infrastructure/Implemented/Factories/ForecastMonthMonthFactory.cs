using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented.Factories
{    
    public class ForecastMonthMonthFactory : IForecastMonthFactory
    {
        private readonly IDomainSettings _domainSettings;

        public ForecastMonthMonthFactory(IDomainSettings domainSettings)
        {
            _domainSettings = domainSettings;
        }

        public ForecastMonth CreateForecastMonth(int month, int year, User user, User createdBy)
        {
            return new ForecastMonth(month, year, _domainSettings.PastMonthsDayLock ,user, createdBy);
        }
    }
}