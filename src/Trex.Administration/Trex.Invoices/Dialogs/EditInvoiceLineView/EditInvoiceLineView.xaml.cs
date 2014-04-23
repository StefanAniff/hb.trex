using System.Windows;
using System.Windows.Controls;
using Trex.Core.Interfaces;

namespace Trex.Invoices.Dialogs.EditInvoiceLineView
{
    public partial class EditInvoiceLineView : ChildWindow, IView
    {
        public EditInvoiceLineView()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }
    }
}

