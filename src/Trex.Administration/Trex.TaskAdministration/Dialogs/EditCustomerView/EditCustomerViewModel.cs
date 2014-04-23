using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using System.Linq;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using System;

namespace Trex.TaskAdministration.Dialogs.EditCustomerView
{
    public class EditCustomerViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IUserRepository _users;
        private readonly bool _isNew;

        private Customer Customer { get; set; }

        private bool _isPrint;
        private bool _isMail;
        private User _selectedUser;
        //private Customer _tempcustomer;

        public EditCustomerViewModel(Customer customer, IDataService dataService, IUserRepository users)
        {
            SaveCommand = new DelegateCommand<object>(ExecuteSave, CanSave);
            CancelCommand = new DelegateCommand<object>(ExecuteCancel);

            Customer = customer;

            //Customer = customer.DeepCopy();
            //_tempcustomer = customer;

            _dataService = dataService;
            _users = users;
            _isNew = customer.Id == 0;
            if (customer.UserId != null)
                _selectedUser = users.Users.First(u => u.UserID == customer.UserId);

            if (customer.SendFormat == 1)
                IsMail = true;
            else if (_isNew)
                IsMail = true;
            else
                IsPrint = true;
        }

        [Required]
        public string CustomerName
        {
            get { return Customer.CustomerName; }
            set
            {
                Customer.CustomerName = value;
                OnPropertyChanged("CustomerName");
                Validate("CustomerName", value);
            }
        }

        [Required]
        public string Country
        {
            get { return Customer.Country; }
            set
            {
                Customer.Country = value;
                OnPropertyChanged("Country");
                Validate("Country", value);
            }
        }

        [Required]
        public string StreetAddress
        {
            get { return Customer.StreetAddress; }
            set
            {
                Customer.StreetAddress = value;
                OnPropertyChanged("StreetAddress");
                Validate("StreetAddress", value);
            }
        }

        [Required]
        public string ZipCode
        {
            get { return Customer.ZipCode; }
            set
            {
                Customer.ZipCode = value;
                OnPropertyChanged("ZipCode");
                Validate("ZipCode", value);
            }
        }

        [Required]
        public string City
        {
            get { return Customer.City; }
            set
            {
                Customer.City = value;
                OnPropertyChanged("City");
                Validate("City", value);
            }
        }

        [Required]
        public string ContactName
        {
            get { return Customer.ContactName; }
            set
            {
                Customer.ContactName = value;
                OnPropertyChanged("ContactName");
                Validate("ContactName", value);
            }
        }

        public string ContactPhone
        {
            get { return Customer.ContactPhone; }
            set
            {
                Customer.ContactPhone = value;
                OnPropertyChanged("ContactPhone");
            }
        }

        [Required]
        public string Email
        {
            get { return Customer.Email; }
            set
            {
                Customer.Email = value;
                OnPropertyChanged("Email");
                if (IsMail)
                    Validate("Email", value);
            }
        }

        public ObservableCollection<User> Users
        {
            get { return _users.Users; }
        }

