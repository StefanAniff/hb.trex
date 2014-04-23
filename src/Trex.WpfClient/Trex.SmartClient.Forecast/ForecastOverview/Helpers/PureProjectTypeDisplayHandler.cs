using System.Linq;

namespace Trex.SmartClient.Forecast.ForecastOverview.Helpers
{
    /// <summary>
    /// Display handler for forecast on the forecasttype "Project"
    /// </summary>
    public class PureProjectTypeDisplayHandler : ForecastOverviewDisplayHandlerBase
    {
        private bool _forceHideByProjectOrClient;
        private readonly int? _companyFocusId;
        private readonly int? _projectFocusId;

        public PureProjectTypeDisplayHandler(ForecastOverviewForecast forecast, int? projectFocusId, int? companyFocusId)
            : base(forecast)
        {
            _projectFocusId = projectFocusId;
            _companyFocusId = companyFocusId;
            InitializeForceHideByProjectOrClient();
        }

        /// <summary>
        /// If there is search-criteria is project or client
        /// and type is Project, then return true, if not containing
        /// relation to project/client if focus
        /// </summary>
        private void InitializeForceHideByProjectOrClient()
        {
            var hideByProject = _projectFocusId.HasValue
                                && (Forecast.Projects.All(x => x.ProjectId != _projectFocusId)
                                    || !Forecast.Projects.Any());

            var hideByCompany = _companyFocusId.HasValue
                                && (Forecast.Projects.All(y => y.CompanyId != _companyFocusId.Value)
                                    || !Forecast.Projects.Any());

            _forceHideByProjectOrClient = hideByProject || hideByCompany;
        }

        /// <summary>
        /// Hide status depending on toggled forecasttypeOption
        /// </summary>
        /// <param name="typeOption"></param>
        protected override void DoUpdateVisuals(OverviewForecastTypeOption typeOption)
        {
            if (typeOption.ForecastType.Id.Equals(Forecast.ForecastType.Id))
            {
                ForceHideByForecastType = !typeOption.IsSelected;
            }
        }

        /// <summary>
        /// Only show displayvalue
        /// </summary>
        public override string DisplayValue
        {
            get
            { 
                if (_forceHideByProjectOrClient)
                    return string.Empty;

                if (_projectFocusId.HasValue)
                {
                    var projectHours = Forecast.SumHoursByProjectId(_projectFocusId.Value);
                    return projectHours != 0 ? projectHours.ToString(DecimalStringFormat) : string.Empty;
                }

                if (_companyFocusId.HasValue)
                {
                    var projectHours = Forecast.SumHoursByCompanyId(_companyFocusId.Value);
                    return projectHours != 0 ? projectHours.ToString(DecimalStringFormat) : string.Empty;
                }

                return DefaultDisplayValue;
            }
        }

        public override bool ForceHide
        {
            get { return _forceHideByProjectOrClient || ForceHideByForecastType; }
        }
    }
}