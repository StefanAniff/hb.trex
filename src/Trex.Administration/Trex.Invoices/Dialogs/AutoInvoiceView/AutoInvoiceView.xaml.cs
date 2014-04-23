using System.Windows;
using System.Windows.Controls;

namespace Trex.Invoices.Dialogs.AutoInvoiceView
{

    public partial class AutoInvoiceView : ChildWindow
    {

        public AutoInvoiceView()
        {
            InitializeComponent();
        }


        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

    }

}

