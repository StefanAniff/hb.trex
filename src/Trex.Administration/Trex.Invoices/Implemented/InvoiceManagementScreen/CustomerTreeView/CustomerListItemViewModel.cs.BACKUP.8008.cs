#region

using System;
using Trex.Core.Implemented;
using Trex.ServiceContracts;
//using Trex.Infrastructure.TrexSLService;

#endregion

namespace Trex.Invoices.InvoiceManagementScreen.CustomerTreeView
{
    public class CustomerListItemViewModel : ViewModelBase
    {
        private readonly CustomerInvoiceView _customer;
<<<<<<< Updated upstream:WorkingTGA/tga/Trex.Invoices/Implemented/InvoiceManagementScreen/CustomerTreeView/CustomerListItemViewModel.cs
=======
        private bool _isSelected;
>>>>>>> Stashed changes:WorkingTGA/tga/Trex.Invoices/InvoiceManagementScreen/CustomerTreeView/CustomerListItemViewModel.cs

        public CustomerListItemViewModel(CustomerInvoiceView customer)
        {
            _customer = customer;
        }

        public CustomerInvoiceView Customer
        {
            get { return _customer; }
        }

        public string DistinctPrices
        {
            get
            {
                if (Customer.DistinctPrice.HasValue)
                    return Customer.DistinctPrice.Value.ToString("N0");

                return "0";
            }
        }

        public string InventoryValue
        {
            get
            {
                if (Customer.InventoryValue.HasValue)
                    return Customer.InventoryValue.Value.ToString("N2");
                return "0";
            }
        }

        public DateTime FirstTimeEntryDate
        {
            get
            {
                if (Customer.FirstDateNotInvoiced.HasValue)
                    return Customer.FirstDateNotInvoiced.Value;

                return DateTime.Now;
            }
        }

        public string DisplayName
        {
            get { return Customer.CustomerName; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
    }
}
