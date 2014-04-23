using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Core.Implemented;

namespace Trex.SmartClient.Forecast.Shared
{
    public class ForecastDate : ViewModelBase
    {
        private string _holidayDesciption;

        public ForecastDate(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; set; }
        public string ShortDayName { get { return Date.DayOfWeek.ToString().Substring(0, 3); } }
        public bool IsWeekend { get { return Date.IsWeekend(); } }
        public bool IsHoliday { get { return _holidayDesciption != null; } }
        public virtual bool IsWorkDay { get { return !IsWeekend && !IsHoliday; } }

        public string HolidayDesciption
        {
            get { return _holidayDesciption; }
        }

        public string WeekNumber
        {
            get
            {
                // Easy fix for now... only show weeknumber in wednesday column
                if (Date.DayOfWeek != DayOfWeek.Wednesday)
                    return string.Empty;

                return Date.WeeknumberDk().ToString(CultureInfo.InvariantCulture);
            }
        }

        public string Description
        {
            get { return HolidayDesciption ?? Date.ToShortDateString(); }
        }

        public void SetToHoliday(HolidayDto holiday)
        {
            if (holiday.Date != Date)
                throw new Exception(string.Format("Holiday {0} does not have same date as {1}", holiday.Description, Date.ToShortDateString()));

            _holidayDesciption = holiday.Description;
            OnPropertyChanged(() => HolidayDesciption);
            OnPropertyChanged(() => IsHoliday);
            OnPropertyChanged(() => IsWorkDay);
        }

        #region Equality members

        protected bool Equals(ForecastDate other)
        {
            return Date.Equals(other.Date);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ForecastDate)obj);
        }

        public override int GetHashCode()
        {
            return Date.GetHashCode();
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0} - {1}", Date, HolidayDesciption);
        }
    }

    public class ForecastDates : ObservableCollection<ForecastDate>
    {
        public ForecastDates()
        {
        }

        public ForecastDates(IEnumerable<ForecastDate> collection) : base(collection)
        {
        }
    }
}