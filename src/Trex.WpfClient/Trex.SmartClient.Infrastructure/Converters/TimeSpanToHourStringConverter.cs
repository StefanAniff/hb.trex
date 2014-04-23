using System;
using System.Globalization;
using System.Windows.Data;

namespace Trex.SmartClient.Infrastructure.Converters
{
    public class TimeSpanToHourStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpanToConvert = (TimeSpan) value;

            if (parameter == null)
                parameter = "string";


            switch (parameter.ToString())
            {
                case "string":
                    return timeSpanToConvert.TotalHours.ToString("N2");
                case "double":
                    return timeSpanToConvert.TotalHours;
                default:
                    return timeSpanToConvert.TotalHours;

            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueToConvert = value.ToString();
            double convertedDouble;
            double.TryParse(valueToConvert, out convertedDouble);

            return TimeSpan.FromHours(convertedDouble);
        }
    }
}