using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast.ForecastOverview
{
    public class OverviewListOptions : ViewModelBase
    {
        public OverviewListOptions()
        {
            ForecastTypeOptions = new ObservableCollection<OverviewForecastTypeOption>();
        }

        public ObservableCollection<OverviewForecastTypeOption> ForecastTypeOptions { get; set; }

        public void InitializeForecastTypes(List<ForecastType> forecastTypes)
        {
            ForecastTypeOptions.Clear();
            foreach (var forecastType in forecastTypes)
            {
                ForecastTypeOptions.Add(new OverviewForecastTypeOption(forecastType));
            }
        }
    }

    public class OverviewForecastTypeOption : ViewModelBase
    {
        private readonly ForecastType _forecastType;
        private bool _isSelected;

        public OverviewForecastTypeOption(ForecastType forecastType)
        {
            _forecastType = forecastType;
            IsSelected = true;
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(() => IsSelected);
                ForecastLocalCompositeCommands.ForecastOverviewToggleForecatTypeHide.Execute(this);
            }
        }

        public string Name { get { return ForecastType.Name; } }
        public Brush Color { get { return ForecastType.Color; } }
        public ForecastType ForecastType { get { return _forecastType; } }
    }
}