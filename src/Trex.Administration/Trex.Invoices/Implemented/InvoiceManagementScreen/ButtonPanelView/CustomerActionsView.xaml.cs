using System.Windows.Controls;
using Trex.Core.Interfaces;

namespace Trex.Invoices.InvoiceManagementScreen.ButtonPanelView
{
    public partial class CustomerActionsView : UserControl, IView 
    {
        public CustomerActionsView()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }
    }
}
