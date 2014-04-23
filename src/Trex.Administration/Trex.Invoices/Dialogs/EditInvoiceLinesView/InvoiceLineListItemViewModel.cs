using Trex.Core.Implemented;
using Trex.Invoices.Commands;
using Trex.ServiceContracts;

namespace Trex.Invoices.Dialogs.EditInvoiceLinesView
{
    public class InvoiceLineListItemViewModel : ViewModelBase
    {
        private readonly InvoiceLine _invoiceLine;

        public InvoiceLineListItemViewModel(InvoiceLine invoiceLine)
        {
            _invoiceLine = invoiceLine;
        }

        public InvoiceLine InvoiceLine { get { return _invoiceLine; } }

        public double Units
        {
            get { return _invoiceLine.Units; }
            set
            {
                _invoiceLine.Units = value;
                Update();
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

        public double Price
        {
            get { return _invoiceLine.PricePrUnit; }
            set
            {
                _invoiceLine.PricePrUnit = value;
                Update();

            }
        }

        public double VatAmount
        {
            get { return _invoiceLine.VatPercentage; }

            set
            {
                _invoiceLine.VatPercentage = value;
                Update();
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

        public string Description
        {
            get { return _invoiceLine.Text; }
            set
            {
                _invoiceLine.Text = value;
                OnPropertyChanged("Description");
            }
        }

        public double Total { get { return _invoiceLine.PricePrUnit * Units; } }

        public bool HasChanges
        {
            get
            {
                return _invoiceLine.ChangeTracker.State != ObjectState.Unchanged;
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            InternalCommands.InvoiceLineChanged.Execute(this.InvoiceLine);
            base.OnPropertyChanged(propertyName);
        }
    }
}
