using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.SmartClient.Core.Implemented;
using System.Linq;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Forecast.ForecastOverview.Helpers;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast.ForecastOverview
{
    public class ForecastOverviewSearchOptions : ViewModelBase
    {
        private readonly IForecastService _forecastService;
        private readonly ForecastOverviewUserSearchOptions _userSearchOptions;
        private ObservableCollection<Project> _projects = new ObservableCollection<Project>();
        private Project _selectedProject;
        private ObservableCollection<ForecastType> _forecastTypes = new ObservableCollection<ForecastType>();
        private ForecastType _selectedForecastType;
        private ObservableCollection<Company> _companies = new ObservableCollection<Company>();
        private Company _selectedCompany;
        private int _selectedTabIndex;

        public ForecastOverviewSearchOptions(IForecastService forecastService, ForecastOverviewUserSearchOptions userSearchOptions)
        {
            _forecastService = forecastService;
            _userSearchOptions = userSearchOptions;
        }

        #region Users

        public ForecastOverviewUserSearchOptions UserSearchOptions
        {
            get { return _userSearchOptions; }
        }

        public ObservableCollection<ForecastUserDto> Users
        {
            get { return UserSearchOptions.Users; }
            set
            {
                UserSearchOptions.Users = value;
                OnPropertyChanged(() => Users);
            }
        }

        public ObservableCollection<ForecastUserDto> SelectedUsers
        {
            get { return UserSearchOptions.SelectedUsers; }
            set
            {
                UserSearchOptions.SelectedUsers = value;
                OnPropertyChanged(() => SelectedUsers);
            }
        }

        public void InitializeUsers(IEnumerable<ForecastUserDto> newUsers)
        {
            UserSearchOptions.InitializeUsers(newUsers);
        }

        #endregion

        #region Projects

        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
                OnPropertyChanged(() => Projects);
            }
        }

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                OnPropertyChanged(() => SelectedProject);

                // Project and company search cannot be combined. Reset other if value is entered
                if (SelectedProject != null && SelectedCompany != null)
                    SelectedCompany = null;
            }
        }

        public int? SelectedProjectId { get { return SelectedProject != null ? SelectedProject.Id : (int?) null; } }

        public void InitializeProjects(IEnumerable<Project> newProjects)
        {
            Projects.Clear();
            foreach (var newProject in newProjects.OrderBy(x => x.Name))
            {
                Projects.Add(newProject);
            }
        }

        #endregion

        #region Status/forecast type

        public ObservableCollection<ForecastType> ForecastTypes
        {
            get { return _forecastTypes; }
            set
            {
                _forecastTypes = value;
                OnPropertyChanged(() => ForecastTypes);
            }
        }

        public ForecastType SelectedForecastType
        {
            get { return _selectedForecastType; }
            set
            {
                _selectedForecastType = value;
                OnPropertyChanged(() => SelectedForecastType);
            }
        }

        public int? SelectedForecastTypeId
        {
            get
            {
                return _selectedForecastType != null && !(_selectedForecastType is EmptyForecastType)
                        ? SelectedForecastType.Id 
                        : (int?) null;
            }
        }

        public void InitializeForecastTypes(IEnumerable<ForecastType> newForecastTypes)
        {
            ForecastTypes.Clear();
            ForecastTypes.Add(new EmptyForecastType());
            foreach (var newForecastType in newForecastTypes)
            {
                ForecastTypes.Add(newForecastType);
            }
        }

        #endregion

        #region Companies

        public ObservableCollection<Company> Companies
        {
            get { return _companies; }
            set
            {
                _companies = value;
                OnPropertyChanged(() => Companies);
            }
        }

        public Company SelectedCompany
        {
            get { return _selectedCompany; }
            set
            {
                _selectedCompany = value;
                OnPropertyChanged(() => SelectedCompany);

                // Project and company search cannot be combined. Reset other if value is entered
                if (SelectedCompany != null && SelectedProject != null)
                    SelectedProject = null;
            }
        }

        public void InitializeCompanies(IEnumerable<Company> newCompanies)
        {
            Companies.Clear();
            foreach (var newCompany in newCompanies)
            {
                Companies.Add(newCompany);
            }
        }

        public int? SelectedCompanyId
        {
            get { return SelectedCompany != null ? _selectedCompany.Id : (int?) null; }
        }

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(() => SelectedTabIndex);
            }
        }

        public bool SelectedTabIsSearchByRegistration
        {
            get { return SelectedTabIndex == 0; }
        }

        #endregion        
        
        public async Task<ForecastSearchResponse> DoSearch(int month, int year)
        {
            ForecastSearchResponse response = null;

            // Search by users
            if (SelectedTabIndex == ForecastOverviewViewSetup.SearchByUserTabIndex)
            {
                var requestedUsers = GetUsersToSearchFor().ToList();
                response = await _forecastService.GetBySearch(month, year, requestedUsers);

                if (response != null)
                {
                    TryAddMissingUsers(month, year, response);
                }
            }

            // Search by registration
            if (SelectedTabIndex == ForecastOverviewViewSetup.SearchByRegistrationTabIndex)
            {
                response = await _forecastService.GetBySearch(month
                                                            , year
                                                            , SelectedProjectId
                                                            , SelectedCompanyId
                                                            , SelectedForecastTypeId);
            }            

            return response;
        }

        private IEnumerable<int> GetUsersToSearchFor()
        {
            var allUsersMarker = SelectedUsers.FirstOrDefault(x => x.IsAllUsers);

            // If allUsersMarker, get all users
            return allUsersMarker != null 
                ? Users.Where(x => !x.IsAllUsers).Select(x => x.UserId) 
                : SelectedUsers.Select(x => x.UserId);
        }

        /// <summary>
        /// If user does not have a forecastmonth registration
        /// the server will not return anytning.
        /// Generate "dummy" line, so missing registrations are visible
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="response"></param>
        public virtual void TryAddMissingUsers(int month, int year, ForecastSearchResponse response)
        {
            var responsedUsersIds = response.ForecastMonths.Select(x => x.UserId).ToList();
            var requestedUserIds = SelectedUsers.Where(x => !x.IsAllUsers).Select(x => x.UserId).ToList();
            var userEmptyResult = requestedUserIds.Where(x => responsedUsersIds.All(y => y != x));
            foreach (
                var user in
                    userEmptyResult.Select(userId => SelectedUsers.SingleOrDefault(x => x.UserId == userId)) // Select the local user-object
                                   .Where(user => user != null))
            {
                response.ForecastMonths.Add(new ForecastMonthDto
                    {
                        UserId = user.UserId,
                        UserName = user.Name,
                        CreatedById = 0,
                        Id = 0,
                        IsLocked = false,
                        Month = month,
                        Year = year,
                        ForecastDtos = new Collection<ForecastDto>()
                    });
            }
        }

        public void Reset()
        {
            SelectedForecastType = null;
            SelectedProject = null;
            SelectedCompany = null;
            SelectedUsers.Clear();
        }

        public class EmptyForecastType : ForecastType
        {
            public EmptyForecastType()
            {
                ColorStringHex = "transparent";
                Name = string.Empty;
            }
        }

        #region Result filtering

        /// <summary>
        /// Applies the display handler on forecasts depending on 
        /// their forecasttype.
        /// </summary>
        /// <param name="usrMonth"></param>
        /// <param name="projectForecastTypeId"></param>
        public virtual void SetupVisualsBySearchOption(ForecastOverviewForecastMonth usrMonth, int projectForecastTypeId)
        {
            foreach (var forecast in usrMonth.Forecasts)
            {
                // Current forecast is NOT a workday. Give it an empty display handler
                if (!forecast.IsWorkDay || forecast.ForecastType == null)
                {
                    forecast.DisplayHandler = new EmptyDisplayHandler(forecast);
                    continue;
                }

                // ForecastType is "Project"
                if (forecast.ForecastType.Id.Equals(projectForecastTypeId))
                {
                    forecast.DisplayHandler = new PureProjectTypeDisplayHandler(forecast,
                                                                                SelectedTabIsSearchByRegistration ? SelectedProjectId : null,
                                                                                SelectedTabIsSearchByRegistration ? SelectedCompanyId : null);
                    continue;                    
                }


                // ForecastType supports project registrations
                if (forecast.ForecastType.SupportsProjectHours)
                {
                    // Has project/company search focus
                    if ((SelectedProjectId.HasValue || SelectedCompanyId.HasValue) && SelectedTabIsSearchByRegistration)
                        forecast.DisplayHandler = new SupportsProjectsWithFocusDisplayHandler(forecast, SelectedProjectId, SelectedCompanyId, projectForecastTypeId);

                    // No project/company search focus
                    else
                        forecast.DisplayHandler = new SupportsProjectsNoFocusDisplayHandler(forecast, projectForecastTypeId);

                    continue;
                }

                // ForecastType is neither "Project" or supporting project registrations
                forecast.DisplayHandler = new NonProjectSupportingDisplayHandler(forecast);        
            }
        }

        #endregion

    }
}