using System.Collections.Generic;
using System.Linq;
using Trex.SmartClient.Core.Implemented;

namespace Trex.SmartClient.Forecast.ForecastRegistration
{
    public class ProjectRegistrations : ObservableCollectionExtended<ProjectRegistration>
    {
        public override string ToString()
        {
            return string.Format("{0} {1} count", GetType(), Count);
        }

        public IEnumerable<ProjectHourRegistration> ProjectHourRegistraionsWithValue 
        {
            get
            {
                return this
                    .SelectMany(x => x.Registrations)
                    .Where(y => y.IsNotZero);
            }
        }

        protected override void RemoveItem(int index)
        {
            var toRemove = this[index];
            base.RemoveItem(index);

            foreach (var hourRegistration in toRemove.Registrations)
            {
                hourRegistration.DateColumn.RemoveProjectHourRegistration(hourRegistration);
            }
        }

    }
}