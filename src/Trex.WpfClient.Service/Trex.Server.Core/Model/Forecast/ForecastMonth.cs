using System;
using System.Collections.Generic;

namespace Trex.Server.Core.Model.Forecast
{
    public class ForecastMonth : EntityBase
    {
        private readonly int _id;
        private readonly int _month;
        private readonly int _year;
        private readonly User _user;
        private bool _unLocked;
        private DateTime _lockedFrom;
        private readonly IList<Forecast> _forecasts = new List<Forecast>(); 

        #region Timestamp
        private User _createdBy;
        private DateTime _createdDate;
        #endregion

        protected ForecastMonth() { }

        public ForecastMonth(int month, int year, int nextMonthLockDay, User user, User createdBy)
        {
            if (user == null)
                throw new Exception("User can't be null");

            if (createdBy == null)
                throw new Exception("CreatedBy can't be null");

            if (month < 1 || month > 12)
                throw new Exception(string.Format("Month value {0} is invalid", month));

            if (year < 1)
                throw new Exception(string.Format("Year value {0} is invalid", year));

            if (nextMonthLockDay < 1)
                throw new Exception(string.Format("NextMonthLockDay {0} is invalid", nextMonthLockDay));

            _month = month;
            _year = year;
            _user = user;
            _lockedFrom = CreateLockedFrom(month, year, nextMonthLockDay);
            SetCreationDetails(createdBy);
        }

        public virtual int Id
        {
            get { return _id; }
        }

        public override int EntityId
        {
            get { return Id; }
        }

        /// <summary>
        /// Date for creation
        /// </summary>
        public virtual DateTime CreatedDate
        {
            get { return _createdDate; }
        }

        /// <summary>
        /// User who created this ForecastMonth
        /// </summary>
        public virtual User CreatedBy
        {
            get { return _createdBy; }
        }

        /// <summary>
        /// Month for this ForecastMonth
        /// </summary>
        public virtual int Month
        {
            get { return _month; }
        }

        /// <summary>
        /// Year for this ForecastMonth
        /// </summary>
        public virtual int Year
        {
            get { return _year; }
        }

        /// <summary>
        /// User this ForecastMonth is on
        /// </summary>
        public virtual User User
        {
            get { return _user; }
        }

        /// <summary>
        /// Month is unlocked forced some user
        /// </summary>
        public virtual bool UnLocked
        {
            get { return _unLocked; }
            set { _unLocked = value; }
        }

        /// <summary>
        /// Forecasts in scope of this ForecastMonth
        /// </summary>
        public virtual IEnumerable<Forecast> Forecasts
        {
            get { return _forecasts; }
        }

        /// <summary>
        /// Indicates the date for when ForecastMonth
        /// may be updated until
        /// </summary>
        public virtual DateTime LockedFrom
        {
            get { return _lockedFrom; }
        }

        /// <summary>
        /// Creates and adds a new forecast based on input
        /// </summary>
        /// <param name="date"></param>
        /// <param name="forecastType"></param>
        /// <param name="dedicatedHours"></param>
        public virtual Forecast AddForecast(DateTime date, ForecastType forecastType, decimal? dedicatedHours)
        {
            EnsureDate(date);
            var newItem = new Forecast(date, forecastType, this, dedicatedHours);
            _forecasts.Add(newItem);
            return newItem;
        }

        /// <summary>
        /// Clears forecasts collection
        /// </summary>
        public virtual void ClearForecasts()
        {
            _forecasts.Clear();
        }

        /// <summary>
        /// Incomming date must be in the same month and year
        /// </summary>
        /// <param name="date"></param>
        private void EnsureDate(DateTime date)
        {
            if (date.Month != Month || date.Year != Year)
                throw new Exception(string.Format("Date {0} is not a part of ForecastMonth month: {1} year: {2}", date.ToShortDateString(), _month, _year));
        }

        /// <summary>
        /// Sets the creation details
        /// </summary>
        /// <param name="createdBy"></param>
        public virtual void SetCreationDetails(User createdBy)
        {
            if (createdBy == null)
                throw new Exception("CreatedBy may not be null");

            _createdBy = createdBy;
            _createdDate = DateTime.Now;
        }

        /// <summary>
        /// Get if the Month is locked for updates.
        /// </summary>
        /// <param name="currentLockDate"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public virtual bool IsLocked
        {
            get
            {
                // If marked as unlocked, we dont care for LockedFrom
                if (UnLocked)
                    return false;

                // If current date exceeds LockedFrom then lock
                return (Now.Date > LockedFrom.Date);   
            }
        }

        #region Helpers

        /// <summary>
        /// Creates a date in next month with day nextMonthLockDay
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="nextMonthLockDay"></param>
        /// <returns></returns>
        public static DateTime CreateLockedFrom(int month, int year, int nextMonthLockDay)
        {
            var endOfNextMonth = DateSpan.EndOfMonth(new DateTime(year, month, 1).AddMonths(1));

            // If lockday is exceeds the actual days in the month, then use the end of next month
            return nextMonthLockDay > endOfNextMonth.Day
                ? endOfNextMonth
                : new DateTime(endOfNextMonth.Year, endOfNextMonth.Month, nextMonthLockDay);
        }

        // Poor mans injection for unittesting, since we cant inject services to model. Not persisted
        private DateTime? _now;
        public virtual DateTime Now
        {
            get { return _now.HasValue ? _now.Value : DateTime.Now; }
            set { _now = value; }
        }

        #endregion

    }
}