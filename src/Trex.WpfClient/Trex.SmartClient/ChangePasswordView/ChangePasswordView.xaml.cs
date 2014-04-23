using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.ChangePasswordView
{
    /// <summary>
    /// Interaction logic for ChangePasswordView.xaml
    /// </summary>
    public partial class ChangePasswordView : Window,IView
    {

        public ChangePasswordView()
        {
            InitializeComponent();
            pwdNew1.KeyUp += pwdNew1_KeyUp;
            pwdNew2.KeyUp += pwdNew2_KeyUp;
            pwdOld.KeyUp += pwdOld_KeyUp;
            ChangePasswordSucceedetCommand = new DelegateCommand<object>(ChangePasswordSucceeded);
            ApplicationCommands.ChangePasswordSucceeded.RegisterCommand(ChangePasswordSucceedetCommand);
        }

        private DelegateCommand<object> ChangePasswordSucceedetCommand { get; set; }

        private void ChangePasswordSucceeded(object obj)
        {
            ApplicationCommands.ChangePasswordSucceeded.UnregisterCommand(ChangePasswordSucceedetCommand);
            if (MessageBox.Show("Your password has been changed.", "Password changed", MessageBoxButton.OK) == MessageBoxResult.OK)
                this.DialogResult = true;
        }

        void pwdOld_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ((ChangePasswordViewModel)DataContext).OldPassword = pwdOld.Password;
        }

        void pwdNew2_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ((ChangePasswordViewModel)DataContext).NewPassword2 = pwdNew2.Password;
        }

        void pwdNew1_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ((ChangePasswordViewModel) DataContext).NewPassword1 = pwdNew1.Password;

        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        private void CancelDialog(object sender, RoutedEventArgs e)
        {
            ApplicationCommands.ChangePasswordSucceeded.UnregisterCommand(ChangePasswordSucceedetCommand);
            this.DialogResult = false;
            
        }
    }
}
