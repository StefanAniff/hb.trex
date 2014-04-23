using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Trex.SmartClient.Forecast.ForecastOverview
{
    public class ForecastOverviewForecastMonth
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public List<ForecastOverviewForecast> Forecasts { get; set; }
    }

    public class ForecastOverviewForecastMonths : ObservableCollection<ForecastOverviewForecastMonth>
    {
        public ForecastOverviewForecastMonths()
        {
        }

        public ForecastOverviewForecastMonths(IEnumerable<ForecastOverviewForecastMonth> collection) : base(collection)
        {
        }

        public void UpdateForecastTypeForceHide(OverviewForecastTypeOption option)
        {
            foreach (var forecast in this.SelectMany(x => x.Forecasts))
            {
                forecast.UpdateVisuals(option);
            }
        }
    }
}