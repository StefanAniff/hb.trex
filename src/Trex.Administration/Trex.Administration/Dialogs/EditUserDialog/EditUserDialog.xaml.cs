using Microsoft.Practices.Prism.Commands;
using Trex.Administration.Commands;
using Trex.Core.Interfaces;

namespace Trex.Administration.Dialogs.EditUserDialog
{
    public partial class EditUserDialog : IView
    {
        

        public EditUserDialog()
        {
            InitializeComponent();
            InternalCommands.CreateNewUserCompleted.RegisterCommand(new DelegateCommand<object>(CreateUserCompleted));
            this.GotFocus += new System.Windows.RoutedEventHandler(EditUserDialog_GotFocus);
        }

        void EditUserDialog_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            this.GotFocus -= EditUserDialog_GotFocus;
            UserName.Focus();
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion

        private void CreateUserCompleted(object obj)
        {
            this.ViewModel.Close();
                Close();
        }
    }
}