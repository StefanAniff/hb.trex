using System.Windows;
using System.Windows.Controls;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Dialogs
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)e.OriginalSource).SelectAll();
        }

        private LoginViewModel LoginViewModel
        {
            get { return (LoginViewModel)DataContext; }
        }
    }
}
