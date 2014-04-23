using System;
using Microsoft.Practices.Composite.Presentation.Commands;
using Trex.Core.Implemented;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.Infrastructure.Implemented;
//using Trex.Infrastructure.TrexSLService;
using Trex.Invoices.Commands;
using Trex.ServiceContracts;

namespace Trex.Invoices.InvoiceManagementScreen.CustomerTreeView
{
    public class CustomerListItemViewModel : ViewModelBase
    {
        private readonly CustomerInvoiceView _customer;
        




        public CustomerListItemViewModel(CustomerInvoiceView customer)
        {
            _customer = customer;
            
        }


        public CustomerInvoiceView Customer
        {
            get
            {
                return _customer;
            }

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


        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }


    }
}
