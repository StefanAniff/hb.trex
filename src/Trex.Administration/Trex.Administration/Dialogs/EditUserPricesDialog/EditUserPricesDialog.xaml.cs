using Microsoft.Practices.Prism.Commands;
using Trex.Administration.Commands;
using Trex.Core.Interfaces;

namespace Trex.Administration.Dialogs.EditUserPricesDialog
{
    public partial class EditUserPricesDialog : IView
    {
        private DelegateCommand<object> _editUserPricesCompleted;

        public EditUserPricesDialog()
        {
            _editUserPricesCompleted = new DelegateCommand<object>(CloseDialog);
            InitializeComponent();
            InternalCommands.EditUserPricesCompleted.RegisterCommand(_editUserPricesCompleted);
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion

        private void CloseDialog(object obj)
        {
            InternalCommands.EditUserPricesCompleted.UnregisterCommand(_editUserPricesCompleted);
            this.ViewModel.Close();

            Close();

        }
    }
}