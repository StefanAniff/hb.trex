using System.Windows.Controls;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.LoginStatusView
{
    /// <summary>
    /// Interaction logic for LoginStatusView.xaml
    /// </summary>
    public partial class LoginStatusView : UserControl, IView
    {
        public LoginStatusView()
        {
            InitializeComponent();
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }
    }
}
