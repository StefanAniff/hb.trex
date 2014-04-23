using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Infrastructure.Extensions;

namespace Trex.SmartClient.Infrastructure.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.
        ///                 </param><param name="targetType">The type of the binding target property.
        ///                 </param><param name="parameter">The converter parameter to use.
        ///                 </param><param name="culture">The culture to use in the converter.
        ///                 </param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTimeToConvert = (DateTime)value;

            var convertOption = (string) parameter;
            string returnValue = string.Empty;

            if (string.IsNullOrEmpty(convertOption))
                convertOption = "ShortDateTime";


            switch (convertOption)
            {
                case "ShortDate":
                    returnValue = dateTimeToConvert.ToShortDateString();
                    break;

                case "ShortDateAndTime":
                    returnValue = dateTimeToConvert.ToShortDateAndTimeString();
                    break;

                case "ShortTime":
                    returnValue = dateTimeToConvert.ToShortTimeString();
                    break;
                case "DayAndMonth":
                    returnValue = dateTimeToConvert.ToDayAndMonth();
                    break;
                    

                
                    
            }

            return returnValue;


        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.
        ///                 </param><param name="targetType">The type to convert to.
        ///                 </param><param name="parameter">The converter parameter to use.
        ///                 </param><param name="culture">The culture to use in the converter.
        ///                 </param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
