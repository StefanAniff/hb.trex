using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Forecast.ForecastOverview.Helpers;
using Trex.SmartClient.Forecast.Shared;
using System.Linq;
using Trex.SmartClient.Core.Extensions;

namespace Trex.SmartClient.Forecast.ForecastOverview.DesignData
{
    public class DesignForecastOverviewViewModel : IForecastOverviewViewModel
    {
        private static readonly ForecastOverviewDataGenerator _dataGenerator = new ForecastOverviewDataGenerator();

        public DesignForecastOverviewViewModel()
        {
            SelectedDate = DateTime.Now.FirstDayOfMonth();
        }

        public void Dispose() { }
        public void Initialize() { }

        public bool IsBusy { get; set; }        

        public DelegateCommand<object> NextMonthCommand { get; private set; }
        public DelegateCommand<object> PreviousMonthCommand { get; private set; }
        public DelegateCommand<object> CurrentMonthCommand { get; private set; }
        public DelegateCommand<object> SearchCommand { get; set; }

        public DateTime SelectedDate { get; set; }

        public string SelectedMonthString { get { return string.Format("{0} {1}", SelectedDate.ToString("MMMM", CultureInfo.InvariantCulture), SelectedDate.Year); ; } private set { } }

        private ForecastDates _dates = CreateDates();
        private List<ForecastType> _forecastTypes = CreateTypes();

        private static List<ForecastType> CreateTypes()
        {
            var result = new List<ForecastType>
                {
                    _project,
                    _vacation,
                    _training,
                    _illness,
                    _open
                };

            return result;
        }

        private static ForecastDates CreateDates()
        {
            var result = _dataGenerator.CreateDatesFromDate(DateTime.Now);

            var toHoliday = result.Where(x => x.Date.Day < 8 && !x.IsWeekend);
            foreach (var forecastDateViewModel in toHoliday)
            {
                forecastDateViewModel.SetToHoliday(new HolidayDto { Date = forecastDateViewModel.Date, Description = "Holiday" });
            }
            return result;
        }

        public ForecastDates Dates 
        { 
            get { return _dates; } 
            set {  } 
        }

        private ForecastOverviewForecast CreateForecast(ForecastDate date, ForecastType forecastType, int? dedicatedHours = null)
        {
            var forecast = new ForecastOverviewForecast
                {
                    Date = date,
                    ForecastType = forecastType,
                    DedicatedForecastTypeHours = dedicatedHours
                };

            forecast.DisplayHandler = new SupportsProjectsWithFocusDisplayHandler(forecast, null, null, _project.Id);
            return forecast;
        }

        public ForecastOverviewForecastMonths UserRegistrations
        {
            get
            {
                var result = new ForecastOverviewForecastMonths
                    {
                        new ForecastOverviewForecastMonth
                            {
                                UserId = 1,
                                UserName = "yyy",
                                Forecasts =
                                    new List<ForecastOverviewForecast>(
                                        _dates.Select(x => CreateForecast(x, _project)))
                            },
                            new ForecastOverviewForecastMonth
                            {
                                UserId = 2,
                                UserName = "xxx",
                                Forecasts =
                                    new List<ForecastOverviewForecast>(
                                        _dates.Select(x => CreateForecast(x, _vacation)))
                            },
                            new ForecastOverviewForecastMonth
                            {
                                UserId = 2,
                                UserName = "zzz",
                                Forecasts =
                                    new List<ForecastOverviewForecast>(
                                        _dates.Select(x => CreateForecast(x, _illness, 7)))
                            }
                    };


                return result;
            }
            set {}
        }

        public OverviewListOptions ListOptions
        {
            get { return new OverviewListOptions
                {
                    ForecastTypeOptions = new ObservableCollection<OverviewForecastTypeOption>(_forecastTypes.Select(x => new OverviewForecastTypeOption(x)))
                };} 
            set{}
        }

        public ForecastOverviewSearchOptions SearchOptions
        {
            get
            {
                var userSearchOptions = new ForecastOverviewUserSearchOptions(null)
                    {
                        UserListPresets = new ObservableCollection<ForecastOverviewUserSearchOptions.UserListPreset>
                            {
                                new ForecastOverviewUserSearchOptions.UserListPreset { Name = "Team X"},
                                new ForecastOverviewUserSearchOptions.UserListPreset { Name = "Team Y", IsEditMode = true}
                            }
                    };

                return new ForecastOverviewSearchOptions(null, userSearchOptions)
                    {
                        SelectedUsers = new ObservableCollection<ForecastUserDto>
                            {
                                new ForecastUserDto { Name = "Some awesome dude"},
                                new ForecastUserDto { Name = "The dude"}
                            },
                        SelectedForecastType = _vacation,
                    };
            } 
            set{}
        }

        public DelegateCommand<object> ClearAllCommand { get; private set; }
        public DelegateCommand<object> PrintCommand { get; set; }

        private static readonly ForecastType _project = new ForecastType
        {
            ColorStringHex = "#FF696969",
            Name = "Project",
            SupportsProjectHours = true,
            SupportsDedicatedHours = false
        };

        private static readonly ForecastType _training = new ForecastType
        {
            ColorStringHex = "#FFFFFF00",
            Name = "Training",
            SupportsProjectHours = true,
            SupportsDedicatedHours = true
        };

        private static readonly ForecastType _open = new ForecastType
        {
            ColorStringHex = "#FF008000",
            Name = "Open",
            SupportsProjectHours = true,
            SupportsDedicatedHours = true
        };

        private static readonly ForecastType _illness = new ForecastType
        {
            ColorStringHex = "#FF8B0000",
            Name = "Illness",
            SupportsProjectHours = false,
            SupportsDedicatedHours = true
        };

        private static readonly ForecastType _vacation = new ForecastType
        {
            ColorStringHex = "#FF9400D3",
            Name = "Vacation",
            SupportsProjectHours = false,
            SupportsDedicatedHours = false
        };
    }
}