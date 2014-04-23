using System;
using System.Windows.Controls;
using Trex.Core.Interfaces;
using Trex.Invoices.Commands;
using Trex.Invoices.InvoiceManagementScreen.InvoiceView;

namespace Trex.Invoices.InvoiceManagementScreen.ButtonPanelView
{
    public partial class InvoiceActionsView : IView
    {
        public InvoiceActionsView()
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
