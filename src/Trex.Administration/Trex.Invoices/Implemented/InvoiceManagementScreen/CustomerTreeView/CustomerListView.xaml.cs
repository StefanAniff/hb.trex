using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.Invoices.Commands;
using Trex.Invoices.InvoiceManagementScreen.Interfaces;
using Trex.ServiceContracts;

namespace Trex.Invoices.InvoiceManagementScreen.CustomerTreeView
{
    public partial class CustomerListView : UserControl, ICustomerListView
    {
        public CustomerListView()
        {
            InitializeComponent();
            InternalCommands.ReselectedCustomer.RegisterCommand(new DelegateCommand<ObservableCollection<CustomerListItemViewModel>>(ReselectedCustomer));
            
        }

        private int _i = 0;
        private void ReselectedCustomer(ObservableCollection<CustomerListItemViewModel> clivm)
        {
            _i = 0;
            if(clivm.Count != 0)
                CustomerList.SelectionChanged -= SelectionChanged;
            foreach (var item in CustomerList.Items.Cast<CustomerListItemViewModel>())
            {
                foreach (var customerListItemViewModel in clivm)
                {
                    if (item.Customer.CustomerID == customerListItemViewModel.Customer.CustomerID)
                    {
                        _i++; 
                        if (_i >= clivm.Count)
                        {
                            CustomerList.SelectionChanged += SelectionChanged;
                            CustomerList.SelectedItems.Add(item);
                        }
                        else
                        {
                            CustomerList.SelectionChanged -= SelectionChanged;
                            CustomerList.SelectedItems.Add(item);
                        }
                    }
                }
            }
        }

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        private void TextBlockMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Clipboard.SetText(((TextBlock)sender).Text);
            }
            catch (Exception)
            {

            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = (ListBox)sender;
            var temp = new ObservableCollection<CustomersInvoiceView>();
            foreach (var customerListItemViewModel in list.SelectedItems.Cast<CustomerListItemViewModel>())
            {
                temp.Add(customerListItemViewModel.Customer);
            }
            if (temp.Count > 0)
            {
                InternalCommands.ResetSelectedInvoices.Execute(null); //Remove all TimeEntries and InvoiceLines
                InternalCommands.GetInvoicesFromMany.Execute(temp); //Load what must be loaded
            }
        }
    }
}
