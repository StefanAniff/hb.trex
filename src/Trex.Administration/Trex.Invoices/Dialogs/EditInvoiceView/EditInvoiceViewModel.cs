using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.Core.Services;
using Trex.Invoices.Commands;
using Trex.ServiceContracts;
using System.Linq;

namespace Trex.Invoices.Dialogs.EditInvoiceView
{
    public class EditInvoiceViewModel : ViewModelBase
    {
        public DelegateCommand<object> SaveInvoiceCommand { get; set; }
        public DelegateCommand<object> CancelCommand { get; set; }

        private ObservableCollection<CustomerInvoiceGroup> _customerInvoiceGroups;
        private readonly CustomerInvoiceGroup _customerInvoiceGroup;
        private readonly Invoice _invoice;
        private readonly Customer _customer;
        private readonly IDataService _dataService;

        public EditInvoiceViewModel(Invoice invoice, Customer customer, IDataService dataService)
        {
            
            _invoice = invoice;
            _customer = customer;
            _dataService = dataService;
            _customerInvoiceGroups = new ObservableCollection<CustomerInvoiceGroup>();
            _customerInvoiceGroup = new CustomerInvoiceGroup();
            SaveInvoiceCommand = new DelegateCommand<object>(ExecuteSaveInvoice, CanExecuteSaveInvoice);
            CancelCommand = new DelegateCommand<object>(ExecuteCancel, canExecuteCancel);

            GetAllCustomerInvoiceGroups();

            CustomerName = customer.CustomerName;
            CanEditSendFormat = false;
        }

        public ObservableCollection<CustomerInvoiceGroup> CustomerInvoiceGroups
        {
            get { return _customerInvoiceGroups; }
            set
            {
                _customerInvoiceGroups = value;
                OnPropertyChanged("CustomerInvoiceGroups");
            }
        }

        public string WindowTitle
        {
            get
            {
                return "Edit/Add Invoice";
            }
        }

        public string CustomerName { get; set; }

        private CustomerInvoiceGroup _selectedcig;
        public CustomerInvoiceGroup SelectedCIG
        {
            get { return _selectedcig; }
            set
            {
                _selectedcig = value;
                if (value == null)
                {
                    CanEdit = true;
                    CanEditSendFormat = false;
                }
                else
                {
                    CanEdit = !value.DefaultCig;
                    CanEditSendFormat = !value.DefaultCig;
                    
                }
                UpdateData();
                OnPropertyChanged("SelectedCIG");
                SaveInvoiceCommand.RaiseCanExecuteChanged();
            }
        }

        private void UpdateData()
        {
            if (SelectedCIG != null)
            {
                Attention = SelectedCIG.Attention;
                if (SelectedCIG.SendFormat == 1)
                {
                    IsMail = true;
                    IsPrint = false;
                }
                else if (SelectedCIG.SendFormat == 2)
                {
                    IsMail = false;
                    IsPrint = true;
                }
                Email = SelectedCIG.Email;
                EmailCC = SelectedCIG.EmailCC;
                Country = SelectedCIG.Country;
                City = SelectedCIG.City;
                ZipCode = SelectedCIG.ZipCode;
                BillingStreetAddress = SelectedCIG.Address1;
                BillingAddress2 = SelectedCIG.Address2;
            }

        }

        private void GetAllCustomerInvoiceGroups()
        {
            _dataService.GetCustomerInvoiceGroupByCustomerId(_customer.Id).Subscribe(
                i =>
                {
                    foreach (var customerInvoiceGroup in i)
                    {
                        CustomerInvoiceGroups.Add(customerInvoiceGroup);
                    }

                    NewCigLabel = CustomerInvoiceGroups.Where(d => d.DefaultCig).Select(l => l.Label).First();
                });
        }

        #region InvoiceData
        public DateTime EndDate
        {
            get { return _invoice.EndDate; }
            set
            {
                _invoice.EndDate = value;
                if (value < StartDate)
                {
                    OnPropertyChanged("StartDate");
                    throw new ValidationException("End date must be larger then start date");
                }
                
                OnPropertyChanged("EndDate");
                OnPropertyChanged("StartDate");
                OnPropertyChanged("DueDate");
            }
        }

