﻿#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.Infrastructure.Implemented;
using Trex.Invoices.Commands;
using Trex.Invoices.Implemented;
using Trex.Invoices.Interfaces;
using Trex.ServiceContracts;
using DelegateCommand = Telerik.Windows.Controls.DelegateCommand;
using ViewModelBase = Trex.Core.Implemented.ViewModelBase;

#endregion

namespace Trex.Invoices.InvoiceManagementScreen.CustomerTreeView
{
    public class CustomerListViewModel : ViewModelBase, ICustomerListViewModel
    {
        private readonly IDataService _dataService;
        private readonly IUserSession _userSession;
        private ObservableCollection<CustomerListItemViewModel> _customers;
        private Filter _filter;
        private CustomerListItemViewModel _selectedCustomerListItem;
        private bool _showAllCustomers;
        private DateTime? _startDate;
        private DateTime? _endDate;

        private AutoCompleteFilterPredicate<object> _invoiceIDFilter;

        public DelegateCommand<object> ApplyFilterCommand { get; set; }
        public DelegateCommand<object> ResetFilterCommand { get; set; }
        public DelegateCommand<object> AutoGenerateCommand { get; set; }
        public DelegateCommand<object> FinalizeDraftCommand { get; set; }

        private InvoiceListItemView SelectedInvoice { set; get; }

        public CustomerListViewModel(IDataService dataService, IUserSession userSession)
        {
            _badlist = new List<ServerResponse>();
            _dataService = dataService;
            _userSession = userSession;
            Customers = new ObservableCollection<CustomerListItemViewModel>();
            SelectedCustomerListItems = new ObservableCollection<CustomerListItemViewModel>();

            SelectedCustomerListItems.CollectionChanged += SelectedCustomerListItemsChanged_EventHandler;

            _filter = new Filter();
            InternalCommands.InvoiceSelected.RegisterCommand(new DelegateCommand<InvoiceListItemView>(RefreshInvoice));
            InternalCommands.RefreshCustomers.RegisterCommand(new DelegateCommand(RefreshCustomers));
            InternalCommands.UpdateInvoiceList.RegisterCommand(new DelegateCommand(UpdateInvoiceListFromSelectedCustomer));
            InternalCommands.InvoiceIDSelected.RegisterCommand(new DelegateCommand(InvoiceIDSelected));
            InternalCommands.ReloadInvoiceSearchFilter.RegisterCommand(new DelegateCommand(LoadAllInvoices));

            ApplyFilterCommand = new DelegateCommand<object>(ExecuteApplyFilter);
            ResetFilterCommand = new DelegateCommand<object>(ExecuteResetFilter);
            AutoGenerateCommand = new DelegateCommand<object>(ExecuteAutoGenerate, CanExecuteAutoGenerate);
            FinalizeDraftCommand = new DelegateCommand<object>(ExecuteFinalizeDraftCommand,
                                                               CanExecuteFinalizeDraftCommand);

            //LoadCustomerViews();
            InternalCommands.ReloadInvoiceSearchFilter.Execute(null);


            EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
            SelectedInvoice = new InvoiceListItemView();
        }

        private void InvoiceIDSelected(object obj)
        {
            SeeDebitorlist = false;
            InternalCommands.FindInvoiceFromID.Execute(SelectedInvoiceID);
        }

        public void LoadAllInvoices(object obj)
        {
            _dataService.GetAllInvoiceIDs().Subscribe
                (
                    r =>
                    {
                        InvoiceID = r;
                    }
                );
        }

        public AutoCompleteFilterPredicate<object> InvoiceIDFilter
        {
            get { return _invoiceIDFilter; }
            set
            {
                _invoiceIDFilter = value;
                OnPropertyChanged("InvoiceIDFilter");
            }
        }

        private ObservableCollection<CustomerListItemViewModel> _selectedCustomerListItems;

        public ObservableCollection<CustomerListItemViewModel> SelectedCustomerListItems
        {
            get
            {
                return _selectedCustomerListItems;
            }
            set
            {
                if (value.Count > 2)
                {
                    StartDate = null;
                    OnPropertyChanged("StartDate");
                }
                _selectedCustomerListItems = value;
                OnPropertyChanged("SelectedCustomerListItems");
            }
        }

        public ObservableCollection<CustomerListItemViewModel> Customers
        {
            get { return _customers; }
            private set
            {
                _customers = value;
                //TotalInventoryValue = Customers.Sum(tcv => tcv.Customer.InventoryValue);
                OnPropertyChanged("Customers");
                OnPropertyChanged("TotalInventoryValue");
            }
        }

        public CustomerListItemViewModel SelectedCustomerListItem
        {
            get
            {
                return _selectedCustomerListItem;
            }
            set
            {

                //_seeDebitorlist = false;
                OnPropertyChanged("SeeDebitorlist");

                _selectedCustomerListItem = value;
                if (value != null)
                {
                    InternalCommands.CustomerSelected.Execute(value.Customer);
                }
                AutoGenerateCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedCustomer");
            }
        }

