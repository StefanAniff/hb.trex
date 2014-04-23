using System.Windows.Controls;
using System.Windows.Input;
using Trex.Core.Interfaces;
using Trex.Invoices.Commands;
using Trex.Invoices.InvoiceManagementScreen.Interfaces;

namespace Trex.Invoices.InvoiceManagementScreen.FilterView
{
    public partial class FilterView : UserControl, IFilterView
    {
        public FilterView()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        private void AutoCompleteBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                InternalCommands.InvoiceIDSelected.Execute(null);
                //InvoiceIDSearch.Text = string.Empty;
            }
        }

        private void InvoiceIDSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Key.ToString(), "\\d+"))
                e.Handled = true;
        }
    }
}
