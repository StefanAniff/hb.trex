using System;
using System.Reactive.Subjects;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Forecast.ForecastRegistration.ValidationRules;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    public class HourRegistration : ValidationEnabledViewModelBase
    {
        private decimal _hours;
        private Subject<ForecastRegistrationDateColumn> _hoursUpdated = new Subject<ForecastRegistrationDateColumn>();
        private bool _isEditEnabled = true;

        public virtual ForecastRegistrationDateColumn DateColumn { get; set; }

        public HourRegistration()
        {
            AddValidationRule(() => Hours, new TotalDateHoursValidationRule());
        }        

        public decimal Hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                OnPropertyChanged(() => Hours);
                OnHoursUpdated();
            }
        }

        public decimal EnabledHours
        {
            get
            {
                return IsEditEnabled ? Hours : 0;
            }
        }

        public bool IsNotZero
        {
            get { return EnabledHours != decimal.Zero; }
        }

        public ISubject<ForecastRegistrationDateColumn> HoursUpdated
        {
            get { return _hoursUpdated; }
        }

        public void ResetHoursUpdatedSubscriptions()
        {
            _hoursUpdated.Dispose();
            _hoursUpdated = new Subject<ForecastRegistrationDateColumn>();
        }

        public void InitializeIsEditEnabled(ForecastType forecastType)
        {
            _isEditEnabled = forecastType.SupportsProjectHours;
        }

        [NoDirtyCheck]
        public bool IsEditEnabled
        {
            get { return _isEditEnabled; }
            set
            {
                _isEditEnabled = value;
                OnPropertyChanged(() => IsEditEnabled);
                OnPropertyChanged(() => Hours);
                OnPropertyChanged(() => IsEditableWorkDay);
                if (!_isEditEnabled)
                    Hours = 0;
            }
        }

        [NoDirtyCheck]
        public bool IsEditableWorkDay
        {
            get { return _isEditEnabled && DateColumn.IsWorkDay; }
        }

        protected virtual void OnHoursUpdated()
        {
            HoursUpdated.OnNext(null);
        }

        public bool IsWeekend
        {
            get { return DateColumn.IsWeekend; }
        }

        protected override void Dispose(bool disposing)
        {
            _hoursUpdated.Dispose();
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", GetType(), DateColumn.Date.Date, Hours);
        }
    }    
}