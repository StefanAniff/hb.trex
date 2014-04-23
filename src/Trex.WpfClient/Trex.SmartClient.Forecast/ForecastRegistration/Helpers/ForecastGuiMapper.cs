using System;
using System.Linq;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Core.Model;

//using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Forecast.ForecastRegistration.Helpers
{
    public class ForecastGuiMapper
    {
        public ForecastTypeRegistration PresenceRegistrationByDate(DateTime date, IForecastRegistrationViewModel vm)
        {
            return vm.PresenceRegistrations.Single(x => x.DateColumn.Date.Date == date.Date);
        }

        public void Map(ForecastDto src, IForecastRegistrationViewModel vm)
        {
            // Update corresponding PresenceType
            var presenceReg = PresenceRegistrationByDate(src.Date, vm);
            presenceReg.SilentStatusSetById(src.ForecastType.Id);
            presenceReg.Id = src.Id;
            presenceReg.DedicatedHours = src.DedicatedForecastTypeHours;

            // Update or create client registrations
            if (src.ForecastType.SupportsProjectHours)
            {
                foreach (var projectHoursDto in src.ForecastProjectHoursDtos)
                {                    
                    // If client not found create new
                    var toUpdate = vm.ProjectRegistrations.SingleOrDefault(x => x.ProjectId == projectHoursDto.Project.Id) ??
                                   vm.AddNewProjectRegistration(projectHoursDto.Project.Id, projectHoursDto.Project.Name, projectHoursDto.Project.CompanyDto.Name);

                    var dateHourReg = toUpdate.Registrations.SingleOrDefault(x => x.DateColumn.Date == src.Date);
                    if (dateHourReg != null)
                    {
                        dateHourReg.Hours = projectHoursDto.Hours;
                    }
                }
            }
        }
    }
}