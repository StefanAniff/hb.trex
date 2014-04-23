using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Trex.SmartClient.Infrastructure.Converters
{
    public class DecimalZeroToEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            decimal dec;
            var parseOk = decimal.TryParse(value.ToString(), out dec);
            if (!parseOk)
                return null;

            if (dec == decimal.Zero)
                return string.Empty;

            var result = value.ToString();
            var decimalSep = culture.NumberFormat.NumberDecimalSeparator;

            if (result.Contains(decimalSep))
            {
                var wholeNumber = result.Substring(0, result.LastIndexOf(decimalSep, StringComparison.CurrentCulture));

                var allDecimalValues = result.Substring(result.LastIndexOf(decimalSep, StringComparison.CurrentCulture) + 1);
                var decimalsWithNoZeroesTrail = Regex.Replace(allDecimalValues, "0+$", String.Empty);

                if (!String.IsNullOrEmpty(decimalsWithNoZeroesTrail))
                {
                    result = wholeNumber + decimalSep + decimalsWithNoZeroesTrail;
                }
                else
                {
                    result = wholeNumber;
                }

                return result;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return value.ToString() == "" ? decimal.Zero : value;
        }
    }
}