        public User SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                Customer.UserId = value.UserID;
            }
        }

        public string EmailCC
        {
            get { return Customer.EmailCC; }
            set
            {
                Customer.EmailCC = value;
                OnPropertyChanged("EmailCC");
            }
        }

        public string Address2
        {
            get { return Customer.Address2; }
            set
            {
                Customer.Address2 = value;
                OnPropertyChanged("Address2");
            }
        }

        public bool IsActive
        {
            get { return !Customer.Inactive; }
            set
            {
                Customer.Inactive = !value;
                OnPropertyChanged("IsActive");
            }
        }

        public bool? Internal
        {
            get { return Customer.Internal; } 
            set { Customer.Internal = value; }
        }

        public bool IsMail
        {
            get { return _isMail; }
            set
            {
                _isMail = value;
                OnPropertyChanged("IsMail");
                OnPropertyChanged("Email");
                Customer.SendFormat = 1;
            }
        }

        public bool IsPrint
        {
            get { return _isPrint; }
            set
            {
                _isPrint = value;
                OnPropertyChanged("IsPrint");
                OnPropertyChanged("Email");
                Customer.SendFormat = 2;
            }
        }

        public string PaymentTermNumberOfDays
        {
            get { return Customer.PaymentTermsNumberOfDays.ToString(); }
            set
            {
                int days;
                if (!int.TryParse(value, out days))
                {
                    throw new ValidationException("Value must be a positive integer");
                }

                Customer.PaymentTermsNumberOfDays = days;
                OnPropertyChanged("PaymentTermNumberOfDays");
            }
        }

        public bool PaymentTermIncludeCurrentMonth
        {
            get { return Customer.PaymentTermsIncludeCurrentMonth; }
            set
            {
                Customer.PaymentTermsIncludeCurrentMonth = value;
                OnPropertyChanged("PaymentTermIncludeCurrentMonth");
            }
        }

        public DelegateCommand<object> SaveCommand { get; set; }
        public DelegateCommand<object> CancelCommand { get; set; }

        private void ExecuteCancel(object obj)
        {
            InternalCommands.CustomerEditCompleted.Execute(null);
        }

        private bool CanSave(object arg)
        {
            if (CheckAll())
                return true;
            return false;
        }

        private bool CheckAll()
        {
            if (IsMail)
                if (!string.IsNullOrEmpty(CustomerName) &&
                    !string.IsNullOrEmpty(StreetAddress) &&
                    !string.IsNullOrEmpty(ZipCode) &&
                    !string.IsNullOrEmpty(City) &&
                    !string.IsNullOrEmpty(Country) &&
                    !string.IsNullOrEmpty(Email)
                  )
                    return true;
                else
                    return false;
            if (!string.IsNullOrEmpty(CustomerName) &&
                !string.IsNullOrEmpty(StreetAddress) &&
                !string.IsNullOrEmpty(ZipCode) &&
                !string.IsNullOrEmpty(City) &&
                !string.IsNullOrEmpty(Country)
                )
                return true;
            return false;
        }

        private void ExecuteSave(object obj)
        {

            _dataService.SaveCustomer(Customer).Subscribe(
                customer =>
                {
                    if (_isNew)
                    {
                        Customer = Customer;
                        Customer.CustomerID = customer.CustomerID;
                        Customer.AcceptChanges();
                        OverwriteCig(Customer);
                        InternalCommands.CustomerAddCompleted.Execute(Customer);
                    }
                    else
                    {
                        Customer = Customer;
                        Customer.AcceptChanges();
                        OverwriteCig(Customer);
                        InternalCommands.CustomerEditCompleted.Execute(Customer);
                    }
                });
        }

        private void OverwriteCig(Customer customer)
        {
            _dataService.GetCustomerInvoiceGroupByCustomerId(customer.CustomerID)
                .Subscribe(r =>
                               {
                                   if (r.Count != 0)
                                   {
                                       CustomerInvoiceGroup cig = r.First(f => f.DefaultCig);
                                       cig.Label = "Default";
                                       cig.Attention = customer.ContactName;
                                       cig.Email = customer.Email;
                                       cig.Address1 = customer.StreetAddress;
                                       cig.Address2 = customer.Address2;
                                       cig.Country = customer.Country;
                                       cig.ZipCode = customer.ZipCode;
                                       cig.City = customer.City;
                                       cig.CustomerID = customer.CustomerID;
                                       cig.SendFormat = customer.SendFormat;
                                       cig.EmailCC = customer.EmailCC;
                                       SaveCig(cig);
                                   }
                                   else
                                       CreateNewCig();
                               });
        }

        private void CreateNewCig()
        {
            var cig = new CustomerInvoiceGroup
                          {
                              Label = "Default",
                              Attention = Customer.ContactName,
                              Email = Customer.Email,
                              EmailCC = Customer.EmailCC,
                              Address1 = Customer.StreetAddress,
                              Address2 = Customer.Address2,
                              Country = Customer.Country,
                              City = Customer.City,
                              CustomerID = Customer.CustomerID,
                              SendFormat = 1,
                              DefaultCig = true
                          };

            _dataService.SaveCustomerInvoiceGroup(cig);
        }

        private void SaveCig(CustomerInvoiceGroup cig)
        {
            _dataService.OverWriteCig(cig).Subscribe(s =>
                                                         {
                                                            if (s == null)
                                                                MessageBox.Show(
                                                                    "The settings are properly not saved due to an error on the server",
                                                                    "Error", MessageBoxButton.OK);
                                                            if (!s.Success)
                                                                MessageBox.Show(s.Response);
            });
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}