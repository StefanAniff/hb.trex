using System;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Project.TaskDisposition;

namespace Trex.SmartClient.Project.ProjectAdministration
{
    public class ProjectAdministrationViewModel : ViewModelDirtyHandlingBase, IProjectAdministrationViewModel
    {
        private readonly ICompanyRepository _companyRepository;
        private Company _someCompany;

        public DelegateCommand<object> GotoProjectDispositionCommand { get; private set; }

        public ProjectAdministrationViewModel(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
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

        public ObservableCollectionExtended<Company> AvailableCompanies
        {
            get { return new ObservableCollectionExtended<Company> { _someCompany }; }
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
            MessageBox.Show("Get stuff!!");
        }
    }
}