using System;
using Microsoft.Practices.Prism.Commands;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Forecast.ForecastRegistration.Helpers;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.Forecast.ForecastStatistics
{    
    public class ForecastStatisticsTabViewModel : ViewModelBase, IForecastStatisticsTabViewModel
    {
        private readonly IForecastService _forecastService;
        private readonly ForecastRegistrationSelectedUserHandler _selectedUserHandler;
        private ForecastStatisticsDto _statistics;
        private bool _isBusy;

        public ForecastStatisticsTabViewModel(IForecastService forecastService, ForecastRegistrationSelectedUserHandler selectedUserHandler)
        {
            _forecastService = forecastService;
            _selectedUserHandler = selectedUserHandler;
        }

        public void Initialize()
        {
            InitializeApplicationCommands();
        }

        private void InitializeApplicationCommands()
        {
            ApplicationCommands.GetForecastStatistics.RegisterCommand(new DelegateCommand<object>(GetStatistics));
        }

        private async void GetStatistics(object currentMonth)
        {
            var month = currentMonth as DateTime?;
            if (!month.HasValue)
                return;

            IsBusy = true;
            var response = await _forecastService.GetForecastStatistics(month.Value, _selectedUserHandler.UserId);
            if (response == null)
            {
                Statistics = null;
                IsBusy = false;
                return;
            }

            Statistics = response.ForecastStatistics;
            IsBusy = false;
        }        
        
        public void Dispose() { }

        public ForecastStatisticsDto Statistics
        {
            get { return _statistics; }
            set
            {
                _statistics = value;
                OnPropertyChanged(() => Statistics);
                OnPropertyChanged(() => CurrentYear);
                OnPropertyChanged(() => Next12MonthsDateSpanString);
                OnPropertyChanged(() => CurrentVacationPeriodString);
                OnPropertyChanged(() => NextVacationperiodString);
            }
        }

        public int CurrentYear
        {
            get { return DateTime.Now.Year; }
        }

        public string Next12MonthsDateSpanString
        {
            get { return _statistics != null ? string.Format("{0} - {1}", _statistics.Next12MonthsDateSpan.From.ToShortDateString(), _statistics.Next12MonthsDateSpan.To.ToShortDateString()) : string.Empty; }
        }

        public string CurrentVacationPeriodString
        {
            get { return _statistics != null ? string.Format("{0} - {1}", _statistics.PlannedVacationCurrentDateSpan.From.ToShortDateString(), _statistics.PlannedVacationCurrentDateSpan.To.ToShortDateString()) : string.Empty; }
        }

        public string NextVacationperiodString
        {
            get { return _statistics != null ? string.Format("{0} - {1}", _statistics.PlannedVacationNextDateSpan.From.ToShortDateString(), _statistics.PlannedVacationNextDateSpan.To.ToShortDateString()) : string.Empty; }
        }

        [NoDirtyCheck]
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
            }
        }
    }
}