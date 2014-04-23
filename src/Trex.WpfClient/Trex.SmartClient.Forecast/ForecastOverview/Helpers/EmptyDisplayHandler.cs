namespace Trex.SmartClient.Forecast.ForecastOverview.Helpers
{
    /// <summary>
    /// Empty display handler for "null" objects
    /// </summary>
    public class EmptyDisplayHandler : ForecastOverviewDisplayHandlerBase
    {
        public EmptyDisplayHandler(ForecastOverviewForecast forecast) : base(forecast)
        {
        }

        protected override void DoUpdateVisuals(OverviewForecastTypeOption typeOption)
        {
            // Do nothing
        }

        public override string DisplayValue
        {
            get { return string.Empty; }
        }
    }
}