using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using System.Linq;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Forecast.ForecastRegistration.ValidationRules;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    public class ForecastTypeRegistration : ValidationEnabledViewModelBase
    {
        private ForecastType _selectedForecastType;
        private bool _showName;
        private readonly Subject<ForecastTypeRegistration> _showNameTimer = new Subject<ForecastTypeRegistration>();
        private readonly Subject<object> _forecastTypeRegistrationChanged = new Subject<object>();
        private decimal? _dedicatedHours;

        public int Id { get; set; }        
        public IList<ForecastType> ForecastTypes { get; set; }
        public ForecastRegistrationDateColumn DateColumn { get; set; }

        #region Commands

        public DelegateCommand<object> ForecastTypeClickCommand { get; set; }
        public DelegateCommand<object> CopyForwardCommand { get; set; }
        public DelegateCommand<object> CopyBackwardsCommand { get; set; }
        public DelegateCommand<object> CopyToAllCommand { get; set; }

        #endregion        

        public ForecastTypeRegistration(ForecastType selectedForecastType, IList<ForecastType> presenceTypes)
        {
            _selectedForecastType = selectedForecastType;
            ForecastTypes = presenceTypes;

            Initialize();
        }

        private void Initialize()
        {
            AddValidationRule(() => DedicatedHours, new RequiredDedicatedHoursValidationRule());

            ForecastTypeClickCommand = new DelegateCommand<object>(PresenceClickExecute);

            // Timer for setting ShowName to true for a limited time
            _showNameTimer
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .ObserveOnDispatcher()
                .Subscribe(x => { x.ShowName = false; });
        }

        private void PresenceClickExecute(object obj)
        {
            // Cycle through presencetypes

            // If last is selected, then select first
            if (SelectedForecastType == null || ForecastTypes.IndexOf(SelectedForecastType) == ForecastTypes.Count - 1)
            {
                SelectedForecastType = ForecastTypes.FirstOrDefault();
                return;
            }

            // Select next in list
            SelectedForecastType = ForecastTypes[ForecastTypes.IndexOf(SelectedForecastType) + 1];
        }

        public decimal? DedicatedHours
        {
            get { return _dedicatedHours; }
            set
            {
                _dedicatedHours = value;
                OnPropertyChanged(() => DedicatedHours);
                ForecastTypeRegistrationChanged.OnNext(this);
            }
        }

        public Brush Color
        {
            get
            {
                return SelectedForecastType == null
                           ? null
                           : SelectedForecastType.Color;
            }
        }

        public string Letter
        {
            get
            {
                return SelectedForecastType == null ? null : SelectedForecastType.Letter;
            }
        }

        public string ForecastTypeName
        {
            get { return SelectedForecastType == null ? null : SelectedForecastType.Name; }
        }        

        public ForecastType SelectedForecastType
        {
            get { return _selectedForecastType; }
            set
            {
                EnsureIsWorkDay();

                _selectedForecastType = value;
                OnForecastTypePropertyChanged();
                DoShowName();
            }
        }

        public bool SupportsDedicatedHours
        {
            get { return (SelectedForecastType != null && SelectedForecastType.SupportsDedicatedHours); }
        }

        private void EnsureIsWorkDay()
        {
            if (!DateColumn.IsWorkDay)
                throw new Exception(
                    string.Format(
                        "Cannot set SelectedForecastType on {0}, since it is not a workday! IsWeekend {1}, IsHoliday {2}",
                        DateColumn.Date, DateColumn.IsWeekend, DateColumn.IsHoliday));
        }

        private void OnForecastTypePropertyChanged()
        {
            OnPropertyChanged(() => SelectedForecastType);
            OnPropertyChanged(() => Color);
            OnPropertyChanged(() => Letter);
            OnPropertyChanged(() => ForecastTypeName);

            UpdateDedicatedHours();

            OnPropertyChanged(() => SupportsDedicatedHours);
            DateColumn.CalculateTotal();
            EnableClientHours();
            ForecastTypeRegistrationChanged.OnNext(this);
        }

        private void UpdateDedicatedHours()
        {
            /* Reset dedicated if hours if not supported by forcasttype
             * IVA: Refactor. Not using setter on property to avoid firing ForecastTypeRegistrationChanged
             **/
            if (SelectedForecastType != null && !SelectedForecastType.SupportsDedicatedHours)
            {
                _dedicatedHours = null;
            }
            else
            {
                _dedicatedHours = 0;
            }
            OnPropertyChanged(() => DedicatedHours);
        }       

        /// <summary>
        /// To avoid presence type name poping up when direct setter is used
        /// </summary>
        public virtual void SilentStatusSetById(int id)
        {
            _selectedForecastType = ForecastTypes.SingleOrDefault(x => x.Id == id);
            OnForecastTypePropertyChanged();
        }

        public bool ShowName
        {
            get { return _showName; }
            set
            {
                _showName = value;
                OnPropertyChanged(() => ShowName);
            }
        }

        private void DoShowName()
        {
            // Only show name for x milliseconds
            ShowName = true;
            _showNameTimer.OnNext(this);
        }

        public void EnableClientHours()
        {
            if (SelectedForecastType == null)
                return;

            SelectedForecastType.EnableHours(DateColumn.ProjectHours);
        }

        protected override void Dispose(bool disposing)
        {
            _showNameTimer.Dispose();
            _forecastTypeRegistrationChanged.Dispose();

            base.Dispose(disposing);
        }
        
        public bool IsEnabled
        {
            get { return !DateColumn.IsHoliday && !DateColumn.IsWeekend; }
        }

        public ISubject<object> ForecastTypeRegistrationChanged
        {
            get { return _forecastTypeRegistrationChanged; }
        }
    }
}