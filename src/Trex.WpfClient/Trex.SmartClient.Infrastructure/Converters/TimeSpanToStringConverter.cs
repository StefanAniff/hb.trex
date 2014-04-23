using System;
using System.Globalization;
using System.Windows.Data;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Infrastructure.Extensions;

namespace Trex.SmartClient.Infrastructure.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpanToConvert = (TimeSpan) value;
            return timeSpanToConvert.ToTimeString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}