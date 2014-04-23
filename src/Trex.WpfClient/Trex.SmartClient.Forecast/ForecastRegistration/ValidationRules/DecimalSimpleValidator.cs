using System.Globalization;
using System.Windows.Controls;

namespace Trex.SmartClient.Forecast.ForecastRegistration.ValidationRules
{
    /// <summary>
    /// Decimal validator used in xaml binding.
    /// Simple -> Only used in view/xaml. (not usable in viewmodels)
    /// Enables validation with ValidatesOnNotifyDataErrors="True" 
    /// and registration under Binding.ValidationRules
    /// </summary>
    public class DecimalSimpleValidator : ValidationRule
    {
        private decimal _max = decimal.MaxValue;
        private decimal _min = decimal.MinValue;

        public decimal Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public decimal Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            decimal toTest;
            if (!decimal.TryParse(value as string, out toTest)) 
                return new ValidationResult(true, null);

            if (toTest > _max)
                return new ValidationResult(false, string.Format("Maximum value allowed is {0}", Max));

            if (toTest < _min)
                return new ValidationResult(false, string.Format("Minimum value allowed is {0}", Min));

            return new ValidationResult(true, null);
        }
    }
}