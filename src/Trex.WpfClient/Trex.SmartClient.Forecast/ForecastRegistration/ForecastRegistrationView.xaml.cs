using System.Windows.Controls;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    /// <summary>
    /// Interaction logic for ForecastRegistrationView.xaml
    /// </summary>
    public partial class ForecastRegistrationView : UserControl, IForecastRegistrationView
    {
        public ForecastRegistrationView()
        {
            InitializeComponent();            
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }                
    }        
}
