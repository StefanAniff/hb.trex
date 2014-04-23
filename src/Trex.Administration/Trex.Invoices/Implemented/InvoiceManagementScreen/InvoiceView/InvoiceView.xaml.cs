using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Trex.Core.Interfaces;
using Trex.Infrastructure.Commands;
using Trex.Invoices.Commands;
using Trex.Invoices.Dialogs.InvoiceComments;
using Trex.Invoices.InvoiceManagementScreen.Interfaces;

namespace Trex.Invoices.InvoiceManagementScreen.InvoiceView
{
    public partial class InvoiceView : UserControl, IInvoiceView
    {
        public InvoiceView()
        {
            InitializeComponent();
            InvoicesGrid.AddHandler(GridViewCell.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(OnDoubleClick), true);

            InvoicesGrid.RowLoaded += OnRowLoaded;

        }

        private void OnDoubleClick(object sender, RadRoutedEventArgs e)
        {
            var cell = e.OriginalSource as GridViewCell;

            if (cell != null && cell.Column.Header != null && cell.Column.Header.ToString() == "InvoiceID")
                InternalCommands.FinalizeInvoice.Execute(1);
            else if (cell != null && cell.Column.Header != null && cell.Column.Header.ToString() == "Comments")
            {
                var i = e.Source as GridViewCell;

                
                InternalCommands.SeeCommentsStart.Execute(i.ParentRow.Item);
            }
        }

        private void OnRowLoaded(object sender, RowLoadedEventArgs e)
        {
            if (!(e.Row is GridViewRow)) return;
            foreach (var cell in e.Row.Cells)
            {
                if ((string) cell.Column.Header == "Due Date")
                {
                    var g = (InvoiceListItemViewModel) cell.DataContext;
                    if(g.DueDate < DateTime.Now && g.Delivered)
                        cell.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
        }

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        private void InvoicesGridSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            var gridView = (RadGridView)sender;
            var tmp = gridView.SelectedItems;

            InternalCommands.UpdateSelectedInvoiceItems.Execute(tmp);
        }
    }
}
