using System.ComponentModel.DataAnnotations;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using System.Linq;

namespace Trex.SmartClient.Forecast.ForecastRegistration.ValidationRules
{
    public class TotalDateHoursValidationRule : IViewModelValidationRule
    {
        public ValidationResult ValidateViewModel(IViewModel viewModel, string propertyName, object value)
        {
            var vm = viewModel as HourRegistration;
            if (vm == null || !vm.DateColumn.IsWorkDay)
                return null;

            // If client hours exist, then nothing to validate (0 total is valid)
            if (!vm.DateColumn.HasProjectHours && !vm.DateColumn.ForecastTypeRegistration.SupportsDedicatedHours)
                return null;

            // No negatives plz...
            if (vm.Hours < 0)
                return new ValidationResult("Negative values are not appreciated");

            // Max 24 hours
            if (vm.Hours > 24) 
                return new ValidationResult("Max 24 hours in total for a given day is allowed");

            // If PresenceType is Client total cant be zero
            if (vm.DateColumn.ForecastType.SupportsProjectHoursOnly
                && vm.Hours == 0)
                return new ValidationResult(string.Format("When \"{0}\" is selected, total can't be zero", vm.DateColumn.ForecastType.Name));

            return null;
        }
    }
}