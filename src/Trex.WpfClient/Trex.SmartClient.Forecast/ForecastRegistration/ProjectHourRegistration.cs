using Microsoft.Practices.Prism.Commands;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    public class ProjectHourRegistration : HourRegistration
    {
        public ProjectRegistration Parent { get; set; }

        public DelegateCommand<object> CopyToFutureCommand { get; set; }
        public DelegateCommand<object> CopyToPastCommand { get; set; }
        public DelegateCommand<object> CopyToAllCommand { get; set; }

        public ProjectHourRegistration(ProjectRegistration parent)
        {
            Parent = parent;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CopyToFutureCommand = new DelegateCommand<object>(x => CopyHoursForward());
            CopyToPastCommand = new DelegateCommand<object>(x => CopyHoursBackwards());
            CopyToAllCommand = new DelegateCommand<object>(x => CopyHoursAll());
        }

        private void CopyHoursAll()
        {
            Parent.ApplyHoursToAll(this);
        }

        private void CopyHoursBackwards()
        {
            Parent.ApplyHoursBackward(this);
        }

        private void CopyHoursForward()
        {
            Parent.ApplyHoursForward(this);
        }

        protected override void OnHoursUpdated()
        {
            base.OnHoursUpdated();
            Parent.InvokeTotalPropertyChanged();
        }

        public bool PresenceSupportsProjectHours
        {
            get { return DateColumn.ForecastTypeRegistration.SelectedForecastType.SupportsProjectHours; }
        }

        public bool SelectedPresencetypeSupportsProjectHoursOnly()
        {
            if (DateColumn == null
                || DateColumn.ForecastTypeRegistration == null
                || DateColumn.ForecastTypeRegistration.SelectedForecastType == null)
                return false;

            return DateColumn.ForecastTypeRegistration.SelectedForecastType.SupportsProjectHoursOnly;
        }

        public bool SelectedPresencetypeSupportsProjectHours()
        {
            if (DateColumn == null
                || DateColumn.ForecastTypeRegistration == null
                || DateColumn.ForecastTypeRegistration.SelectedForecastType == null)
                return false;

            return DateColumn.ForecastTypeRegistration.SelectedForecastType.SupportsProjectHours;
        }

        public bool SelectedPresencetypeSupportsDedicatedHours
        {
            get
            {
                if (DateColumn == null
                || DateColumn.ForecastTypeRegistration == null
                || DateColumn.ForecastTypeRegistration.SelectedForecastType == null)
                    return false;

                return DateColumn.ForecastTypeRegistration.SelectedForecastType.SupportsDedicatedHours;
            }
        }

        public override string ToString()
        {
            var baseStr = base.ToString();
            return string.Format("{0}, {1}", Parent.ProjectName, baseStr);
        }
    }
}