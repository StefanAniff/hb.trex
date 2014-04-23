using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Trex.Core.Interfaces;
using Trex.Invoices.Commands;

namespace Trex.Invoices.InvoiceTemplatesScreen.InvoiceTemplatesListView
{
    public partial class InvoiceTemplatesListView : UserControl, IView
    {
        public InvoiceTemplatesListView()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel)DataContext; }
            set { DataContext = value; }
        }
    }
}