        public DateTime StartDate
        {
            get { return _invoice.StartDate; }
            set
            {
                _invoice.StartDate = value;
                if(value > EndDate)
                {
                    OnPropertyChanged("EndDate");
                    throw new ValidationException("Start date must be smaller then end date");
                }
                
                OnPropertyChanged("StartDate");
                OnPropertyChanged("EndDate");
                OnPropertyChanged("DueDate");
            }
        }

        public DateTime? DueDate
        {
            get { return _invoice.DueDate; }
            set
            {
                _invoice.DueDate = value;

                OnPropertyChanged("DueDate");
                OnPropertyChanged("EndDate");
                OnPropertyChanged("StartDate");
            }
        }

        public DateTime InvoiceDate
        {
            get { return _invoice.InvoiceDate; }
            set
            {
                _invoice.InvoiceDate = value;
                OnPropertyChanged("InvoiceDate");
            }
        }

        public string Regarding
        {
            get { return _invoice.Regarding; }
            set
            {
                _invoice.Regarding = value;
                OnPropertyChanged("Regarding");
            }
        }
        #endregion

        #region cigData

        [Required]
        public string Attention
        {
            get
            {
                if (_customerInvoiceGroup.DefaultCig)
                    return _customer.ContactName;

                return _customerInvoiceGroup.Attention;
            }
            set
            {
                _customerInvoiceGroup.Attention = value;
                Validate("Attention", value);
                OnPropertyChanged("Attention");
            }
        }

        [Required]
        public string Email
        {
            get
            {
                if (_customerInvoiceGroup.DefaultCig)
                    return _customer.Email;

                return _customerInvoiceGroup.Email;
            }
            set
            {
                _customerInvoiceGroup.Email = value;
                if (IsMail)
                    Validate("Email", value);
                OnPropertyChanged("Email");
            }
        }

        public string EmailCC
        {
            get
            {
                if (_customerInvoiceGroup.DefaultCig)
                    return _customer.EmailCC;

                return _customerInvoiceGroup.EmailCC;
            } 
            set
            {
                _customerInvoiceGroup.EmailCC = value;
                OnPropertyChanged("EmailCC");
            }
        }

        [Required]
        public string Country
        {
            get
            {
                if (_customerInvoiceGroup.DefaultCig)
                    return _customer.Country;

                return _customerInvoiceGroup.Country;
            }
            set
            {
                _customerInvoiceGroup.Country = value;
                Validate("Country", value);
                OnPropertyChanged("Country");
            }
        }

        [Required]
        public string City
        {
            get
            {
                if (_customerInvoiceGroup.DefaultCig)
                    return _customer.City;

                return _customerInvoiceGroup.City;
            }
            set
            {
                _customerInvoiceGroup.City = value;
                Validate("City", value);
                OnPropertyChanged("City");
            }
        }

        [Required]
        public string ZipCode
        {
            get
            {
                if (_customerInvoiceGroup.DefaultCig)
                    return _customer.ZipCode;

                return _customerInvoiceGroup.ZipCode;
            }
            set
            {
                _customerInvoiceGroup.ZipCode = value;
                Validate("ZipCode", value);
                OnPropertyChanged("ZipCode");
            }
        }

        public string BillingAddress2
        {
            get
            {
                if (_customerInvoiceGroup.DefaultCig)
                    return _customer.Address2;

                return _customerInvoiceGroup.Address2;
            }
            set
            {
                _customerInvoiceGroup.Address2 = value;
                OnPropertyChanged("BillingAddress2");
            }
        }

        [Required]
        public string BillingStreetAddress
        {
            get
            {
                if (_customerInvoiceGroup.DefaultCig)
                    return _customer.StreetAddress;

                return _customerInvoiceGroup.Address1;
            }
            set
            {
                _customerInvoiceGroup.Address1 = value;
                Validate("BillingStreetAddress", value);
                OnPropertyChanged("BillingStreetAddress");
            }
        }

        public bool IsMail
        {
            get
            {
                if (_customerInvoiceGroup.DefaultCig)
                    if (_customer.SendFormat == 1)
                        return true;
                    else
                        return false;

                if (_customerInvoiceGroup.SendFormat == 2)
                    return false;
                return true;
            }
            set
            {
                if (value)
                    _customerInvoiceGroup.SendFormat = 1;
                else
                    _customerInvoiceGroup.SendFormat = 2;
                OnPropertyChanged("IsMail");
                OnPropertyChanged("Email");
            }
        }

