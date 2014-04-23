using System;

namespace Trex.SmartClient.Forecast.ForecastOverview.Helpers
{
    /// <summary>
    /// Display handler for forecasttypes not supporting project-hours/registrations
    /// </summary>
    public class NonProjectSupportingDisplayHandler : ForecastOverviewDisplayHandlerBase
    {
        public NonProjectSupportingDisplayHandler(ForecastOverviewForecast forecast) : base(forecast)
        {
            if (forecast.ForecastType.SupportsProjectHours)
                throw new Exception(string.Format("Cannot use NonProjectSupportingDisplayHandler on a forecasttype that supports projecthours. ForecastId: {0}", forecast.ForecastId));
        }

        protected override void DoUpdateVisuals(OverviewForecastTypeOption typeOption)
        {
            if (typeOption.ForecastType.Id.Equals(Forecast.ForecastType.Id))
                ForceHideByForecastType = !typeOption.IsSelected;
        }

        public override string DisplayValue
        {
            get
            {
                return Forecast.DedicatedForecastTypeHours.HasValue
                           ? Forecast.DedicatedForecastTypeHours.Value.ToString(DecimalStringFormat)
                           : DefaultDisplayValue;
            }
        }
    }
}