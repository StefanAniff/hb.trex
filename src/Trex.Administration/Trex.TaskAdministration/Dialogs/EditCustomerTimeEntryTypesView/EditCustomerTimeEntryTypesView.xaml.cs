using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.Dialogs.EditCustomerTimeEntryTypesView
{
    public partial class EditCustomerTimeEntryTypesView : ChildWindow, IView
    {
        private bool _isClosing;

        public EditCustomerTimeEntryTypesView()
        {
            InitializeComponent();
            InternalCommands.EditCustomerTimeEntryTypesCompleted.RegisterCommand(new DelegateCommand<object>(EditCompleted));
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion

        private void EditCompleted(object obj)
        {
            if (!_isClosing)
            {
                Close();
                _isClosing = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}