        private ObservableCollection<CustomerListItemViewModel> _temp;
        public void SelectedCustomerListItemsChanged_EventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            _temp = new ObservableCollection<CustomerListItemViewModel>();
            _temp = sender as ObservableCollection<CustomerListItemViewModel>;
        }

        private void UpdateInvoiceListFromSelectedCustomer(object obj)
        {
            if (SeeDebitorlist)
                return;
            if (SelectedCustomerListItem != null)
                _lastSelectedCustomerID = SelectedCustomerListItem.Customer.CustomerID;

            var temp = new ObservableCollection<CustomersInvoiceView>();
            foreach (var customerListItemViewModel in SelectedCustomerListItems)
            {
                temp.Add(customerListItemViewModel.Customer);
            }

            if (temp.Count > 0)
            {
                InternalCommands.GetInvoicesFromMany.Execute(temp);

                AutoGenerateCommand.RaiseCanExecuteChanged();
            }
            else
            {
                if (SelectedCustomerListItem != null)
                {
                    var tmp = new ObservableCollection<CustomersInvoiceView>();
                    tmp.Add(SelectedCustomerListItem.Customer);

                    InternalCommands.GetInvoicesFromMany.Execute(tmp);
                }
            }
        }

        public ObservableCollection<int?> InvoiceID
        {
            get { return _invoiceID; }
            set
            {
                _invoiceID = value;
                OnPropertyChanged("InvoiceID");
            }
        }

        public int? SelectedInvoiceID
        {
            get
            {
                return _selectedInvoiceID;
            }
            set
            {
                _selectedInvoiceID = value;
                OnPropertyChanged("SelectedInvoiceID");
            }
        }

        public string TotalInventoryValue
        {
            get
            {
                if (Customers != null)
                    return Customers.Where(c => c.Internal == false).Sum(c => c.Customer.InventoryValue).Value.ToString("N2");
                return "0";
            }
        }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                if (value < StartDate)
                {
                    _endDate = null;
                    OnPropertyChanged("EndDate");
                    AutoGenerateCommand.RaiseCanExecuteChanged();
                    throw new ValidationException("End date must have a value, bigger then start date");
                }

