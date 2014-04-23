using Microsoft.Practices.Prism.Commands;

namespace Trex.SmartClient.Forecast.Shared
{
    public static class ForecastLocalCompositeCommands
    {
        // Registration
        public static CompositeCommand ForecastRegistrationProjectSelected = new CompositeCommand();

        // Overview
        public static CompositeCommand ForecastOverviewToggleForecatTypeHide = new CompositeCommand();
    }
}