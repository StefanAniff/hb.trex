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
using Trex.Core.Interfaces;
using Trex.Invoices.Commands;

namespace Trex.Invoices.Dialogs.InvoiceComments
{
    public partial class InvoiceComments : ChildWindow, IView
    {
        public InvoiceComments()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            
            this.DialogResult = true;
        }

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        private void ChildWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                InternalCommands.SendComment.Execute(InvMsg.Text);
                this.DialogResult = true;
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

