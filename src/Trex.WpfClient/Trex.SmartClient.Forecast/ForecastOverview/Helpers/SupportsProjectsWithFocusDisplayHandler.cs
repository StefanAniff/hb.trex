using System.Linq;

namespace Trex.SmartClient.Forecast.ForecastOverview.Helpers
{
    /// <summary>
    /// Display handler for forecasts supporting projects (without being the main "Project" forecasttype).
    /// Behaviour is to sum dedicatedhours and projecthours, and also substracting hours depending on 
    /// which forecasttypes the user disables.
    /// Visibility for this type therefore depends on it's own forecasttype and the "Project"-forecasttype
    /// Use when searchcriterias includes project or company
    /// </summary>
    public class SupportsProjectsWithFocusDisplayHandler : ForecastOverviewDisplayHandlerBase
    {
        /// <summary>
        /// Optional project id.
        /// Set if searching by project
        /// </summary>
        private readonly int? _projectFocusId;

        /// <summary>
        /// Optional company id.
        /// Set if searching by company
        /// </summary>
        private readonly int? _companyFocusId;

        /// <summary>
        /// Id for the "Project" forecasttype
        /// </summary>
        private readonly int _projectForecastTypeId;

        /// <summary>
        /// Indicates that forecasttype "Project" has been visually disabled
        /// </summary>
        private bool _projectTypeDisabled;

        /// <summary>
        /// Indicates that this' forecasttype has been visually disabled
        /// </summary>
        private bool _ownTypeDisabled;

        /// <summary>
        /// Indicates that this is related to the current project or company 
        /// search criteria
        /// </summary>
        private bool _isRelatedToCurrentProjectOrCompany;

        public SupportsProjectsWithFocusDisplayHandler(ForecastOverviewForecast forecast, int? projectFocusId, int? companyFocusId, int projectForecastTypeId)
            : base(forecast)
        {
            _projectFocusId = projectFocusId;
            _companyFocusId = companyFocusId;
            _projectForecastTypeId = projectForecastTypeId;
            InitializeProjectOrCompanyRelation();
        }

        /// <summary>
        /// Find out if this has any project relation to current project/company searchcriteria
        /// </summary>
        private void InitializeProjectOrCompanyRelation()
        {
            var hasProjectRelation = _projectFocusId.HasValue && (Forecast.Projects.Any(x => x.ProjectId == _projectFocusId.Value));
            var hasCompanyRelation = _companyFocusId.HasValue && (Forecast.Projects.Any(y => y.CompanyId == _companyFocusId.Value));

            _isRelatedToCurrentProjectOrCompany = hasProjectRelation || hasCompanyRelation;
        }

        /// <summary>
        /// Sets up the state of the object, so displayvalue returns the expected value
        /// </summary>
        /// <param name="typeOption">Forecasttype that has been enabled/disabled</param>
        protected override void DoUpdateVisuals(OverviewForecastTypeOption typeOption)
        {
            // Toggled forecasttype is "Project"
            if (typeOption.ForecastType.Id.Equals(_projectForecastTypeId))
            {
                _projectTypeDisabled = !typeOption.IsSelected;
            }
            // Toggled forecasttype is the same as this objects forecasttype
            else if (typeOption.ForecastType.Id.Equals(Forecast.ForecastType.Id))
            {
                _ownTypeDisabled = !typeOption.IsSelected;
            }

            // ForceHide if this forecast is not related to current project/company search or both "Project" and forecasts own type are disabled
            ForceHideByForecastType = (!IsRelatedToCurrentProjectOrCompany || _projectTypeDisabled) && _ownTypeDisabled;
        }

        public override string DisplayValue
        {
            get
            {
                // Forecasttype "Project" is enabled and this is related to current Project/Company search criteria
                if (!_projectTypeDisabled && IsRelatedToCurrentProjectOrCompany)
                {
                    // When project in search-focus, then show project releated hours + type-dedicated hours
                    if (_projectFocusId.HasValue)
                    {
                        return CalculateHoursStringByProject(_projectFocusId.Value);
                    }

                    // When company in search-focus, then show company releated hours + type-dedicated hours
                    if (_companyFocusId.HasValue)
                    {
                        return CalculateHoursStringByCompany(_companyFocusId.Value);
                    }
                }
                // Type of this is not disabled and dedicated hours has a value
                else if (!_ownTypeDisabled && Forecast.DedicatedForecastTypeHours.HasValue)
                {
                    return Forecast.DedicatedForecastTypeHours.Value.ToString(DecimalStringFormat);
                }                
      
                return DefaultDisplayValue;
            }
        }

        /// <summary>
        /// Indicates that this is related to the current project or company 
        /// search criteria
        /// </summary>
        public virtual bool IsRelatedToCurrentProjectOrCompany
        {
            get { return _isRelatedToCurrentProjectOrCompany; }
        }

        private string CalculateHoursStringByCompany(int companyId)
        {
            var projectHours = Forecast.SumHoursByCompanyId(companyId);
            return TryAddDedicatedHours(projectHours);
        }

        private string CalculateHoursStringByProject(int projectId)
        {
            var projectHours = Forecast.SumHoursByProjectId(projectId);
            return TryAddDedicatedHours(projectHours);
        }

        private string TryAddDedicatedHours(decimal toAdd)
        {
            var dedicatedHours = GetActiveDedicatedHours();
            var totalHours = toAdd + dedicatedHours;
            return totalHours != decimal.Zero ? totalHours.ToString(DecimalStringFormat) : string.Empty;
        }

        private decimal GetActiveDedicatedHours()
        {
            // Only include dedicated hours if own type is enabled
            return !_ownTypeDisabled ? Forecast.DedicatedHours : decimal.Zero;
        }
    }
}