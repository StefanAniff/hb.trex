using System.Windows.Controls;
using Trex.Core.Interfaces;

namespace Trex.Invoices.InvoiceManagementScreen.ButtonPanelView
{
    public partial class ButtonPanelView : UserControl, IView 
    {
        public ButtonPanelView()
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
