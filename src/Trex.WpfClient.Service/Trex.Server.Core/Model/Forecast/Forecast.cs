using System;
using System.Collections.Generic;
using System.Linq;

namespace Trex.Server.Core.Model.Forecast
{
    public class Forecast : EntityBase
    {
        private readonly ForecastType _forecastType;
        private readonly DateTime _date;
        private readonly decimal? _dedicatedForecastTypeHours;
        private readonly IList<ForecastProjectHours> _projectRegistrations = new List<ForecastProjectHours>();
        private readonly ForecastMonth _forecastMonth;

        public virtual int Id { get; set; }

        protected Forecast() { }

        public Forecast(DateTime date, ForecastType forecastType, ForecastMonth forecastMonth, decimal? dedicatedHours = null)
        {
            if (forecastType == null)
                throw new Exception("ForecastType can't be null");

            if (forecastMonth == null)
                throw new Exception("ForecastMonth can't be null");

            _forecastMonth = forecastMonth;
            _forecastType = forecastType;
            _date = date;

            EnsureSupportsDedicatedHours(forecastType, dedicatedHours);
            _dedicatedForecastTypeHours = dedicatedHours;
        }

        public virtual DateTime Date
        {
            get { return _date; }            
        }

        public override int EntityId
        {
            get { return Id; }
        }

        /// <summary>
        /// Optional depending on ForecastType
        /// </summary>
        public virtual IEnumerable<ForecastProjectHours> ProjectRegistrations
        {
            get { return _projectRegistrations; }
        }

        /// <summary>
        /// Optional depending on ForecastType
        /// </summary>
        public virtual decimal? DedicatedForecastTypeHours
        {
            get { return _dedicatedForecastTypeHours; }
        }

        public virtual ForecastType ForecastType
        {
            get { return _forecastType; }
        }

        public virtual ForecastMonth ForecastMonth
        {
            get { return _forecastMonth; }
        }


        public virtual void ClearProjectRegistrations()
        {
            _projectRegistrations.Clear();
        }

        public virtual void AddProjectRegistration(Project project, decimal hours)
        {            
            AddProjectRegistration(new ForecastProjectHours(project, hours, this));
        }

        public virtual void AddProjectRegistration(ForecastProjectHours registration)
        {
            EnsureSupportsProjectHours(registration);
            EnsureDistinctProject(registration.Project);
            _projectRegistrations.Add(registration);
        }

        private void EnsureSupportsProjectHours(ForecastProjectHours registration)
        {
            if (!ForecastType.SupportsProjectHours)
                throw new Exception(string.Format("{0} ForecastType on date: {1} and project: {2} does not support project hours {3}"
                                                    , ForecastType.Name, Date.ToShortDateString()
                                                    , registration.Project.ProjectName
                                                    , registration.Hours));
        }

        private void EnsureDistinctProject(Project project)
        {
            if (_projectRegistrations.Any(x => x.Project.ProjectID.Equals(project.ProjectID)))
                throw new Exception(string.Format("There can only be one ForecastProjectHours project: {0} pr ProjectForecast", project.ProjectName));
        }

        private void EnsureSupportsDedicatedHours(ForecastType forecastType, decimal? dedicatedHours)
        {
            if (!forecastType.SupportsDedicatedHours && dedicatedHours.HasValue)
                throw new Exception(string.Format("{0} ForecastType on date {1} does not support dedicated hours {2}", forecastType.Name, Date.ToShortDateString(), dedicatedHours));                                
            
        }

        public virtual void RemoveProjectRegistration(ForecastProjectHours registration)
        {
            _projectRegistrations.Remove(registration);
        }
    }
}