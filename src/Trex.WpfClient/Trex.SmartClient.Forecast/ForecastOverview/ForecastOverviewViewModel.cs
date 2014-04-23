using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Forecast.ForecastOverview.Helpers;
using Trex.SmartClient.Forecast.Shared;
using System.Linq;

namespace Trex.SmartClient.Forecast.ForecastOverview
{
    public class ForecastOverviewViewModel : ViewModelBase, IForecastOverviewViewModel
    {
        private readonly ForecastOverviewDataGenerator _dataGenerator;
        private readonly IForecastService _forecastService;
        private readonly ForecastTypeProvider _forecastTypeProvider;
        private readonly IProjectRepository _projectRepository;
        private readonly ICompanyRepository _companyRepository;
        private bool _isBusy;
        private DateTime _selectedDate;
        private ForecastDates _dates = new ForecastDates();
        private ForecastOverviewForecastMonths _userRegistrations = new ForecastOverviewForecastMonths();

        #region Commands

        public DelegateCommand<object> NextMonthCommand { get; private set; }
        public DelegateCommand<object> PreviousMonthCommand { get; private set; }
        public DelegateCommand<object> CurrentMonthCommand { get; private set; }
        public DelegateCommand<object> RemoveUserCommand { get; private set; }
        public DelegateCommand<object> SearchCommand { get; private set; }
        public DelegateCommand<object> ClearAllCommand { get; private set; }
        public DelegateCommand<object> PrintCommand { get; private set; }

        #endregion

        #region Auto-props

        public List<ForecastType> ForecastTypes { get; set; }
        public OverviewListOptions ListOptions { get; set; }
        public ForecastOverviewSearchOptions SearchOptions { get; set; }

        #endregion


        public ForecastOverviewViewModel(ForecastOverviewDataGenerator dataGenerator
            , IForecastService forecastService
            , ForecastTypeProvider forecastTypeProvider
            , IProjectRepository projectRepository
            , ICompanyRepository companyRepository
            , ForecastOverviewSearchOptions searchOptions)
        {
            _dataGenerator = dataGenerator;
            _forecastService = forecastService;
            _forecastTypeProvider = forecastTypeProvider;
            _projectRepository = projectRepository;
            _companyRepository = companyRepository;
            ListOptions = new OverviewListOptions();
            SearchOptions = searchOptions;
        }

        public void Initialize()
        {
            InitializeCommands();
            InitializeCompositeCommands();

            SelectedDate = DateTime.Now.FirstDayOfMonth();
            InitializeAsync();
        }

        private void InitializeCompositeCommands()
        {
            ForecastLocalCompositeCommands.ForecastOverviewToggleForecatTypeHide.RegisterCommand(new DelegateCommand<object>(HideUserRegistrations));
        }

        private void HideUserRegistrations(object forecastTypeOption)
        {
            var option = forecastTypeOption as OverviewForecastTypeOption;
            if (option == null || UserRegistrations == null)
                return;

            UserRegistrations.UpdateForecastTypeForceHide(option);
        }

        private async void InitializeAsync()
        {
            // Forecasttypes from server
            ForecastTypes = new List<ForecastType>(await _forecastTypeProvider.Initialize());
            ListOptions.InitializeForecastTypes(ForecastTypes);
            SearchOptions.InitializeForecastTypes(ForecastTypes);

            // Users from server
            var optionsResponse = await _forecastService.GetOverivewSearchOptions();
            if (optionsResponse != null)
            {
                SearchOptions.InitializeUsers(optionsResponse.Users);                
            }

            // Projects from localstorage
            SearchOptions.InitializeProjects(_projectRepository.GetAllActive());

            // Companies from localstorage
            SearchOptions.InitializeCompanies(_companyRepository.GetAllActive());
        }

