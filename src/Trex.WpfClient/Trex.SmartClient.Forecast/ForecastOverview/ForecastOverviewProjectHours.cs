namespace Trex.SmartClient.Forecast.ForecastOverview
{
    public class ForecastOverviewProjectHours
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int CompanyId { get; set; }

        public decimal Hours { get; set; }
    }
}