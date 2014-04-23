using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.Roles.Commands;

namespace Trex.Roles.Dialogs.DeleteRoleDialog
{
    public partial class DeleteRoleDialog : ChildWindow, IView
    {
        private bool _isClosing;

        public DeleteRoleDialog()
        {
            InitializeComponent();
            InternalCommands.DeleteRoleCompleted.RegisterCommand(new DelegateCommand<object>(DeleteRoleCompleted));
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion

        private void DeleteRoleCompleted(object obj)
        {
            if (!_isClosing)
            {
                Close();
                _isClosing = true;
            }
        }
    }
}