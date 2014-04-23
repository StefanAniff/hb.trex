using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.Invoices.Commands;

namespace Trex.Invoices.Dialogs.EditInvoiceLinesView
{
    public partial class EditInvoiceLinesView : ChildWindow, IView
    {
        private DelegateCommand<object> _editCompleted;

        public EditInvoiceLinesView()
        {
            InitializeComponent();
            _editCompleted = new DelegateCommand<object>(ExecuteCompleted);
            
            InternalCommands.ManageInvoiceLinesCompleted.RegisterCommand(_editCompleted);
        }

        private void ExecuteCompleted(object obj)
        {
            InternalCommands.ManageInvoiceLinesCompleted.UnregisterCommand(_editCompleted);
            ViewModel.Close();
            this.Close();
        }

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }
    }
}

