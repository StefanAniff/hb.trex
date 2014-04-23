using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Forecast.ForecastOverview.Helpers;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast.ForecastOverview
{
    public class ForecastOverviewForecast : ViewModelBase
    {
        public ForecastOverviewForecast()
        {
            Projects = new List<ForecastOverviewProjectHours>();
        }

        public int ForecastId { get; set; }
        public ForecastDate Date { get; set; }
        public virtual ForecastType ForecastType { get; set; }
        public decimal? DedicatedForecastTypeHours { get; set; }
        public IList<ForecastOverviewProjectHours> Projects { get; set; }
        public virtual ForecastOverviewDisplayHandlerBase DisplayHandler { get; set; }
        public Brush Color { get { return ForecastType == null ? null : ForecastType.Color; } }

        public string Description
        {
            get
            {
                if (ForecastType == null)
                    return "No registration";

                var msg = new StringBuilder().Append(ForecastType.Name);
                if (DedicatedForecastTypeHours.HasValue)
                    msg.Append(string.Format(" - {0:N2}", DedicatedForecastTypeHours.Value));

                msg.AppendLine();

                if (Projects == null || !Projects.Any())
                {
                    return msg.ToString().TrimEnd();                    
                }

                foreach (var project in Projects)
                {
                    msg.AppendLine(string.Format("{0} - {1:N2}", project.ProjectName, project.Hours));
                }
                return msg.ToString().TrimEnd();
            }
        }

        public virtual void UpdateVisuals(OverviewForecastTypeOption typeOption)
        {
            DisplayHandler.UpdateVisuals(typeOption);
        }

        public decimal SumAllProjectHours
        {
            get
            {
                return Projects == null ? decimal.Zero : Projects.Sum(x => x.Hours);
            }
        }

        public decimal SumHoursByProjectId(int projectId)
        {
            return Projects == null ? decimal.Zero : Projects.Where(x => x.ProjectId == projectId).Sum(y => y.Hours);
        }

        public decimal SumHoursByCompanyId(int companyId)
        {
            return Projects == null ? decimal.Zero : Projects.Where(x => x.CompanyId == companyId).Sum(y => y.Hours);
        }

        public decimal DedicatedHours
        {
            get { return DedicatedForecastTypeHours.HasValue ? DedicatedForecastTypeHours.Value : decimal.Zero; }
        }

        public virtual bool HasNoProjects
        {
            get { return Projects == null || Projects.Count == 0; }
        }

        public virtual bool IsWorkDay
        {
            get { return Date != null && Date.IsWorkDay; }   
        }
    }
}