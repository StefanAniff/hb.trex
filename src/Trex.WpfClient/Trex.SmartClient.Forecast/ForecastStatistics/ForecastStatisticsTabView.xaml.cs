using System.Windows.Controls;
using System.Windows.Input;
using Trex.SmartClient.Core.Interfaces;
using ApplicationCommands = Trex.SmartClient.Infrastructure.Commands.ApplicationCommands;

namespace Trex.SmartClient.Forecast.ForecastStatistics
{
    /// <summary>
    /// Interaction logic for ForecastStatisticsTabView.xaml
    /// </summary>
    public partial class ForecastStatisticsTabView : UserControl, IForecastStatisticsTabView
    {
        public ForecastStatisticsTabView()
        {
            InitializeComponent();
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        private void _tabGrid_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ApplicationCommands.ToggleStatistics.Execute(sender);
        }
    }
}
