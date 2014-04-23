namespace Trex.SmartClient.Forecast.ForecastOverview.Helpers
{
    /// <summary>
    /// Display handler for forecasts supporting projectregistrations (without being the main "Project" forecasttype).
    /// Use when searchcriterias does not include project or company
    /// </summary>
    public class SupportsProjectsNoFocusDisplayHandler : ForecastOverviewDisplayHandlerBase
    {
        private readonly int _projectForecastTypeId;
        private bool _projectTypeDisabled;
        private bool _ownTypeDisabled;

        public SupportsProjectsNoFocusDisplayHandler(ForecastOverviewForecast forecast, int projectForecastTypeId)
            : base(forecast)
        {
            _projectForecastTypeId = projectForecastTypeId;
        }

        protected override void DoUpdateVisuals(OverviewForecastTypeOption typeOption)
        {
            // Toggled forecasttype is "Project"
            if (typeOption.ForecastType.Id.Equals(_projectForecastTypeId))
            {
                _projectTypeDisabled = !typeOption.IsSelected;
            }
                // Toggled forecasttype is the same as this objects forecasttype
            else if (typeOption.ForecastType.Id.Equals(Forecast.ForecastType.Id)) // Same type as this
            {
                _ownTypeDisabled = !typeOption.IsSelected;
            }

            ForceHideByForecastType = _ownTypeDisabled && (_projectTypeDisabled || Forecast.HasNoProjects);
        }

        public override string DisplayValue
        {
            get
            {
                var result = decimal.Zero;
                if (!_projectTypeDisabled)
                    result = Forecast.SumAllProjectHours;

                if (!_ownTypeDisabled)
                    result += Forecast.DedicatedHours;

                if (result != decimal.Zero)
                    return result.ToString(DecimalStringFormat);

                return DefaultDisplayValue;
            }
        }
    }
}