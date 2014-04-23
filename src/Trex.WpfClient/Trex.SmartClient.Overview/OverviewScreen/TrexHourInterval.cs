//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Telerik.Windows.Controls.TimeBar;

//namespace Telerik.Windows.Controls.TimeBar
//{
//    public class TrexHourInterval : IntervalBase
//    {
//        public Func<DateTime, string>[] formatters { get; set; }
//        private const int Weight = 1350;
//        private const int RowCount = 6;

//        public TrexHourInterval()
//        {
//            formatters = new Func<DateTime, string>[]
//                {
//                    date => date.ToString("HH:mm:ss.fff"),
//                    date => date.ToString("HH:mm:ss"),
//                    date => date.ToString("mm:ss"),
//                    date => date.ToString("ss")
//                };
//        }

//        /// <summary>
//        /// Gets the code that identifies this interval uniquely.
//        /// </summary>
//        /// <value>The code.</value>
//        public int Code
//        {
//            get
//            {
//                return Weight;
//            }
//        }

//        /// <summary>
//        /// Gets a collection of formatters used to convert DateTime objects
//        /// to specific strings.
//        /// </summary>
//        /// <value>The formatters.</value>
//        public Func<DateTime, string>[] Formatters
//        {
//            get
//            {
//                return formatters;
//            }
//        }

//        /// <summary>
//        /// Gets the smallest period.
//        /// </summary>
//        /// <value>The smallest period.</value>
//        public TimeSpan SmallestPeriod
//        {
//            get
//            {
//                return TimeSpan.FromSeconds(10);
//            }
//        }

//        /// <summary>
//        /// Creates the string measurement table.
//        /// </summary>
//        /// <returns></returns>
//        public string[,] CreateStringMeasurementTable()
//        {
//            string[,] secondsList = new string[this.Formatters.Length, RowCount];

//            DateTime sampleDate = new DateTime(2000, 10, 30, 0, 0, 0);

//            for (int secondIndex = 0; secondIndex < RowCount; secondIndex++)
//            {
//                for (int formatIndex = 0; formatIndex < this.Formatters.Length; formatIndex++)
//                {
//                    secondsList[formatIndex, secondIndex] = this.Formatters[formatIndex](sampleDate);
//                }
//                sampleDate = sampleDate.AddSeconds(10);
//            }

//            return secondsList;
//        }

//        /// <summary>
//        /// Extracts the interval start from the specified DateTime object.
//        /// </summary>
//        /// <param name="date">The date.</param>
//        /// <returns></returns>
//        public DateTime ExtractIntervalStart(DateTime date)
//        {
//            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second - (date.Second % 10));
//        }

//        /// <summary>
//        /// Increments the specified date.
//        /// </summary>
//        /// <param name="date">The date.</param>
//        /// <returns></returns>
//        public DateTime IncrementByInterval(DateTime date)
//        {
//            return date.AddSeconds(10);
//        }
//    }
//}
