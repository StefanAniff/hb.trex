using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.Roles.Commands;

namespace Trex.Roles.Dialogs.CreateNewRoleDialog
{
    public partial class CreateNewRoleDialog : ChildWindow, IView
    {
        private bool _isClosing;

        public CreateNewRoleDialog()
        {
            InitializeComponent();
            InternalCommands.CreateNewRoleCompleted.RegisterCommand(new DelegateCommand<object>(CreateNewRoleCompleted));
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion

        private void CreateNewRoleCompleted(object obj)
        {
            if (!_isClosing)
            {
                Close();
                _isClosing = true;
            }
        }
    }
}