using Trex.SmartClient.Core.Implemented;
using System.Linq;

namespace Trex.SmartClient.Forecast.ForecastOverview.Helpers
{
    /// <summary>
    /// Base class for visual handling of a forecast in the ForecastOverview.
    /// (Visibility and visual-value)
    /// </summary>
    public abstract class ForecastOverviewDisplayHandlerBase : ViewModelBase
    {
        protected ForecastOverviewForecast Forecast;
        protected const string DecimalStringFormat = "0.##";
        
        private bool _forceHideByForecastType;

        protected ForecastOverviewDisplayHandlerBase(ForecastOverviewForecast forecast)
        {
            Forecast = forecast;
        }

        /// <summary>
        /// Set by list options, if use wants to hide the status
        /// </summary>
        protected bool ForceHideByForecastType
        {
            get { return _forceHideByForecastType; }
            set
            {
                _forceHideByForecastType = value;
                OnPropertyChanged(() => ForceHide);
            }
        }

        /// <summary>
        /// Try to sum projecthours and dedicatedhours.
        /// If zero then return letter for forecasttype
        /// </summary>
        protected string DefaultDisplayValue
        {
            get
            {
                var projectHours = Forecast.Projects.Sum(x => x.Hours);
                var dedicatedHours = Forecast.DedicatedForecastTypeHours.HasValue
                                         ? Forecast.DedicatedForecastTypeHours.Value
                                         : decimal.Zero;
                var total = projectHours + dedicatedHours;

                if (total != decimal.Zero)
                    return total.ToString(DecimalStringFormat);

                return Forecast.ForecastType != null
                           ? Forecast.ForecastType.Letter
                           : string.Empty;
            }
        }

        /// <summary>
        /// Update visual-state
        /// </summary>
        /// <param name="typeOption"></param>
        public void UpdateVisuals(OverviewForecastTypeOption typeOption)
        {
            DoUpdateVisuals(typeOption);
            OnPropertyChanged(() => DisplayValue);
        }

        /// <summary>
        /// Visibility indicator
        /// </summary>
        public virtual bool ForceHide { get { return ForceHideByForecastType; } }

        /// <summary>
        /// Updates the visual state for this forecast (Both visibility and displayvalue)
        /// </summary>
        /// <param name="typeOption"></param>
        protected abstract void DoUpdateVisuals(OverviewForecastTypeOption typeOption);

        /// <summary>
        /// Returns the displayvalue
        /// </summary>
        public abstract string DisplayValue { get; }
    }
}