using Telerik.Windows.Controls;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Forecast.ForecastStatistics;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    public class ForecastRegistrationRootViewModel : ViewModelBase, IForecastRegistrationRootViewModel
    {
        private readonly IAppSettings _appSettings;
        private IForecastRegistrationViewModel _forecastRegistrationVm;
        private IForecastStatisticsTabViewModel _forecastStatisticsTabVm;

        public ForecastRegistrationRootViewModel(IAppSettings appSettings
            , IForecastRegistrationViewModel forecastRegistrationVm
            , IForecastStatisticsTabViewModel forecastStatisticsTabVm)
        {
            _appSettings = appSettings;
            ForecastRegistrationVm = forecastRegistrationVm;
            ForecastStatisticsTabVm = forecastStatisticsTabVm;
        }

        public double ForecastStatisticsTabHeight
        {
            get { return _appSettings.TabForecastStatisticsHeight; }
            set
            {
                _appSettings.TabForecastStatisticsHeight = value;
                _appSettings.Save();
            }
        }

        public IForecastRegistrationViewModel ForecastRegistrationVm
        {
            get { return _forecastRegistrationVm; }
            set
            {
                _forecastRegistrationVm = value;
                OnPropertyChanged(() => ForecastRegistrationVm);
                _forecastRegistrationVm.Initialize();
            }
        }

        public IForecastStatisticsTabViewModel ForecastStatisticsTabVm
        {
            get { return _forecastStatisticsTabVm; }
            set
            {
                _forecastStatisticsTabVm = value;
                OnPropertyChanged(() => ForecastStatisticsTabVm);
                _forecastStatisticsTabVm.Initialize();
            }
        }
    }
}