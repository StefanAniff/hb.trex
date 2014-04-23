using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Interfaces;
using Trex.Infrastructure.Commands;

namespace TrexSL.Shell.Login
{
    public partial class LoginView : ChildWindow, IView
    {
        private readonly DelegateCommand<object> _loginSuccess;

        public LoginView()
        {
            InitializeComponent();
            Loaded += LoginView_Loaded;
            KeyUp += LoginView_KeyUp;

            _loginSuccess = new DelegateCommand<object>(LoginSuccess);
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationCommands.LoginSucceeded.RegisterCommand(_loginSuccess);
            txtUserName.UpdateLayout();
            txtUserName.Focus();
        }

        private void LoginSuccess(object obj)
        {
            ApplicationCommands.LoginSucceeded.UnregisterCommand(_loginSuccess);
            Close();
        }

        private void textBoxGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox) e.OriginalSource).SelectAll();
        }

        private void LoginView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //var binding =txtPassword.GetBindingExpression(PasswordBox.PasswordProperty);

                var binding = txtCustomerId.GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();

                //OKButton.Focus();
                // Create a ButtonAutomationPeer using the ClickButton

                var buttonAutoPeer = new ButtonAutomationPeer(OKButton);

                // Create an InvokeProvider

                var invokeProvider = buttonAutoPeer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;

                // Invoke the default event, which is click for the button

                invokeProvider.Invoke();
            }
        }
    }
}