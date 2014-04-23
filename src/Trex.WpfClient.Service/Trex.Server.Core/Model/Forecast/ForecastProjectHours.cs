namespace Trex.Server.Core.Model.Forecast
{
    /// <summary>
    /// Childitem for ClientForecasts
    /// </summary>
    public class ForecastProjectHours
    {
        protected ForecastProjectHours()
        {
        }

        public virtual int Id { get; set; }

        /// <summary>
        /// Company which hours are registered to
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// Hours registered for project property.
        /// (In context with ClientForecast)
        /// </summary>
        public virtual decimal Hours { get; set; }       

        /// <summary>
        /// ClientForecast which is parant for this client hours
        /// </summary>
        public virtual Forecast Parent { get; set; }

        public ForecastProjectHours(Project project, decimal hours, Forecast parent)
        {
            Project = project;
            Hours = hours;
            Parent = parent;
        }
    }
}