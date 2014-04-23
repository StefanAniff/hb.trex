using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.Forecast.Shared
{
    public class ForecastTypeProvider
    {
        private readonly IForecastService _forecastService;
        private List<ForecastType> _forecastTypes;

        public ForecastTypeProvider(IForecastService forecastService)
        {
            _forecastService = forecastService;
            _forecastTypes = new List<ForecastType>();
        }

        public virtual List<ForecastType> ForecastTypes 
        {
            get { return _forecastTypes; }
        }

        public virtual ForecastType Default
        {
            get { return _forecastTypes.FirstOrDefault(); } 
        }

        public async Task<IEnumerable<ForecastType>> Initialize()
        {
            if (_forecastTypes != null && _forecastTypes.Count > 0)
                return _forecastTypes;

            var response = await _forecastService.GetForecastTypes();
            _forecastTypes = response.ForecastTypeDtos.ToClient();

            return _forecastTypes;
        }
    }
}