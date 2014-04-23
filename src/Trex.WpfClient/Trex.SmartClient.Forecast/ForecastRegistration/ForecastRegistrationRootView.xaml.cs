using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    /// <summary>
    /// Interaction logic for ForecastRegistrationRootView.xaml
    /// </summary>
    public partial class ForecastRegistrationRootView : IForecastRegistrationRootView
    {
        private double _tabStatisticsLastHeight = 233;
        private const double TabStatisticsMinimumHeight = 33;

        public ForecastRegistrationRootView()
        {
            InitializeComponent();
            ApplicationCommands.ToggleStatistics.RegisterCommand(new DelegateCommand<object>(ToggleStatistics));
        }

        public void ApplyViewModel(IViewModel viewModel)
        {
            DataContext = viewModel;
        }

        private void ToggleStatistics(object obj)
        {
            if (_tabRowDefinition.ActualHeight > TabStatisticsMinimumHeight)
            {
                _tabStatisticsLastHeight = _tabRowDefinition.ActualHeight;
                _tabRowDefinition.Height = new GridLength(TabStatisticsMinimumHeight, GridUnitType.Pixel);
            }
            else
            {
                _tabRowDefinition.Height = new GridLength(_tabStatisticsLastHeight, GridUnitType.Pixel);
            }
        }
    }
}
