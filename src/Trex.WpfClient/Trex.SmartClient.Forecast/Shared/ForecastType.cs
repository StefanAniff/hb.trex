using System.Collections.Generic;
using System.Windows.Media;
using Trex.SmartClient.Forecast.ForecastRegistration;

namespace Trex.SmartClient.Forecast.Shared
{
    public class ForecastType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ColorStringHex { get; set; }
        public bool SupportsProjectHours { get; set; }
        public bool SupportsDedicatedHours { get; set; }

        public bool SupportsProjectHoursOnly
        {
            get { return SupportsProjectHours && !SupportsDedicatedHours; }
        }

        public virtual string Letter
        {
            // Assuming for now that all first letters in ForecastTypes are distinct
            get { return Name.Substring(0, 1); }
        }

        public void EnableHours(IEnumerable<HourRegistration> hourRegistrations)
        {
            foreach (var hourRegistration in hourRegistrations)
            {
                hourRegistration.IsEditEnabled = SupportsProjectHours;
            }
        }

        public Brush Color
        {
            get
            {
                return string.IsNullOrEmpty(ColorStringHex)
                    ? null
                    : (Brush)new BrushConverter().ConvertFrom(ColorStringHex);
            }
        }

        #region Equality members

        protected bool Equals(ForecastType other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ForecastType)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        #endregion
    }
}