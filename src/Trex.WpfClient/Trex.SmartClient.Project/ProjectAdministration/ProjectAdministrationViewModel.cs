using System;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Project.TaskDisposition;
using System.Linq;

namespace Trex.SmartClient.Project.ProjectAdministration
{
    public class ProjectAdministrationViewModel : ViewModelDirtyHandlingBase, IProjectAdministrationViewModel
    {
        private readonly ICustomerService _customerService;
        private Company _someCompany;
        private ObservableCollection<Company> _availableCompanies;

        public DelegateCommand<object> GotoProjectDispositionCommand { get; private set; }

        public ProjectAdministrationViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            InitializeCommands();
        }

        public Company SelectedCompany
        {
            get
            {
                return _someCompany;
            } 
            set
            {
                _someCompany = value;
                OnPropertyChanged(() => SelectedCompany);
            }
        }

        public ObservableCollection<Company> AvailableCompanies
        {
            get { return _availableCompanies; }
            set
            {
                _availableCompanies = value;
                OnPropertyChanged(() => AvailableCompanies);
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
            FetchCustomers();
        }

        private async void FetchCustomers()
        {
            var result = await _customerService.GetAllActiveCustomers();
            AvailableCompanies = new ObservableCollection<Company>(result.Select(x => Company.Create(x.Name, x.Id, false, false)));
        }
    }
}