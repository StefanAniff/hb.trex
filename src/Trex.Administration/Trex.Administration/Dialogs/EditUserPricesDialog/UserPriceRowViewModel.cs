using Microsoft.Practices.Prism.Commands;
using Trex.Administration.Commands;
using Trex.Core.Implemented;
using Trex.ServiceContracts;

namespace Trex.Administration.Dialogs.EditUserPricesDialog
{
    public class UserPriceRowViewModel : ViewModelBase
    {
        private readonly Customer _customer;
        private UsersCustomer _customerInfo;
        private UsersCustomer _tempcustomerInfo;


        public UserPriceRowViewModel(Customer customer, UsersCustomer customerInfo)
        {
            _customer = customer;

            _customerInfo = customerInfo;
            _tempcustomerInfo = customerInfo.DeepCopy();
            DeleteCommand = new DelegateCommand<object>(ExecuteDelete, CanExecuteDelete);
        }

        public string Customer
        {
            get { return _customer.CustomerName; }
        }

        public UsersCustomer UserCustomerInfo { get { return _customerInfo; } }

        public double PricePrHour
        {
            get { return _customerInfo.Price; }
            set
            {
                _customerInfo.Price = value;
                OnPropertyChanged("PricePrHour");
            }
        }

        public DelegateCommand<object> DeleteCommand { get; set; }

        private bool CanExecuteDelete(object arg)
        {
            return true;
        }

        private void ExecuteDelete(object obj)
        {

            InternalCommands.DeleteCustomerInfo.Execute(_customerInfo);
        }

        public void SubmitChanges()
        {
            _customerInfo.AcceptChanges();

        }

        public void CancelChanges()
        {
            _customerInfo.ChangeTracker.SetParentObject(_customerInfo);
            _customerInfo.CancelChanges();
        }
    }
}