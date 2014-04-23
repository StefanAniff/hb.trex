using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.Invoices.Commands;

namespace Trex.Invoices.Dialogs.EditInvoiceView
{
    public partial class EditInvoiceView : ChildWindow, IView
    {
        public EditInvoiceView()
        {
            InitializeComponent();

            InternalCommands.CloseAddEditInvoiceWindow.RegisterCommand(new DelegateCommand<bool?>(CloseWindow));
        }

        private void EscClose(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                this.DialogResult = false;
        }

        private void CloseWindow(bool? ok)
        {
            this.DialogResult = ok;
        }

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }
    }
}

