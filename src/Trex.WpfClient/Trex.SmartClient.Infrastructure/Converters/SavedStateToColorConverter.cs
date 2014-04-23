using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Trex.SmartClient.Infrastructure.Converters
{
    public class SavedStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var saved = (bool) value;
            return saved
                       ? new SolidColorBrush(new Color {R = 190, G = 191, B = 191, A = 255})
                       : new SolidColorBrush(new Color {R = 156, G = 46, B = 46, A = 255});
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
