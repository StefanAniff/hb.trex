using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Project.ProjectAdministration;

namespace Trex.SmartClient.Project.DesignData
{
    public class DesignProjectAdministration : ViewModelDirtyHandlingBase, IProjectAdministrationViewModel
    {
        private const string DefaultInvoiceGroup = "Default invoicegroup";

        private readonly DummyObject _someCompany = new DummyObject { Id = 25, Name = "Company X"};
        private readonly DummyObject _selectedInvoiceGroup = new DummyObject {Id = 1, Name = DefaultInvoiceGroup};
        private readonly DummyObject _selectedProjectManager = new DummyObject {Id = 1, Name = "Anders Andersen"};
        private readonly DummyObject _selectedFileType = new DummyObject { Id = 1, Name = "Estimate" };

        public DelegateCommand<object> GotoProjectDispositionCommand { get; set; }

        public ObservableCollection<DummyObject> Projects
        {
            get
            {
                return new ObservableCollection<DummyObject>
                    {
                        new DummyObject { Id = 1, Name = "Optimize cashflow", Field1 = DefaultInvoiceGroup, Field2 = "Anders Andersen"},
                        new DummyObject { Id = 2, Name = "Resource optimization", Field1 = DefaultInvoiceGroup, Field2 = "Lone Pedersen"},
                        new DummyObject { Id = 3, Name = "Peptalk training", Field1 = DefaultInvoiceGroup, Field2 = "Peter Pedersen"}                        
                    };
            }
        }

        public DummyObject SelectedCompany { get { return _someCompany; } set { value = null; } }

        public ObservableCollection<DummyObject> AvailableCompanies
        {
            get { return new ObservableCollection<DummyObject> { _someCompany }; }
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