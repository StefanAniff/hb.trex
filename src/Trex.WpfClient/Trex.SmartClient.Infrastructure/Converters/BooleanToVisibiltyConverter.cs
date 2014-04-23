using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Trex.SmartClient.Infrastructure.Converters
{
    public class BooleanToVisibiltyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolToConvert = (bool) value;

            if (boolToConvert)
            {
                return parameter == null ? Visibility.Visible : Visibility.Collapsed;
            }
            return parameter == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (Visibility) value;
            if (visibility == Visibility.Visible)
            {
                return parameter == null;
            }
            else
            {
                return parameter != null; ;
            }
        }
    }
}