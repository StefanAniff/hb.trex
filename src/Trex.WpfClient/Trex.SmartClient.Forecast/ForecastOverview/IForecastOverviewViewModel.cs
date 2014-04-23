using System;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast.ForecastOverview
{
    public interface IForecastOverviewViewModel : IViewModel
    {
        void Initialize();
        bool IsBusy { get; set; }
        DelegateCommand<object> NextMonthCommand { get; }
        DelegateCommand<object> PreviousMonthCommand { get; }
        DelegateCommand<object> CurrentMonthCommand { get; }
       
        DateTime SelectedDate { get; set; }
        string SelectedMonthString { get; }
        ForecastDates Dates { get; set; }
        ForecastOverviewForecastMonths UserRegistrations { get; set; }
        OverviewListOptions ListOptions { get; set; }
        ForecastOverviewSearchOptions SearchOptions { get; set; }
        DelegateCommand<object> SearchCommand { get; }
        DelegateCommand<object> ClearAllCommand { get; }
        DelegateCommand<object> PrintCommand { get; }
    }
}