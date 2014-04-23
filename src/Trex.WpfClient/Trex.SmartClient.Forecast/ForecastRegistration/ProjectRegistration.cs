using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Implemented;
using System.Linq;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    public class ProjectRegistration : ViewModelBase
    {
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public string ComapyName { get; set; }
        private ObservableCollection<ProjectHourRegistration> _registrations;
        public DelegateCommand<string> UpdateProjectHoursByTotalCommand { get; private set; }

        public ProjectRegistration()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            UpdateProjectHoursByTotalCommand = new DelegateCommand<string>(TryDistributeTotalInput);
        }

        public void TryDistributeTotalInput(string totalString)
        {
            if (_registrations.Count == 0 || !_registrations.Any(x => x.IsEditableWorkDay))
                return;

            decimal totalInt;
            if (!decimal.TryParse(totalString, out totalInt))
                return;

            var toUpdate = AllEditableProjectHourRegistrations.ToList();

            // Distribute hours on types supporting projecthours only.
            DistributeHoursOnProject(toUpdate, totalInt);
        }

        private static void DistributeHoursOnProject(IEnumerable<ProjectHourRegistration> toUpdate, decimal totalInt)
        {
            var supportsProjectHoursOnly = toUpdate.Where(x => x.SelectedPresencetypeSupportsProjectHours()).ToList();
            var divisor = supportsProjectHoursOnly.Count;
            if (divisor == 0)
            {
                return;
            }

            // Only whole and halfs are interesting for now. E.g. we don't want 7,03 in hours
            var newDayHours = Math.Round(totalInt * 2 / divisor, MidpointRounding.AwayFromZero) / 2;
            foreach (var hourRegistration in supportsProjectHoursOnly)
            {
                hourRegistration.Hours = newDayHours;
            }
        }

        public ObservableCollection<ProjectHourRegistration> Registrations
        {
            get { return _registrations; }
            set
            {
                _registrations = value;
                OnPropertyChanged(() => Registrations);
            }
        }

        public void ApplyHoursForward(ProjectHourRegistration source)
        {
            var toUpdate = AllEditableProjectHourRegistrations.Where(x => x.DateColumn.Date > source.DateColumn.Date);
            foreach (var registration in toUpdate)
            {
                registration.Hours = source.Hours;
            }
        }

        public void ApplyHoursBackward(ProjectHourRegistration source)
        {
            var toUpdate = AllEditableProjectHourRegistrations.Where(x => x.DateColumn.Date < source.DateColumn.Date);
            foreach (var registration in toUpdate)
            {
                registration.Hours = source.Hours;
            }
        }

        public void ApplyHoursToAll(ProjectHourRegistration source)
        {
            foreach (var registration in AllEditableProjectHourRegistrations)
            {
                registration.Hours = source.Hours;
            }
        }

        private IEnumerable<ProjectHourRegistration> AllEditableProjectHourRegistrations
        {
            get
            {
                return Registrations.Where(x => x.IsEditableWorkDay);
            }
        }

        public void ResetHoursUpdatedSubscriptions()
        {
            foreach (var hourRegistration in Registrations)
            {
                hourRegistration.ResetHoursUpdatedSubscriptions();
            }
        }

        [NoDirtyCheck]
        public decimal Total
        {
            get
            {
                return Registrations == null ? 0 : Registrations.Sum(x => x.EnabledHours);
            }
            set
            {
                var _ = value; // Total is calculated. We just need to raise OnPropertyChanged
                OnPropertyChanged(() => Total);
            }
        }

        public void InvokeTotalPropertyChanged()
        {
            OnPropertyChanged(() => Total);            
        }

        protected override void Dispose(bool disposing)
        {
            if (Registrations != null)
            {
                foreach (var hourRegistration in Registrations)
                {
                    hourRegistration.Dispose();
                }    
            }            

            base.Dispose(disposing);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", GetType(), ProjectId, ProjectName);
        }
    }   
}