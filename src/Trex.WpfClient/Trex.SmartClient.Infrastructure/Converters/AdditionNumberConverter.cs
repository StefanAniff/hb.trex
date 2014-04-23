using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Trex.SmartClient.Infrastructure.Converters
{
    public class AdditionNumberConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stringFormat = parameter.ToString();

            decimal parsedValue;
            Decimal.TryParse(value.ToString(), out parsedValue);

            var formattedResult = parsedValue.ToString(stringFormat);
            if (parsedValue > 0)
            {
                return string.Format("+{0}", formattedResult);
            }

            return formattedResult;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
