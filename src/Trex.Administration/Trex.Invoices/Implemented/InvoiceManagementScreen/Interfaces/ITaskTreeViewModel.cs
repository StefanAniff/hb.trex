using System.Collections.ObjectModel;
using Trex.Core.Interfaces;
using Trex.Invoices.InvoiceManagementScreen.CustomerTreeView;

namespace Trex.Invoices.InvoiceManagementScreen.Interfaces
{
    public interface ITaskTreeViewModel : IViewModel
    {
        ObservableCollection<CustomerListItemViewModel> Customers { get; }
    }
}