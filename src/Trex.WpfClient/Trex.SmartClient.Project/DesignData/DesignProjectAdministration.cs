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

        public ObservableCollection<DummyObject> AvailableInvoiceGrouops {get { return new ObservableCollection<DummyObject> { _selectedInvoiceGroup };}} 
    }

    public class DummyObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}