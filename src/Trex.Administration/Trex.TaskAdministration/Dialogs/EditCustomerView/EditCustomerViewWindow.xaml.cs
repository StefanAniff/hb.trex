using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.Dialogs.EditCustomerView
{
    public partial class EditCustomerViewWindow : ChildWindow, IView
    {
      
        private DelegateCommand<object> _editCompleted;
       

        public EditCustomerViewWindow()
        {
            _editCompleted = new DelegateCommand<object>(EditCompleted);
            InitializeComponent();

            this.GotFocus += new System.Windows.RoutedEventHandler(EditCustomerViewWindow_GotFocus);
            InternalCommands.CustomerEditCompleted.RegisterCommand(_editCompleted);
            InternalCommands.CustomerAddCompleted.RegisterCommand(_editCompleted);
        }

        void EditCustomerViewWindow_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            this.GotFocus -= EditCustomerViewWindow_GotFocus;
            txtCustomerName.Focus();
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
           
            InternalCommands.CreateTimeEntryTypeCompleted.UnregisterCommand(_editCompleted);
            InternalCommands.EditTimeEntryTypeCompleted.UnregisterCommand(_editCompleted);
            Close();
          
        }
    }
}