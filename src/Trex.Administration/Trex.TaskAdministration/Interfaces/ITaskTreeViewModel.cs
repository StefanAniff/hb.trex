using System.Collections.ObjectModel;
using Trex.Core.Interfaces;
using Trex.TaskAdministration.TaskManagementScreen.TaskTreeView;

namespace Trex.TaskAdministration.Interfaces
{
    public interface ITaskTreeViewModel : IViewModel
    {
        ObservableCollection<TreeCustomerViewModel> Customers { get; }
    }
}