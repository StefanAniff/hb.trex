using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Project.CommonViewModels;
using Trex.SmartClient.Project.ProjectAdministration;

namespace Trex.SmartClient.Project.DesignData
{
    public class DesignProjectAdministration : ViewModelDirtyHandlingBase, IProjectAdministrationViewModel
    {
        private const string DefaultInvoiceGroup = "Default invoicegroup";

        private readonly CompanyViewModel _selectedCompany = CompanyViewModel.Create("Company X", 25, false);
        private readonly DummyObject _selectedInvoiceGroup = new DummyObject {Id = 1, Name = DefaultInvoiceGroup};
        private readonly DummyObject _selectedProjectManager = new DummyObject {Id = 1, Name = "Anders Andersen"};
        private readonly DummyObject _selectedFileType = new DummyObject { Id = 1, Name = "Estimate" };

        private readonly UserViewModel _projectManager = new UserViewModel {Id = 1, Name = "IVA"};

        private readonly ProjectViewModel _selectedProject;

        public DesignProjectAdministration()
        {
            _selectedProject = new ProjectViewModel
                {
                    Id = 1, 
                    Name = "Optimize cashflow", 
                    Inactive = true, 
                    IsFixedPrice = true, 
                    IsInternal = true , 
                    Manager = _projectManager,
                    AvailableProjectManagers = new List<UserViewModel> { _projectManager }
                };
            _selectedCompany = new CompanyViewModel
                {
                    Id = 25,
                    Name = "Company X",
                    Inactive = false,                    
                    Projects = new ObservableCollectionExtended<ProjectViewModel>
                        {
                            _selectedProject,
                            new ProjectViewModel { Id = 2, Name = "Resource optimization", Manager = _projectManager },
                            new ProjectViewModel { Id = 1, Name = "Peptalk training", Manager = _projectManager }
                        }
                };
        }

        public DelegateCommand<object> GotoProjectDispositionCommand { get; set; }        

        public CompanyViewModel SelectedCompany { get { return _selectedCompany; } set { value = null; } }

        public ObservableCollection<CompanyViewModel> AvailableCompanies
        {
            get { return new ObservableCollection<CompanyViewModel> { _selectedCompany }; }
            set {}
        }


        public DummyObject SelectedInvoiceGroup { get { return _selectedInvoiceGroup; } set { value = null; } }
        public ObservableCollection<DummyObject> AvailableInvoiceGroups { get { return new ObservableCollection<DummyObject> { _selectedInvoiceGroup }; } }

        public DummyObject SelectedProjectManager { get { return _selectedProjectManager; } set { value = null; } }
        public ObservableCollection<DummyObject> AvailableProjectManagers {get { return new ObservableCollection<DummyObject> { _selectedProjectManager };}}

        public DummyObject SelectedFileType { get { return _selectedFileType; } set { value = null; } }
        public ObservableCollection<DummyObject> AvailableFileTypes { get { return new ObservableCollection<DummyObject> { _selectedFileType }; } }

        public ObservableCollection<DummyObject> Files
        {
            get
            {
                return new ObservableCollection<DummyObject>
                    {
                        new DummyObject { Id = 1, Name = "Project x forkalk", Field1 = "Estimate", Field2 = "forkalk.xlsx"},
                        new DummyObject { Id = 2, Name = "Project x noter", Field1 = "Assorted", Field2 = "Initiel møde - noter.doc"}
                    };
            }
        }

        public bool IsBusy { get; set; }

        public ProjectViewModel SelectedProject { get { return _selectedProject; } set {} }

        public void Initialize() { }
    }

    public class DummyObject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
    }
}