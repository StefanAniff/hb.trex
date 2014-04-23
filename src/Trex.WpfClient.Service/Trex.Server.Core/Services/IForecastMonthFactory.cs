using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;

namespace Trex.Server.Core.Services
{
    public interface IForecastMonthFactory
    {
        ForecastMonth CreateForecastMonth(int month, int year, User user, User createdBy);
    }
}