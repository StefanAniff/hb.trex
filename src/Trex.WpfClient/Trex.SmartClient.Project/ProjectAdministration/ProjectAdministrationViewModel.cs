using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Project.CommonViewModels;
using Trex.SmartClient.Project.TaskDisposition;
using System.Linq;

namespace Trex.SmartClient.Project.ProjectAdministration
{
    public class ProjectAdministrationViewModel : ViewModelDirtyHandlingBase, IProjectAdministrationViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IProjectService _projectService;
        private CompanyViewModel _selectedCompany;
        private ObservableCollection<CompanyViewModel> _availableCompanies;
        private bool _isBusy;
        private ProjectViewModel _selectedProject;

        public DelegateCommand<object> GotoProjectDispositionCommand { get; private set; }

        public ProjectAdministrationViewModel(ICustomerService customerService
            , IProjectService projectService)
        {
            _customerService = customerService;
            _projectService = projectService;
            InitializeCommands();
        }

        public CompanyViewModel SelectedCompany
        {
            get
            {
                return _selectedCompany;
            } 
            set
            {
                _selectedCompany = value;
                OnPropertyChanged(() => SelectedCompany);
                FetchProjects(_selectedCompany);
            }
        }

        public ObservableCollection<CompanyViewModel> AvailableCompanies
        {
            get { return _availableCompanies; }
            set
            {
                _availableCompanies = value;
                OnPropertyChanged(() => AvailableCompanies);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
            }
        }

        public ProjectViewModel SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                OnPropertyChanged(() => SelectedProject);
            }
        }

        #region Commandhandlers

        private void InitializeCommands()
        {
            GotoProjectDispositionCommand = new DelegateCommand<object>(GotoProjectDispositionExecute,
                                                                        CanExecuteGotoProjectDisposition);
        }

        private bool CanExecuteGotoProjectDisposition(object arg)
        {
            // IVA: Do me
            return true;
        }

        private void GotoProjectDispositionExecute(object obj)
        {
            ApplicationCommands.JumpToSubmenuCommand.Execute(new JumpToSubmenuParam { ViewType = typeof(TaskDispositionView), InitializationParam = null });
        }

        #endregion

        public void Initialize()
        {
            IsBusy = true;
            FetchCustomers();
        }

        private async void FetchCustomers()
        {
            var result = await _customerService.GetAllActiveCustomers();
            AvailableCompanies = new ObservableCollection<CompanyViewModel>(result.Select(x => CompanyViewModel.Create(x.Name, x.Id, x.Inactive)));
            IsBusy = false;
        }

        private async void FetchProjects(CompanyViewModel company)
        {
            if (company == null)
                return;

            IsBusy = true;

            var result = await _projectService.GetProjects(company.Id);

            IsBusy = false;
        }
    }
}