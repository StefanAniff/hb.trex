#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.Administration.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.ServiceContracts;

#endregion

namespace Trex.Administration.Dialogs.EditUserPricesDialog
{
    public class EditUserPricesDialogViewModel : ViewModelBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDataService _dataService;
        private readonly DelegateCommand<UsersCustomer> _deleteCustomerInfo;
        private readonly IExceptionHandlerService _exceptionHandlerService;
        private readonly User _originalUser;
        private User _user;
        private User _tempUser;

        private ObservableCollection<Customer> _customers;
        private double _price;
        private ObservableCollection<UserPriceRowViewModel> _prices;
        private Customer _selectedCustomer;

        public EditUserPricesDialogViewModel(User user, ICustomerRepository customerRepository, IDataService dataService,
                                             IExceptionHandlerService exceptionHandlerService)
        {
            _deleteCustomerInfo = new DelegateCommand<UsersCustomer>(DeleteCustomerInfo);
            AddCommand = new DelegateCommand<object>(ExecuteAdd, CanExecuteAdd);
            SaveCommand = new DelegateCommand<object>(ExecuteSaveCommand, CanExecuteSave);
            CancelCommand = new DelegateCommand<object>(ExecuteCancel);
            InternalCommands.DeleteCustomerInfo.RegisterCommand(_deleteCustomerInfo);

            _user = user;
            _tempUser = user.DeepCopy();

            _customerRepository = customerRepository;
            _dataService = dataService;
            _exceptionHandlerService = exceptionHandlerService;

            Customers = new ObservableCollection<Customer>();

            InitializePrices();
            InitializeCustomers();
        }

        public string DialogTitle
        {
            get { return string.Format("Customer rates for {0}", _user.Name); }
        }

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }

        public double Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }

        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                OnPropertyChanged("Customers");
            }
        }

        public ObservableCollection<UserPriceRowViewModel> Prices
        {
            get { return _prices; }
            set
            {
                _prices = value;
                OnPropertyChanged("Prices");
            }
        }

        public DelegateCommand<object> AddCommand { get; set; }
        public DelegateCommand<object> SaveCommand { get; set; }
        public DelegateCommand<object> CancelCommand { get; set; }

        private void DeleteCustomerInfo(UsersCustomer obj)
        {
            obj.MarkAsDeleted();

            var rowToRemove = _prices.SingleOrDefault(p => p.UserCustomerInfo.Equals(obj));
            if (rowToRemove != null)
                _prices.Remove(rowToRemove);

            //_dataService.SaveUser(_user).Subscribe(
            //    result => InitializePrices()
            //    );
            //_user.RemoveCustomerInfo(obj);
            // 
        }

        private void ExecuteCancel(object obj)
        {
            _user = _tempUser;
            InitializeCustomers();
            InitializePrices();
            InternalCommands.EditUserPricesCompleted.Execute(null);
        }

        private bool CanExecuteSave(object arg)
        {
            return true;
        }

        private void ExecuteSaveCommand(object obj)
        {
            _dataService.SaveUser(_user).Subscribe(
                result =>
                    {
                        _user.AcceptChanges();

                        foreach (var userPriceRowViewModel in Prices)
                        {
                            userPriceRowViewModel.SubmitChanges();
                        }
                        InternalCommands.EditUserPricesCompleted.Execute(null);
                    }
                , _exceptionHandlerService.OnError);
        }

        private bool CanExecuteAdd(object arg)
        {
            return SelectedCustomer != null && !Price.Equals(0);
        }

        private void ExecuteAdd(object obj)
        {
            _user.AddCustomerInfo(new UsersCustomer { User = _user, Customer = SelectedCustomer, Price = Price });
            InitializeCustomers();
            InitializePrices();
        }

        private void InitializeCustomers()
        {
            var customers =
                _customerRepository.Customers.Where(
                    c => _user.UsersCustomers.FirstOrDefault(uc => uc.CustomerID == c.Id) == null).OrderBy(
                        c => c.CustomerName);
            Customers.Clear();
            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }

            OnPropertyChanged("Customers");
        }

        private void InitializePrices()
        {
            var prices = new List<UserPriceRowViewModel>();
            Prices = new ObservableCollection<UserPriceRowViewModel>();
            foreach (var userCustomerInfo in _user.UsersCustomers)
            {
                var customer = _customerRepository.Customers.SingleOrDefault(c => c.Id == userCustomerInfo.CustomerID);
                if (customer != null)
                    prices.Add(new UserPriceRowViewModel(customer, userCustomerInfo));
            }
            var orderedPrices = prices.OrderBy(p => p.Customer);

            foreach (var userPriceRowViewModel in orderedPrices)
            {
                Prices.Add(userPriceRowViewModel);
            }
            OnPropertyChanged("Prices");
        }

        public override void Close()
        {
            InternalCommands.DeleteCustomerInfo.UnregisterCommand(_deleteCustomerInfo);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            AddCommand.RaiseCanExecuteChanged();
            base.OnPropertyChanged(propertyName);
        }
    }
}