        private void InitializeCommands()
        {
            NextMonthCommand = new DelegateCommand<object>(_ => { SelectedDate = SelectedDate.FirstDayOfNextMonth(); }, x => !IsBusy);
            PreviousMonthCommand = new DelegateCommand<object>(_ => { SelectedDate = SelectedDate.FirstDayOfPreviousMonth(); }, x => !IsBusy);
            CurrentMonthCommand = new DelegateCommand<object>(_ => SelectedDate = DateTime.Now.FirstDayOfMonth(), x => !IsBusy);
            SearchCommand = new DelegateCommand<object>(_ => DoSearchForecasts(), x => !IsBusy);
            ClearAllCommand = new DelegateCommand<object>(_ => DoClearAll(), x => !IsBusy);
            PrintCommand = new DelegateCommand<object>(_ => DoPrint(), x => !IsBusy);

            RemoveUserCommand = new DelegateCommand<object>(x =>
                {
                    var usrMonth = x as ForecastOverviewForecastMonth;
                    if (usrMonth == null)
                        return;

                    UserRegistrations.Remove(usrMonth);
                } , x => !IsBusy);
        }

        private void DoPrint()
        {
            // Does nothing. Visualsprinting is in code-behind.
            // CanExecute is nice to have though
        }

        private void DoClearAll()
        {
            UserRegistrations = new ForecastOverviewForecastMonths();
            SearchOptions.Reset();

            // Reset setup
            foreach (var forecastTypeOption in ListOptions.ForecastTypeOptions)
            {
                forecastTypeOption.IsSelected = true;
            }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate == value)
                    return;

                _selectedDate = value;
                OnPropertyChanged(() => SelectedDate);
                OnSelectedDateChanged();
            }
        }

        private void OnSelectedDateChanged()
        {            
            OnPropertyChanged(() => SelectedMonthString);
            Dates = _dataGenerator.CreateDatesFromDate(SelectedDate);
            UserRegistrations = new ForecastOverviewForecastMonths();

            DoSearchForecasts();
        }

        public async void DoSearchForecasts()
        {
            IsBusy = true;
            UserRegistrations = new ForecastOverviewForecastMonths();
            var response = await SearchOptions.DoSearch(SelectedDate.Month, SelectedDate.Year);
            if (response == null)
            {
                // An error occured
                IsBusy = false;
                return;
            }

            _dataGenerator.MergeHolidays(Dates, response.Holidays);
            _dataGenerator.CreateForecastMonths(this, response.ForecastMonths, response.ProjectForecastTypeId);
            UpdateForecastVisualSettings(response.ProjectForecastTypeId);

            IsBusy = false;
        }

        private void UpdateForecastVisualSettings(int projectForecastTypeId)
        {
            // Apply project-search visibility
            foreach (var usrMonth in UserRegistrations)
            {
                SearchOptions.SetupVisualsBySearchOption(usrMonth, projectForecastTypeId);

                // Apply user-forced visibility 
                foreach (var typeOption in ListOptions.ForecastTypeOptions)
                {
                    foreach (var forecast in usrMonth.Forecasts.Where(x => x.Date.IsWorkDay))
                    {
                        forecast.UpdateVisuals(typeOption);
                    }
                }   
            }
        }

        public string SelectedMonthString { get { return string.Format("{0} {1}", SelectedDate.ToString("MMMM", CultureInfo.InvariantCulture), SelectedDate.Year); } }

        public ForecastDates Dates
        {
            get { return _dates; }
            set
            {
                _dates = value;
                OnPropertyChanged(() => Dates);
            }
        }

        public ForecastOverviewForecastMonths UserRegistrations
        {
            get { return _userRegistrations; }
            set
            {
                _userRegistrations = value;
                OnPropertyChanged(() => UserRegistrations);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
                RaiseCanExecuteCommands();
            }
        }        
        
        private void RaiseCanExecuteCommands()
        {
            NextMonthCommand.RaiseCanExecuteChanged();
            PreviousMonthCommand.RaiseCanExecuteChanged();
            RemoveUserCommand.RaiseCanExecuteChanged();
            CurrentMonthCommand.RaiseCanExecuteChanged();
            SearchCommand.RaiseCanExecuteChanged();
            ClearAllCommand.RaiseCanExecuteChanged();
            PrintCommand.RaiseCanExecuteChanged();
        }
        
        public void Dispose() { }
    }
}