        public bool IsPrint
        {
            get
            {
                if (_customerInvoiceGroup.DefaultCig)
                    if (_customer.SendFormat == 2)
                        return true;
                    else
                        return false;

                if (_customerInvoiceGroup.SendFormat == 1)
                    return false;
                return true;
            }
            set
            {
                if (value)
                    _customerInvoiceGroup.SendFormat = 2;
                else
                    _customerInvoiceGroup.SendFormat = 1;
                OnPropertyChanged("IsPrint");
                OnPropertyChanged("Email");

            }
        }
        #endregion

        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                _canEdit = !value;
                OnPropertyChanged("CanEdit");
            }
        }

        public bool CanEditSendFormat
        {
            get { return _canEditSendFormat; }
            set
            {
                _canEditSendFormat = value;
                OnPropertyChanged("CanEditSendFormat");
            }
        }

        private bool CanExecuteSaveInvoice(object arg)
        {
            if (NewCigLabel == null)
                return false;
            if (CheckAll())
                return true;
            return false;
        }

        private bool CheckAll()
        {
            if (IsMail)
                if (!string.IsNullOrEmpty(Attention) &&
                    !string.IsNullOrEmpty(Email) &&
                    !string.IsNullOrEmpty(City) &&
                    !string.IsNullOrEmpty(ZipCode) &&
                    !string.IsNullOrEmpty(BillingStreetAddress) && 
                    (StartDate <= EndDate))
                    return true;
                else
                    return false;
            else
                if (!string.IsNullOrEmpty(Attention) &&
                    !string.IsNullOrEmpty(City) &&
                    !string.IsNullOrEmpty(ZipCode) &&
                    !string.IsNullOrEmpty(BillingStreetAddress) &&
                    (StartDate <= EndDate))
                    return true;
            return false;
        }

        private string _newCigLabel;
        private bool _canEdit;
        private bool _canEditSendFormat;

        public string NewCigLabel
        {
            get { return _newCigLabel; }
            set
            {
                if (!CustomerInvoiceGroups.Select(l => l.Label).Contains(value))
                {
                    CanEdit = true;
                    CanEditSendFormat = true;
                }
                _newCigLabel = value;
                OnPropertyChanged("NewCigLabel");
                SaveInvoiceCommand.RaiseCanExecuteChanged();
            }
        }

        private void ExecuteSaveInvoice(object obj)
        {
            _customerInvoiceGroup.CustomerID = _customer.CustomerID;
            if (CustomerInvoiceGroups.Select(l => l.Label).Contains(NewCigLabel) && !SelectedCIG.DefaultCig)
            {
                var result = MessageBox.Show("Do you want to overwrite existing customer invoice group?", "wait",
                                MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    Overwrite();
                }
            }
            else if (SelectedCIG != null && SelectedCIG.DefaultCig)
            {
                _invoice.CustomerInvoiceGroup = SelectedCIG;
                SaveInvoice();
            }
            else
            {
                _customerInvoiceGroup.Label = NewCigLabel;
                _invoice.CustomerInvoiceGroup = _customerInvoiceGroup;
                SaveInvoice();
            }
        }

        private void Overwrite()
        {
            _customerInvoiceGroup.Label = NewCigLabel;
            _customerInvoiceGroup.CustomerInvoiceGroupID = SelectedCIG.CustomerInvoiceGroupID;

            _dataService.OverWriteCig(_customerInvoiceGroup)
                .Subscribe(d =>
                {
                    _invoice.CustomerInvoiceGroupId = _customerInvoiceGroup.CustomerInvoiceGroupID;
                    SaveInvoice();
                });
        }

        private void SaveInvoice()
        {
            _dataService.SaveInvoice(_invoice).Subscribe(invoice =>
            {
                if (_invoice.ChangeTracker.State == ObjectState.Added)
                    _invoice.ID = invoice.ID;
                InternalCommands.UpdateInvoiceList.Execute(null);
                InternalCommands.CloseAddEditInvoiceWindow.Execute(true);

            });
        }

        private bool canExecuteCancel(object arg)
        {
            return true;
        }

        private void ExecuteCancel(object obj)
        {
            InternalCommands.CloseAddEditInvoiceWindow.Execute(false);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            SaveInvoiceCommand.RaiseCanExecuteChanged();
        }
    }
}
