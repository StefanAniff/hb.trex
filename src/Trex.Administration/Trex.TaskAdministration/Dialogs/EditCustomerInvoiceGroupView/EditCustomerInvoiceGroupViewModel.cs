using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Browser;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.Dialogs.EditCustomerInvoiceGroupView
{
    public class EditCustomerInvoiceGroupViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private EditCustomerInvoiceGroupItemViewModel _selectedCustomerInvoiceGroups;
        private readonly Customer _customer;
        private ObservableCollection<EditCustomerInvoiceGroupItemViewModel> _customerInvoiceGroups;
        private bool _isReadOnly;
        public DelegateCommand<object> CloseAndSaveCIGView { get; set; }
        public DelegateCommand<object> CloseAndUnsaveCIGView { get; set; }
        public DelegateCommand<object> AddNew { get; set; }
        public DelegateCommand<object> DeleteCustomerInvoiceGroup { get; set; }
        public int selectedCIGid;

        public EditCustomerInvoiceGroupViewModel(Customer customer, IDataService dataService)
        {
            _customer = customer;
            _dataService = dataService;

            CloseAndSaveCIGView = new DelegateCommand<object>(ExecuteCloseAndSaveCIGView, CanCloseAndSaveCIGView);
            CloseAndUnsaveCIGView = new DelegateCommand<object>(ExecuteCloseAndUnsaveCIGView);
            AddNew = new DelegateCommand<object>(ExecuteAddNew);
            DeleteCustomerInvoiceGroup = new DelegateCommand<object>(ExecuteDeleteCustomerInvoiceGroup);

            CustomerInvoiceGroups = new ObservableCollection<EditCustomerInvoiceGroupItemViewModel>();

            InternalCommands.CigCanExecuteSave.RegisterCommand(new DelegateCommand(CigCanExecuteSave));

            GetCustomerInvoiceGroups();
        }

        private bool CanCloseAndSaveCIGView(object arg)
        {
            return CheckAll();
        }

        public bool CheckAll()
        {
            if (CustomerInvoiceGroups.Where(s => s.IsMail).Select(r => r.Email).Contains(string.Empty))
                if (CustomerInvoiceGroups.Select(l => l.Label).Contains(null) ||
                    CustomerInvoiceGroups.Select(s => s.Address1).Contains(null) ||
                    CustomerInvoiceGroups.Select(z => z.ZipCode).Contains(null) ||
                    CustomerInvoiceGroups.Select(c => c.City).Contains(null) ||
                    CustomerInvoiceGroups.Select(cc => cc.Country).Contains(null) ||
                    CustomerInvoiceGroups.Select(e => e.Email).Contains(string.Empty))
                    return false;
                else
                    return true;
            if (CustomerInvoiceGroups.Select(l => l.Label).Contains(null) ||
                CustomerInvoiceGroups.Select(s => s.Address1).Contains(null) ||
                CustomerInvoiceGroups.Select(z => z.ZipCode).Contains(null) ||
                CustomerInvoiceGroups.Select(c => c.City).Contains(null) ||
                CustomerInvoiceGroups.Select(cc => cc.Country).Contains(null))
                return false;
            return true;
        }

        private void ExecuteDeleteCustomerInvoiceGroup(object obj)
        {

            if (SelectedCustomerInvoiceGroups != null && !SelectedCustomerInvoiceGroups.CIG.DefaultCig)
            {
                _dataService.DeleteCustomerInvoiceGroup(SelectedCustomerInvoiceGroups.CIG.CustomerInvoiceGroupID).Subscribe(
                    r =>
                    {
                        if (r.Success == false)
                        {
                            if (r.Response == "Could not delete CustomerInvoiceGroup")
                                CustomerInvoiceGroups.Remove(SelectedCustomerInvoiceGroups);
                            else
                                MessageBox.Show(r.Response);
                            
                        }
                        if (r.Success)
                            CustomerInvoiceGroups.Remove(SelectedCustomerInvoiceGroups);
                    });
            }
        }

        private void ExecuteAddNew(object obj)
        {
            CustomerInvoiceGroups.Add(
                new EditCustomerInvoiceGroupItemViewModel(
                new CustomerInvoiceGroup
                {
                    Attention = _customer.ContactName,
                    Email = _customer.Email,
                    Address1 = _customer.StreetAddress,
                    Address2 = _customer.Address2,
                    ZipCode = _customer.ZipCode,
                    City = _customer.City,
                    Country = _customer.Country,
                    CustomerID = _customer.CustomerID,
                    SendFormat = _customer.SendFormat,
                    EmailCC = _customer.EmailCC
                }));
            CloseAndSaveCIGView.RaiseCanExecuteChanged();
            SelectedCustomerInvoiceGroups = CustomerInvoiceGroups.Last();

        }

        private void ExecuteCloseAndSaveCIGView(object obj)
        {
            _dataService.InsertCustomerInvoiceGroup(new ObservableCollection<CustomerInvoiceGroup>(CustomerInvoiceGroups.Select(f => f.CIG))).Subscribe(
                r =>
                {
                    if (r.Success == false)
                        MessageBox.Show(r.Response);

                    InternalCommands.UpdateCustomerInvoiceGroupList.Execute(_customer);
                    InternalCommands.CustomerInvoiceGroupComplete.Execute(null);
                });
        }

        private void ExecuteCloseAndUnsaveCIGView(object obj)
        {
            InternalCommands.CustomerInvoiceGroupComplete.Execute(null);
        }

        public EditCustomerInvoiceGroupItemViewModel SelectedCustomerInvoiceGroups
        {
            get { return _selectedCustomerInvoiceGroups; }
            set
            {
                _selectedCustomerInvoiceGroups = value;
                OnPropertyChanged("SelectedCustomerInvoiceGroups");
            }
        }

        public ObservableCollection<EditCustomerInvoiceGroupItemViewModel> CustomerInvoiceGroups
        {
            get { return _customerInvoiceGroups; }
            set
            {
                _customerInvoiceGroups = value;
                OnPropertyChanged("CustomerInvoiceGroups");
            }
        }

        public void GetCustomerInvoiceGroups()
        {
            _dataService.GetCustomerInvoiceGroupByCustomerId(_customer.Id).Subscribe(
                i =>
                {
                    foreach (var customerInvoiceGroup in i)
                    {
                        CustomerInvoiceGroups.Add(new EditCustomerInvoiceGroupItemViewModel(customerInvoiceGroup));
                    }
                });
        }

        public void CigCanExecuteSave()
        {
            CloseAndSaveCIGView.RaiseCanExecuteChanged();
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            CloseAndSaveCIGView.RaiseCanExecuteChanged();
        }
    }
}
