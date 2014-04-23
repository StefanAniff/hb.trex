﻿#region

using System;
using System.Windows;
using Telerik.Windows.Controls;
using Trex.Infrastructure.Implemented;
using Trex.Invoices.Commands;
using Trex.ServiceContracts;
using ViewModelBase = Trex.Core.Implemented.ViewModelBase;

//using Trex.Infrastructure.TrexSLService;

#endregion

namespace Trex.Invoices.InvoiceManagementScreen.CustomerTreeView
{
    public class CustomerListItemViewModel : ViewModelBase
    {
        private readonly CustomersInvoiceView _customer;
        private bool _isSelected;
        private Visibility _timeEntryVisibility;
        private Visibility _nonBillableVisibility;
        private Visibility _draftVisibility;
        private Visibility _OverduepriceVisibility;

        public CustomerListItemViewModel(CustomersInvoiceView customer)
        {
            _customer = customer;
        }

        public CustomersInvoiceView Customer
        {
            get { return _customer; }
        }

        public string Overdueprice
        {
            get
            {
                if (Customer.OverduePrice.HasValue)
                    return Customer.OverduePrice.Value.ToString("N2");
                OverduepriceVisibility = Visibility.Collapsed;
                return "0";

            }
        }

        public string InventoryValue
        {
            get
            {
                return Customer.InventoryValue.HasValue ? Customer.InventoryValue.Value.ToString("N2") : "0";
            }
        }

        public bool? Internal
        {
            get { return Customer.@internal; }
        }

        public DateTime? FirstTimeEntryDate
        {
            get
            {
                if (Customer.FirstDateNotInvoiced.HasValue)
                {
                    return Customer.FirstDateNotInvoiced.Value;
                }
                TimeEntryVisibility = Visibility.Collapsed;
                return null;
            }
        }

        public double? NonBillableTIme
        {
            get
            {
                if (_customer.NonBillableTime != null)
                    return Math.Round((Double)_customer.NonBillableTime * 4, 0) / 4;
                NonBillableVisibility = Visibility.Collapsed;
                return null;
            }
        }

        public int HasDraft
        {
            get
            {
               
                if (Customer.Drafts != null && _customer.Drafts != 0)
                    return (int)Customer.Drafts;
                if (Customer.Drafts != null && _customer.Drafts == 0)
                    return 0;
                DraftVisibility = Visibility.Collapsed;
                return -1;
            }
        }

        public string DisplayName
        {
            get { return Customer.CustomerName; }
        }

        public Visibility TimeEntryVisibility
        {
            get { return _timeEntryVisibility; }
            set
            {
                _timeEntryVisibility = value;
                Execute.InUIThread(() => OnPropertyChanged("TimeEntryVisibility"));
            }
        }

        public Visibility NonBillableVisibility
        {
            get { return _nonBillableVisibility; }
            set
            {
                _nonBillableVisibility = value;
                OnPropertyChanged("NonBillableVisibility");
            }
        }

        public Visibility DraftVisibility
        {
            get { return _draftVisibility; }
            set
            {
                _draftVisibility = value;
                OnPropertyChanged("DraftVisibility");
            }
        }

        public Visibility OverduepriceVisibility
        {
            get { return _OverduepriceVisibility; }
            set
            {
                _OverduepriceVisibility = value;
                OnPropertyChanged("OverduepriceVisibility");
            }
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
