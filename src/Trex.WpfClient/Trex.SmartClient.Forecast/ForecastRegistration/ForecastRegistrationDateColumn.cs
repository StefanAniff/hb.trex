using System;
using System.Collections.Generic;
using System.Linq;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    public class ForecastRegistrationDateColumn : ForecastDate
    {
        private ForecastTypeRegistration _forecastTypeRegistration;
        private HourRegistration _dateTotal;
        private readonly List<ProjectHourRegistration> _projectHours = new List<ProjectHourRegistration>();        

        public ForecastRegistrationDateColumn(DateTime date) : base(date) { }                                

        public virtual IEnumerable<ProjectHourRegistration> ProjectHours
        {
            get { return _projectHours; }
        }

        public virtual IEnumerable<ProjectHourRegistration> ProjectHoursWithValue
        {
            get { return _projectHours.Where(x => x.IsNotZero); }
        }

        public virtual bool HasProjectHours
        {
            get { return _projectHours.Count > 0; }
        }

        public virtual void RemoveProjectHourRegistration(ProjectHourRegistration hourRegistration)
        {
            _projectHours.Remove(hourRegistration);
        }

        public virtual ForecastTypeRegistration ForecastTypeRegistration
        {
            get { return _forecastTypeRegistration; }
            set
            {         
                _forecastTypeRegistration = value;
                _forecastTypeRegistration.DateColumn = this;
            }
        }

        public virtual ForecastType ForecastType
        {
            get { return ForecastTypeRegistration != null ? ForecastTypeRegistration.SelectedForecastType : null; }
        }

        public virtual decimal? SelectedForecastTypeDedicatedHours
        {
            get { return ForecastTypeRegistration != null ? ForecastTypeRegistration.DedicatedHours : null; }
        }

        public HourRegistration DateTotal
        {
            get { return _dateTotal; }
            set
            {
                _dateTotal = value;
                if (_dateTotal != null)
                    _dateTotal.DateColumn = this;
            }
        }

        public void AddProjectHours(ProjectHourRegistration projectHourRegistration)
        {
            projectHourRegistration.DateColumn = this; 
            _projectHours.Add(projectHourRegistration);
        }        

        public void CalculateTotal()
        {
            if (DateTotal == null)
                DateTotal = new HourRegistration();

            DateTotal.Hours = _projectHours.Sum(x => x.EnabledHours) + DedicatedForecastTypeHours;
        }

        private decimal DedicatedForecastTypeHours
        {
            get
            {
                if (ForecastTypeRegistration == null || !ForecastType.SupportsDedicatedHours)
                    return decimal.Zero;

                var dedicatedHours = ForecastTypeRegistration.DedicatedHours;
                return dedicatedHours.HasValue ? dedicatedHours.Value : decimal.Zero;
            }    
        }

        /// <summary>
        /// Indicates an empty project-status registration, with no project lines.
        /// Status is project, but no project-lines exists.
        /// </summary>
        /// <param name="projectForecastTypeId">Forecast type id that represents the Project forecasttype</param>
        /// <returns></returns>
        public bool IsEmptyProjectRegistration(int projectForecastTypeId)
        {
            if (ForecastType == null)
                return false;

            return (ForecastType.Id == projectForecastTypeId && !HasProjectHours);
        }
    }
}