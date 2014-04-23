using System.Collections.ObjectModel;
using Trex.Core.Interfaces;
using Trex.Invoices.InvoiceManagementScreen.CustomerTreeView;

namespace Trex.Invoices.Interfaces
{
    public interface ICustomerListViewModel:IViewModel
    {
        ObservableCollection<CustomerListItemViewModel> Customers { get;}
    }
}