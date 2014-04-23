using System;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
﻿using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Invoices.Commands;
using Trex.ServiceContracts;

namespace Trex.Invoices.InvoiceManagementScreen.InvoiceView
{
    public class InvoiceLineListItemViewModel : ViewModelBase
    {
        private InvoiceLine _invoiceLine;
        private readonly IDataService _data;
        public DelegateCommand<object> AddInvoiceLine { get; set; }
        public DelegateCommand<object> DeleteInvoiceLine { get; set; }

        public InvoiceLineListItemViewModel(InvoiceLine invoiceLine, IDataService dataService)
        {
            _data = dataService;
            _invoiceLine = invoiceLine;
            AddInvoiceLine = new DelegateCommand<object>(ExecuteAddInvoiceLine, CanExecuteAddInvoiceLine);
            DeleteInvoiceLine = new DelegateCommand<object>(ExecuteDeleteInvoiceLine, CanExecuteDeleteInvoiceLine);
            ButtonOpacity = 1;
            _invoiceLine.VatPercentage = 0.25;
        }

        public InvoiceLine InvoiceLine { get { return _invoiceLine; } }

        public int invoiceLineId
        {
            get { return InvoiceLine.ID; }
            set { InvoiceLine.ID = value; }
        }

        private void ExecuteDeleteInvoiceLine(object obj)
        {
            _data.DeleteInvoiceLine(InvoiceLine.ID).Subscribe(r => InternalCommands.GenerateInvoiceLines.Execute(null));
        }

        private bool CanExecuteDeleteInvoiceLine(object obj)
        {
            if (InvoiceLine.Invoice == null)
                return true;


            if (InvoiceLine.ID == 0 || (UnitType == 1 && InvoiceLine.Invoice.InvoiceID == null))
            {
                ButtonOpacity = 1;
                return true;
            }
            ButtonOpacity = 0;
            return false;
        }

        private void ExecuteAddInvoiceLine(object obj)
        {
            _data.AddNewInvoiceLine(InvoiceLine.InvoiceID, 0.25).Subscribe(r => InternalCommands.GenerateInvoiceLines.Execute(null));
        }

        private bool CanExecuteAddInvoiceLine(object obj)
        {
            ButtonOpacity = 0;
            return false;
        }

        public double ButtonOpacity { get; set; }

        public bool IsTemp { get; set; }

        public double Units
        {
            get { return _invoiceLine.Units; }
            set
            {
                _invoiceLine.Units = value;
                Update();
                SaveInvoiceline();
            }
        }

        public string Unit
        {
            get { return _invoiceLine.Unit; }
            set
            {
                if (value != string.Empty)
                {
                    _invoiceLine.Unit = value;
                    OnPropertyChanged("Unit");
                    SaveInvoiceline();
                }
            }
        }

        public int UnitType
        {
            get { return _invoiceLine.UnitType; }
            set
            {
                _invoiceLine.UnitType = value;
            }
        }

        public double Price
        {
            get
            {
                if (_invoiceLine.UnitType == 2)
                {
                    return Math.Round(_invoiceLine.PricePrUnit / _invoiceLine.Units, 2);
                }
                return _invoiceLine.PricePrUnit;
            }
            set
            {
                _invoiceLine.PricePrUnit = value;
                Update();
                SaveInvoiceline();
            }
        }

        public double VatAmount
        {
            get { return _invoiceLine.VatPercentage; }

            set
            {
                _invoiceLine.VatPercentage = value;
                Update();
                SaveInvoiceline();
            }
        }

        public bool IsExpense
        {
            get { return _invoiceLine.IsExpense; }
            set
            {
                _invoiceLine.IsExpense = value;
                OnPropertyChanged("IsExpense");
                SaveInvoiceline();
            }
        }

        public bool CanEdit
        {
            get
            {
                if ((UnitType == 1 && (InvoiceLine.Invoice == null || InvoiceLine.Invoice.InvoiceID == null)))
                    return false;
                else
                    return true;
            }
        }

        public bool CanEditExp
        {
            get
            {
                if ((UnitType == 1 && (InvoiceLine.Invoice == null || InvoiceLine.Invoice.InvoiceID == null)))
                    return true;
                else
                    return false;
            }
        }

        public bool CanEditDescription
        {
            get
            {
                if ((InvoiceLine.Invoice != null && InvoiceLine.Invoice.InvoiceID != null || _invoiceLine.UnitType == 2))
                    return true;
                return false;
            }
        }

        public string Description
        {
            get
            {
                if (InvoiceLine.Text == null)
                    InvoiceLine.Text = string.Empty;
                return _invoiceLine.Text;
            }
            set
            {
                _invoiceLine.Text = value;
                OnPropertyChanged("Description");
                SaveInvoiceline();
            }
        }

        private void SaveInvoiceline()
        {
            if (!IsTemp)
                _data.SaveInvoiceLine(_invoiceLine).Subscribe(r =>
                {
                    if (r.ID != this.InvoiceLine.ID && InvoiceLine.ID != 0)
                        _data.DeleteInvoiceLine(this.InvoiceLine.ID);
                    this.invoiceLineId = r.ID;

                });
        }

        public string Total
        {
            get
            {
                if (_invoiceLine.UnitType == 2)
                {
                    var t = (_invoiceLine.PricePrUnit * (VatAmount + 1)).ToString("N");
                    return t;

                }
                return (_invoiceLine.PricePrUnit * Units * (VatAmount + 1)).ToString("N");
            }
        }

        public bool HasChanges
        {
            get
            {
                return _invoiceLine.ChangeTracker.State != ObjectState.Unchanged;
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (IsTemp)
            {
                InternalCommands.AddDummyInvoiceLines.Execute(null);
                IsTemp = false;
            }


            //if (!IsTemp)
            //    _data.SaveInvoiceLine(this.InvoiceLine);
            InternalCommands.UpdateExclVAT.Execute(InvoiceLine.InvoiceID);

            base.OnPropertyChanged(propertyName);
        }
    }
}
