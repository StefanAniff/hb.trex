using System.Collections.ObjectModel;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;
using TempProject = Trex.SmartClient.Core.Model.Project;

namespace Trex.SmartClient.Project.DesignData
{
    public class DesignProjectAdministration : ViewModelDirtyHandlingBase
    {
        private readonly Company _someCompany = Company.Create("Company X", 25, false, false);
        private readonly DummyObject _selectedInvoiceGroup = new DummyObject {Id = 1, Name = "Default invoicegroup"};
        private readonly DummyObject _selectedProjectManager = new DummyObject {Id = 1, Name = "Anders Andersen"};
        private readonly DummyObject _selectedFileType = new DummyObject { Id = 1, Name = "Estimation" };

        public ObservableCollection<TempProject> Projects
        {
            get
            {
                return new ObservableCollection<TempProject>
                    {
                        TempProject.Create(1, "Optimize cashflow", _someCompany, false),
                        TempProject.Create(2, "Resource optimization", _someCompany, false),
                        TempProject.Create(2, "Peptalk training", _someCompany, false),
                    };
            }
        }

        public Company SelectedCompany { get { return _someCompany; } set { value = null; } }

        public ObservableCollection<Company> AvailableCompanies
        {
            get { return new ObservableCollection<Company> { _someCompany }; }
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
                        new DummyObject { Id = 1, Name = "Project x forkalk", Field1 = "Estimation", Field2 = "forkalk.xlsx"},
                        new DummyObject { Id = 2, Name = "Project x noter", Field1 = "Diverse", Field2 = "Initiel møde - noter.doc"}
                    };
            }
        }
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