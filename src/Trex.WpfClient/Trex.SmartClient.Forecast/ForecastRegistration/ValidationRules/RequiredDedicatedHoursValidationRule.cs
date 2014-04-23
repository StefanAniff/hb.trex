using System.ComponentModel.DataAnnotations;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Forecast.ForecastRegistration.ValidationRules
{
    public class RequiredDedicatedHoursValidationRule : IViewModelValidationRule
    {
        public ValidationResult ValidateViewModel(IViewModel viewModel, string propertyName, object value)
        {
            var vm = viewModel as ForecastTypeRegistration;
            if (vm == null || vm.SelectedForecastType == null || !vm.SelectedForecastType.SupportsDedicatedHours)
                return null;

            if (!vm.DedicatedHours.HasValue || vm.DedicatedHours.Value < 1)
                return new ValidationResult("Hours is required for selected status");

            return null;
        }
    }
}