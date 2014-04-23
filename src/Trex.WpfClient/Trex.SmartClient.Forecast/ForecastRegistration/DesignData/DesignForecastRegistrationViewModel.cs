using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Practices.Prism.Commands;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using System.Linq;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Forecast.ForecastRegistration.Helpers;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast.ForecastRegistration.DesignData
{
    public class DesignForecastRegistrationViewModel : ViewModelDirtyHandlingBase, IForecastRegistrationViewModel
    {
        public ProjectRegistrations ProjectRegistrations
        {
            get
            {                
                return new ProjectRegistrations
                    {
                        CreateClientRegistration("d60"), 
                        CreateClientRegistration("Danske Commodities"), 
                        CreateClientRegistration("PTS"), 
                        CreateClientRegistration("1"),
                        CreateClientRegistration("2"),
                        CreateClientRegistration("3"),
                        CreateClientRegistration("4"),
                        CreateClientRegistration("5"),
                        CreateClientRegistration("6"),
                        CreateClientRegistration("7"),
                        CreateClientRegistration("8"),
                        CreateClientRegistration("9"),
                        //CreateClientRegistration("10"),
                        //CreateClientRegistration("11"),
                        //CreateClientRegistration("12"),
                        //CreateClientRegistration("13"),
                        //CreateClientRegistration("14"),
                        //CreateClientRegistration("15"),
                        //CreateClientRegistration("16"),
                        //CreateClientRegistration("17"),
                    };
            }
            set { }
        }

        public ProjectRegistration CreateClientRegistration(string clientName)
        {
            var client1 = new ProjectRegistration { ProjectName = clientName };
            client1.Registrations = CreateClientDateRegistrations(client1);
            return client1;
        }


        private IEnumerable<HourRegistration> CreateHourRegistrations()
        {
            var dates = CreateDateHeaders();
            var result = new List<HourRegistration>();
            foreach (var forecastDateColumn in dates)
            {
                var hourReg = new HourRegistration {Hours = 6.5m};
                forecastDateColumn.DateTotal = hourReg;
                result.Add(hourReg);
            }

            result.Last().Hours = 0;
            return result;
        }
        private ObservableCollection<ProjectHourRegistration> CreateClientDateRegistrations(ProjectRegistration parent)
        {
            var dateColumns = CreateDateHeaders();
            var result = new ObservableCollection<ProjectHourRegistration>();
            foreach (var forecastDateColumn in dateColumns)
            {
                var clientHourReg = new ProjectHourRegistration(parent) { Hours = 6.5m };
                forecastDateColumn.AddProjectHours(clientHourReg);
                result.Add(clientHourReg);
            }
            
            result.Last().Hours = 0;
            return result;            
        }

        private ForecastDateColumns CreateDateHeaders()
        {
            var dates = GenereateDatesForMonth(4, 2013);
            var result = new ForecastDateColumns();

            foreach (var date in dates)
            {
                var newItem = new ForecastRegistrationDateColumn(date);

                // Set a couple of days to be holiday
                if (date.Day == 2 || date.Day == 3)
                {
                    var holiday = new HolidayDto { Date = date, Description = "Holiday" };
                    newItem.SetToHoliday(holiday);
                }                
                result.Add(newItem);
            }

            return result;
        }

        private IEnumerable<DateTime> GenereateDatesForMonth(int month, int year)
        {
            var dateInMonth = new DateTime(year, month, 1);
            while (dateInMonth.Month == month)
            {
                yield return dateInMonth;
                dateInMonth = dateInMonth.AddDays(1);
            }
        }

        public ForecastDateColumns DateColumns
        {
            get { return CreateDateHeaders(); }
            set {}
        }

        public ProjectSearchViewModel ProjectSearchViewModel
        {
            get
            {
                return new ProjectSearchViewModel(null)
                    {
                        CompanyProjectSearchString = "d60",
                        SearchResult = new ObservableCollection<Project>
                            {
                                Project.Create(1, "Project 1", Company.Create("Wee", 1, false, false), false),
                                Project.Create(2, "Project 2", Company.Create("Woo", 2, false, false), false),                                
                            }
                    };
            }
        }

        public bool Initializing { get; set; }

        public IEnumerable<ForecastTypeRegistration> PresenceRegistrations
        {
            get
            {
                var commandHandler = new CopyStatusCommandHandler();
                var dates = CreateHourRegistrations();
                var result = new ObservableCollection<ForecastTypeRegistration>();
                foreach (var d in dates)
                {
                    var newItem = new ForecastTypeRegistration(_Project, null) {DateColumn = d.DateColumn};
                    result.Add(newItem);
                    commandHandler.InitializePresenceCopyCommands(newItem, result);
                }

                result[3].SelectedForecastType = _vacation;
                result[4].SelectedForecastType = _vacation;

                SetForecastTypeAndDedicatedHours(result[7], _open, 2.5m);
                SetForecastTypeAndDedicatedHours(result[8], _open, 2);
                SetForecastTypeAndDedicatedHours(result[9], _open, 2);
                SetForecastTypeAndDedicatedHours(result[10], _training, 5);
                SetForecastTypeAndDedicatedHours(result[11], _training, 5);                

                return result;
            }
            set { }
        }

        private void SetForecastTypeAndDedicatedHours(ForecastTypeRegistration forecastTypeReg, ForecastType forecastType, decimal? dedicatedHours)
        {
            forecastTypeReg.SelectedForecastType = forecastType;
            forecastTypeReg.DedicatedHours = dedicatedHours;
        }

        public ObservableCollection<ForecastType> ForecastTypes
        {
            get
            {
                return new ObservableCollection<ForecastType>
                    {
                        _Project, _training, _open, _illness, _vacation
                    };
            }
            set {}
        }

        public decimal DateRealizedTotalsSum
        {
            get
            {
                return DateRealizedTotals.Sum(x => x.GetValueOrDefault());
            }
        }

        [NoDirtyCheck]
        public decimal DateTotalsSum
        {
            get
            {
                return DateTotals.Sum(x => x.Hours);
            }
        }

        public bool ForecastMonthIsLocked { get; set; }

        private ForecastType _Project = new ForecastType
            {
                ColorStringHex = "#FF696969",
                Name = "Project",
                SupportsProjectHours = true,
                SupportsDedicatedHours = false
            };

        private ForecastType _training = new ForecastType
            {
                ColorStringHex = "#FFFFFF00",
                Name = "Training",
                SupportsProjectHours = true,
                SupportsDedicatedHours = true
            };

        private ForecastType _open = new ForecastType
            {
                ColorStringHex = "#FF008000",
                Name = "Open",
                SupportsProjectHours = true,
                SupportsDedicatedHours = true
            };

        private ForecastType _illness = new ForecastType
            {
                ColorStringHex = "#FF8B0000",
                Name = "Illness",
                SupportsProjectHours = false,
                SupportsDedicatedHours = true
            };

        private ForecastType _vacation = new ForecastType
            {
                ColorStringHex = "#FF9400D3",
                Name = "Vacation",
                SupportsProjectHours = false,
                SupportsDedicatedHours = false
            };

        public IEnumerable<HourRegistration> DateTotals
        {
            get
            {
                return CreateHourRegistrations();
            }
        }

        public IEnumerable<decimal?> DateRealizedTotals
        {
            get
            {
                return CreateHourRegistrations().Select(x => (decimal?) x.Hours);
            }
            set {}
        }

        public bool IsDirty { get; private set; }

        public IEnumerable<ProjectRegistration> RemovedProjects { get; private set; }

        public int ForecastMonthId { get; set; }

        public int ProjectForecastTypeId { get; private set; }
        public ForecastRegistrationSelectedUserHandler SelectedUserHandler
        {
            get
            {
                return new ForecastRegistrationSelectedUserHandler(new DesignUserSession(), null)
                    {
                        SelectedUser = new ForecastUserDto { Name = "Some guy", UserName = "sg", UserId = 1}
                    };
            }
        }

        public void Initialize()
        {
            //
        }

        public void CalculateTotals() { }
        public ProjectRegistration AddNewProjectRegistration(int projectId, string projectName, string companyName)
        {
            throw new NotImplementedException();
        }

        public void CalculateTotals(ForecastRegistrationDateColumn dateItem)
        {
            throw new NotImplementedException();
        }

        public void RefreshViewData() { }
        public void RaiseCanExecuteActions() { }
        public void RaisePresenceRegistrationsOnPropertyChanged() { }
        public void FetchForecasts() { }

        public void ApplyViewModel(IViewModel viewModel) { }
        public object DataContext { get; set; }

        public DateTime SelectedDate { get {return new DateTime(2013, 1, 1);} set {} }

        public DelegateCommand<object> PreviousMonthCommand { get; private set; }
        public DelegateCommand<object> NextMonthCommand { get; private set; }
        public DelegateCommand<object> CurrentMonthCommand { get; private set; }
        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> ResetCommand { get; private set; }
        public DelegateCommand<object> CopyPreviousMonth { get; private set; }        
        public IEnumerable<ForecastTypeRegistration> DirtyPresenceRegitrations { get; private set; }
        public IEnumerable<ProjectHourRegistration> DirtyProjectHours { get; private set; }

        public string SelectedMonthString { get { return string.Format("{0} {1}", SelectedDate.ToString("MMMM", CultureInfo.InvariantCulture), SelectedDate.Year);; } }

        public bool IsBusy { get { return false; } set {} }

        public int DatesCount { get { return 31; } set{} }

        public string SaveDisabledText
        {
            get { return "Some validation error"; }
            set {}
        }

        public class DesignUserSession : IUserSession
        {
            public User CurrentUser { get { return User.Create("", "", 99, null, null); } }
            public bool IsLoggedIn { get; private set; }
            public void Initialize()
            {

            }

            public void LoginUser(string username, string password, string customerId, bool persistPassword)
            {
            }

            public void LogOutUser()
            {
            }

            public IUserPreferences UserPreferences { get; private set; }
            public ILoginSettings LoginSettings { get; private set; }
            public UserStatistics UserStatistics { get; set; }
            public bool MayEditOthersWorksplan { get { return true; } }
            public void AttachUserNameAndCustomerId(string username, string customerId)
            {
            }
        }
    }


}