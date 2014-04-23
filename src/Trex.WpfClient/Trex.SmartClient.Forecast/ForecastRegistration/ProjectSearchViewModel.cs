using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    public class ProjectSearchViewModel : ViewModelBase
    {
        private readonly IProjectRepository _projectRepository;
        private string _companyProjectSearchString;
        private ObservableCollection<Project> _searchResult;

        private Subject<ProjectSearchViewModel> _searchStringChanged;

        private Project _selectedProject;

        public DelegateCommand<object> ProjectSelectedCommand { get; private set; }

        public ProjectSearchViewModel(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            InitializeCommands();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            _searchStringChanged
                .Where(x => x.CompanyProjectSearchString != null)
                .Throttle(TimeSpan.FromMilliseconds(400))
                .ObserveOnDispatcher()
                .Subscribe(GetCompaniesByString);
        }

        private async void GetCompaniesByString(ProjectSearchViewModel x)
        {
            // Is null in designtime
            if (_projectRepository == null)
                return;

            // Empty search string. Clear search results
            if (string.IsNullOrEmpty(x.CompanyProjectSearchString))
            {
                x.SearchResult = new ObservableCollection<Project>();
                return;
            }

            var projects = _projectRepository.GetBySearchString(x.CompanyProjectSearchString);
            x.SearchResult = new ObservableCollection<Project>(projects);
        }

        private void InitializeCommands()
        {            
            ProjectSelectedCommand = new DelegateCommand<object>(ProjectSelectedExecute);            

            // Search throttling
            _searchStringChanged = new Subject<ProjectSearchViewModel>();            
        }        

        private void ProjectSelectedExecute(object obj)
        {
            if (SelectedProject == null)
                return;
            
            ForecastLocalCompositeCommands.ForecastRegistrationProjectSelected.Execute(SelectedProject);
            SearchResult = new ObservableCollection<Project>();
            SelectedProject = null;
            CompanyProjectSearchString = null;
            OnPropertyChanged(() => SearchResultsExists);
        }

        public string CompanyProjectSearchString
        {
            get { return _companyProjectSearchString; }
            set
            {
                _companyProjectSearchString = value;
                OnPropertyChanged(() => CompanyProjectSearchString);         
                _searchStringChanged.OnNext(this);
            }
        }

        public ObservableCollection<Project> SearchResult
        {
            get { return _searchResult; }
            set
            {
                _searchResult = value;
                OnPropertyChanged(() => SearchResult);
                OnPropertyChanged(() => SearchResultsExists);
            }
        }

        public bool SearchResultsExists 
        {
            get { return SearchResult != null && SearchResult.Count > 0; }
        }

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                OnPropertyChanged(() => SelectedProject);
            }
        }
    }
}