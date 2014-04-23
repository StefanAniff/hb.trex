using Microsoft.Practices.Prism.Commands;
using Trex.Administration.Commands;
using Trex.Core.Interfaces;

namespace Trex.Administration.Dialogs.InviteUsersDialog
{
    public partial class InviteUsersDialog
    {
        private bool _isClosing;

        public InviteUsersDialog()
        {
            InitializeComponent();

            InternalCommands.InviteUsersCompleted.RegisterCommand(new DelegateCommand<object>(InviteUsersCompleted));
        }

        private void InviteUsersCompleted(object obj)
        {
            if (!_isClosing)
            {
                Close();
                _isClosing = true;
            }
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }
    }
}