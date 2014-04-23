#region

using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.ServiceContracts;

#endregion

namespace Trex.Invoices.Dialogs.EditInvoiceLineView
{
    public class EditInvoiceLineViewModel : ViewModelBase
    {
        private readonly InvoiceLine _invoiceLine;

        private bool _isNew;

        public EditInvoiceLineViewModel(InvoiceLine invoiceLine)
        {
            _invoiceLine = invoiceLine;

            _isNew = invoiceLine.ID == 0;
        }

        public double Units
        {
            get { return _invoiceLine.Units; }
            set
            {
                _invoiceLine.Units = value;
                OnPropertyChanged("Units");
            }
        }

        public string Unit
        {
            get { return _invoiceLine.Unit; }
            set
            {
                _invoiceLine.Unit = value;
                OnPropertyChanged("Unit");
            }
        }

        public double PricePrUnit
        {
            get { return _invoiceLine.PricePrUnit; }
            set
            {
                _invoiceLine.PricePrUnit = value;
                OnPropertyChanged("PricePrUnit");
            }
        }

        public double VatPercentage
        {
            get { return _invoiceLine.VatPercentage; }
            set
            {
                _invoiceLine.VatPercentage = value;
                OnPropertyChanged("VatPercentage");
            }
        }

        public bool IsExpense
        {
            get { return _invoiceLine.IsExpense; }
            set
            {
                _invoiceLine.IsExpense = value;
                OnPropertyChanged("IsExpense");
            }
        }

        public string Text
        {
            get { return _invoiceLine.Text; }
            set
            {
                _invoiceLine.Text = value;
                OnPropertyChanged("Text");
            }
        }
    }
}