using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Dialogs
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : Window, IView
    {
        public LoginDialog()
        {
            InitializeComponent();
            Trex.SmartClient.Infrastructure.Commands.ApplicationCommands.LoginSucceeded.RegisterCommand(new DelegateCommand<object>(LoginSucceedet));
        }

        private void LoginSucceedet(object obj)
        {
            this.Close();
        }


        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }
    }
}
