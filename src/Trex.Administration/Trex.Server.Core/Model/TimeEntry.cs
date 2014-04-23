using System;

namespace Trex.Server.Core.Model
{
    /// <summary>
    /// TimeEntry object for NHibernate mapped table 'Trex.Servers'.
    /// </summary>
    public class TimeEntry : IComparable
    {
        #region Constructors

        public TimeEntry() {}

        public TimeEntry(Guid guid, DateTime startTime, DateTime endTime, TimeEntryType timeEntryType, string description, double timeSpent, double pauseTime, double billableTime, bool billable, double price,
                         Task task, User user)
        {
            Guid = guid;
            StartTime = startTime;
            EndTime = endTime;
            Description = description;
            TimeSpent = timeSpent;
            PauseTime = pauseTime;
            Task = task;
            User = user;
            BillableTime = billableTime;
            Billable = billable;
            Price = price;
            TimeEntryType = timeEntryType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>The start time.</value>
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>The end time.</value>
        public virtual DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the pause time.
        /// </summary>
        /// <value>The pause time.</value>
        public virtual double PauseTime { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the billable time.
        /// </summary>
        /// <value>The billable time.</value>
        public virtual double BillableTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Trex.Server"/> is billable.
        /// </summary>
        /// <value><c>true</c> if billable; otherwise, <c>false</c>.</value>
        public virtual bool Billable { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public virtual double Price { get; set; }

        /// <summary>
        /// Gets or sets the task.
        /// </summary>
        /// <value>The task.</value>
        public virtual Task Task { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public virtual User User { get; set; }

        public virtual DateTime? ChangeDate { get; set; }

        public virtual User ChangedBy { get; set; }

        /// <summary>
        /// Gets or sets the actual time spent.
        /// </summary>
        /// <value>The time spent.</value>
        public virtual double TimeSpent { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public virtual Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the invoice.
        /// </summary>
        /// <value>The invoice.</value>
        public virtual Invoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the type of the time entry.
        /// </summary>
        /// <value>The type of the time entry.</value>
        public virtual TimeEntryType TimeEntryType { get; set; }

        public virtual string TimeSpentFormatted
        {
            get
            {
                var hours = Math.Floor(TimeSpent);

                var decimalMinutes = TimeSpent - hours;

                var minutes = decimalMinutes*60;

                minutes = Math.Floor(minutes);

                //string hourString = string.Format("{0:00}", hours);
                //string minuteString = string.Format("{0:00}",  minutes);

                return string.Format("{0:00}:{1:00}", hours, minutes);
            }
        }

        /// <summary>
        /// Determines if the time entry is weeklocked, by the given date
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <returns></returns>
        public virtual bool WeekLocked(DateTime currentDate)
        {
            var daysSinceWeekLocked = currentDate.DayOfWeek - DayOfWeek.Monday;

            var weekLockDate = currentDate.Date.AddDays(-daysSinceWeekLocked);

            //If the timeentry´s endtime is older than the weeklockDate
            return EndTime.Date < weekLockDate;
        }

        #endregion

        #region IComparable Methods

        public virtual int CompareTo(object obj)
        {
            if (!(obj is TimeEntry))
            {
                throw new InvalidCastException("This object is not of type Trex.Server");
            }

            var relativeValue = 0;
            //switch (SortExpression)
            //{
            //    case "Id":
            //        relativeValue = this.Id.CompareTo(((Trex.Server)obj).Id);
            //        break;
            //    case "StartTime":
            //        relativeValue = this.StartTime.CompareTo(((Trex.Server)obj).StartTime);
            //        break;
            //    case "EndTime":
            //        relativeValue = this.EndTime.CompareTo(((Trex.Server)obj).EndTime);
            //        break;
            //    case "PauseTime":
            //        relativeValue = this.PauseTime.CompareTo(((Trex.Server)obj).PauseTime);
            //        break;
            //    case "TotalTime":
            //        relativeValue = this.TotalTime.CompareTo(((Trex.Server)obj).TotalTime);
            //        break;
            //    default:
            //        goto case "Id";
            //}
            //if (Trex.Server.SortDirection == SortDirection.Ascending)
            //    relativeValue *= -1;
            return relativeValue;
        }

        #endregion
    }
}