using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Forecast.ForecastStatistics
{
    public interface IForecastStatisticsTabViewModel : IViewModel
    {
        void Initialize();
        ForecastStatisticsDto Statistics { get; set; }
        int CurrentYear { get; }
        string Next12MonthsDateSpanString { get; }
        string CurrentVacationPeriodString { get; }
        string NextVacationperiodString { get; }
        bool IsBusy { get; set; }
    }
}