using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Presentation.Commands;
using Telerik.Windows.Controls;
using Trex.Invoices.Commands;
using Trex.Invoices.Implemented;
using Trex.Invoices.InvoiceManagementScreen.Interfaces;
using Trex.ServiceContracts;
using ViewModelBase = Trex.Core.Implemented.ViewModelBase;

namespace Trex.Invoices.InvoiceManagementScreen.FilterView
{
    public class FilterViewModel : ViewModelBase, IFilterViewModel,IDisposable
    {
        public DelegateCommand<object> ApplyFilterCommand { get; set; }
        public DelegateCommand<object> ResetFilterCommand { get; set; }

        public DelegateCommand<object> GenerateInvoiceCommand { get; set; }
        private DelegateCommand<List<Customer>> _customerSelected;

        private DateTime? _endDate;
        private bool _isShowAll;

        public FilterViewModel()
        {
            ApplyFilterCommand = new DelegateCommand<object>(ExecuteApplyFilter);
            ResetFilterCommand = new DelegateCommand<object>(ExecuteResetFilter);
            GenerateInvoiceCommand = new DelegateCommand<object>(ExecuteGenerateInvoice);
            _customerSelected = new DelegateCommand<List<Customer>> (CustomerSelected);
            InternalCommands.CustomerSelected.RegisterCommand(_customerSelected);

            EndDate = DateTime.Now;
        }

        private void CustomerSelected(List<Customer> obj)
        {
            SelectedCustomers = obj;
        }

        private void ExecuteResetFilter(object obj)
        {
            InternalCommands.ApplyFilter.Execute(null);
        }

        private void ExecuteApplyFilter(object obj)
        {
            InternalCommands.ApplyFilter.Execute(GetFilter());
        }

        //!The! button to generate invoices
        private void ExecuteGenerateInvoice(object obj)
        {
           // InternalCommands.ApplyFilter.Execute(GetCustomerIDs(), );
        }

        //TODO: Subscribe to 'Show Invoices' mangler
        public IEnumerable<Customer> SelectedCustomers { set; get; }

        private List<int> GetCustomerIDs()
        {
            var tmpList = new List<int>();

            foreach (var sc in SelectedCustomers)
            {
                tmpList.Add(sc.CustomerID);
            }

            return tmpList;
        }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        public bool ShowAll
        {
            get { return _isShowAll; }
            set
            {
                _isShowAll = value;
                OnPropertyChanged("IsShowAll");
            }
        }

        private Filter GetFilter()
        {
            return new Filter() { EndDate = EndDate, ShowAll = ShowAll };
        }

        //Forlad view kræver dette
        public void Dispose()
        {
            InternalCommands.CustomerSelected.UnregisterCommand(_customerSelected);
        }
    }
}