                _endDate = value;
                InternalCommands.RefreshCustomers.Execute(null);
                AutoGenerateCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("StartDate");
                OnPropertyChanged("EndDate");
            }
        }

        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                if (value > EndDate)
                {
                    _startDate = value;
                    OnPropertyChanged("StartDate");
                    AutoGenerateCommand.RaiseCanExecuteChanged();
                    throw new ValidationException("Start date must be smaller then end date");
                }
                _startDate = value;
                InternalCommands.RefreshCustomers.Execute(null);
                AutoGenerateCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("EndDate");
                OnPropertyChanged("StartDate");
            }
        }

        private bool _seeAllCustomers;
        public bool SeeAllCustomers
        {
            get
            {
                return _seeAllCustomers;
            }
            set
            {
                _seeAllCustomers = value;
                OnPropertyChanged("SeeAllCustomers");
                LoadCustomerViews();
            }
        }

        private bool _seeClosedInvoices;
        public bool SeeClosedInvoices
        {
            get { return _seeClosedInvoices; }
            set
            {
                if (_seeClosedInvoices == value)
                    return;
                _seeClosedInvoices = value;
                if (value)
                    SeeDebitorlist = false;
                InternalCommands.SeeAllInvoices.Execute(value);
                UpdateInvoiceListFromSelectedCustomer(null);
                OnPropertyChanged("SeeClosedInvoices");
            }
        }

        private bool _seeDebitorlist;
        public bool SeeDebitorlist
        {
            get { return _seeDebitorlist; }
            set
            {
                if (_seeDebitorlist == value)
                    return;
                LoadCustomerViews();

                _seeDebitorlist = value;
                if (value)
                    SeeClosedInvoices = false;
                InternalCommands.SeeDebitList.Execute(value);

                OnPropertyChanged("SeeDebitorlist");
            }
        }

        public bool ShowAllCustomers
        {
            get { return _showAllCustomers; }
            set
            {
                _showAllCustomers = value;
                OnPropertyChanged("ShowAllCustomers");
            }
        }

        private bool CanExecuteAutoGenerate(object obj)
        {
            if (SelectedCustomerListItem != null && EndDate.HasValue && (StartDate <= EndDate || StartDate == null))
            {
                return true;
            }
            return false;
        }

        private int _lastSelectedCustomerID;
        private void ExecuteAutoGenerate(object obj)
        {
            if (SelectedCustomerListItems.Count != 0)
            {
                ApplicationCommands.SystemBusy.Execute("Generating invoices");
                _numOfProcessedInvoices = 0;
                _lastSelectedCustomerID = SelectedCustomerListItem.Customer.CustomerID;
                GenerateInvoices();
            }
        }

        private void ExecuteFinalizeDraftCommand(object obj)
        {
            try
            {
                InternalCommands.FinalizeInvoice.Execute(0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CanExecuteFinalizeDraftCommand(object obj)
        {
            if (SelectedInvoice.InvoiceID == null && SelectedInvoice.ID != 0)
                return true;
            return false;
        }

        private void GenerateInvoices()
        {
            var selectedCustomerIds = SelectedCustomerListItems.Where(pr => pr.InventoryValue != "0").Select(s => s.Customer.CustomerID);

            if (selectedCustomerIds.Any())
                Execute.InBackground(() =>
                {
                    foreach (var id in selectedCustomerIds)
                    {
                        var startdate = SelectedCustomerListItems.Where(i => id == i.Customer.CustomerID).Select(d => d.FirstTimeEntryDate).First();

                        if (EndDate.HasValue && StartDate.HasValue)
                            _dataService.GenerateInvoice(StartDate.Value, EndDate.Value, id, _userSession.CurrentUser.UserID, (float)0.25)
                                .Subscribe(UpdateProgress);

                        else if (StartDate == null && EndDate.HasValue)
                            _dataService.GenerateInvoice((DateTime)startdate, EndDate.Value, id, _userSession.CurrentUser.UserID, (float)0.25)
                                .Subscribe(UpdateProgress);

                        else
                        {
                            Execute.InUIThread(() => MessageBox.Show("Failed to generate invoices", "Fail!", MessageBoxButton.OK));
                            InternalCommands.UpdateInvoiceList.Execute(null);
                        }
                    }
                });
            else
                ApplicationCommands.SystemIdle.Execute(null);
        }

        private int _numOfProcessedInvoices;

        readonly List<ServerResponse> _badlist = new List<ServerResponse>();
        private void UpdateProgress(ServerResponse response)
        {
            if (!response.Success)
                _badlist.Add(response);
            var numOfCustomers = SelectedCustomerListItems.Where(pr => pr.InventoryValue != "0").Select(s => s.Customer.CustomerID).Count();
            _numOfProcessedInvoices++;
            if (numOfCustomers <= _numOfProcessedInvoices)
            {
                LoadCustomerViews();
                ApplicationCommands.SystemIdle.Execute(null);
                if (_badlist.Count > 0)
                    MessageBox.Show(string.Join("\r\n", _badlist.Where(s => s.Success == false).Select(r => r.Response)));
                _badlist.Clear();
            }
            else
                ApplicationCommands.SystemBusy.Execute("Processed: " + _numOfProcessedInvoices + "/" + numOfCustomers);
        }

        private void ExecuteResetFilter(object obj)
        {
            _filter = new Filter();
            LoadCustomerViews();
        }

        private void ExecuteApplyFilter(object obj)
        {
            _filter = new Filter { EndDate = EndDate, ShowAll = ShowAllCustomers };
            LoadCustomerViews();
        }

        private ObservableCollection<CustomerListItemViewModel> _selecttemp;
        private ObservableCollection<int?> _invoiceID;
        private int? _selectedInvoiceID;

        private void LoadCustomerViews()
        {
            _selecttemp = new ObservableCollection<CustomerListItemViewModel>();
            var treeCustomers = new ObservableCollection<CustomerListItemViewModel>();

            if (_temp != null)
                foreach (var customerListItemViewModel in _temp)
                {
                    _selecttemp.Add(customerListItemViewModel);
                }
            var endDate = DateTime.Now;

            if (EndDate.HasValue)
                endDate = EndDate.Value;

            var starttime = new DateTime(2000, 01, 01);
            if (StartDate != null)
                starttime = (DateTime)StartDate;

            // Empty view, so user can se what's going on
            Customers = new ObservableCollection<CustomerListItemViewModel>();
            _dataService.GetCustomerInvoiceViews(starttime, endDate).Subscribe(
                    customerViews =>
                    {
                        if (customerViews == null) return;
                        foreach (var customerInvoiceView in customerViews)
                        {
                            if (SeeAllCustomers)
                                treeCustomers.Add(new CustomerListItemViewModel(customerInvoiceView));
                            else if(SeeDebitorlist)
                            {
                                if(customerInvoiceView.OverduePrice != null)
                                    treeCustomers.Add(new CustomerListItemViewModel(customerInvoiceView));
                            }
                            else
                            {
                                if (customerInvoiceView.InventoryValue != null || customerInvoiceView.Drafts > 0)
                                    treeCustomers.Add(new CustomerListItemViewModel(customerInvoiceView));
                            }
                        }
                        Customers = treeCustomers;

                        foreach (var customerListItemViewModel in _selecttemp)
                        {
                            SelectedCustomerListItems.Add(customerListItemViewModel);
                        }
                        if(!SeeDebitorlist)
                            InternalCommands.ReselectedCustomer.Execute(_selecttemp);         
                    });                    
        }

        private void RefreshCustomers(object obj)
        {
            if (SelectedCustomerListItem != null)
                _lastSelectedCustomerID = SelectedCustomerListItem.Customer.CustomerID;
            LoadCustomerViews();
        }

        private void RefreshInvoice(InvoiceListItemView obj)
        {
            SelectedInvoice = obj;
            FinalizeDraftCommand.RaiseCanExecuteChanged();
        }
    }
}
