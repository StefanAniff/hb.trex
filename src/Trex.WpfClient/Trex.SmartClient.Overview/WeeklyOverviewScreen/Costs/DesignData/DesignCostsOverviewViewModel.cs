using System.Collections.ObjectModel;
using Trex.SmartClient.Core.Implemented;

namespace Trex.SmartClient.Overview.WeeklyOverviewScreen.Costs.DesignData
{
    public class DesignCostsOverviewViewModel : ViewModelDirtyHandlingBase
    {
        public ObservableCollection<DummyObject> Costs
        {
            get
            {
                return new ObservableCollection<DummyObject>
                    {
                        new DummyObject { Name = "Type (kr)", Field1 = "Parkering Århus midtby", Field2 = "100"},
                        new DummyObject { Name = "Type (notepad)", Field1 = "Kursus hos d60", Field2 = "8"},
                        new DummyObject { Name = "Km", Field1 = "Århus - Horsens. Møde med klient X", Field2 = "96"}
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