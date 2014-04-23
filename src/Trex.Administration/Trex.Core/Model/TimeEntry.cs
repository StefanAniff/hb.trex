using System;
using Trex.Core.Interfaces;

namespace Trex.Core.Model
{
    /// <summary>
    /// TimeEntry object for NHibernate mapped table 'TimeRegs'.
    /// </summary>
    public class TimeEntry : IComparable, IEntity
    {
        private Task _task;

        #region Constructors

        public TimeEntry()
        {
            Guid = Guid.NewGuid();
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
        }

        public TimeEntry(Guid guid, DateTime startTime, DateTime endTime, string description, double timeSpent, double pauseTime, double billableTime, bool billable, double price, Task task, User user)
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
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>The start time.</value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>The end time.</value>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the pause time.
        /// </summary>
        /// <value>The pause time.</value>
        public double PauseTime { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        public string TaskName { get; set; }

        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the billable time.
        /// </summary>
        /// <value>The billable time.</value>
        public double BillableTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TimeReg"/> is billable.
        /// </summary>
        /// <value><c>true</c> if billable; otherwise, <c>false</c>.</value>
        public bool Billable { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public double? Price { get; set; }

        /// <summary>
        /// Gets or sets the task.
        /// </summary>
        /// <value>The task.</value>
        public Task Task
        {
            get { return _task; }
            set
            {
                _task = value;
                TaskId = value.Id;
                TaskGuid = value.Guid;
            }
        }

        public bool Approved { get; set; }

        public int TaskId { get; set; }

        public Guid TaskGuid { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the actual time spent.
        /// </summary>
        /// <value>The time spent.</value>
        public double TimeSpent { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public Guid Guid { get; set; }

        public int? InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the invoice.
        /// </summary>
        /// <value>The invoice.</value>
        public Invoice Invoice { get; set; }

        public string TimeSpentFormatted
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

        public TimeEntryType TimeEntryType { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        public bool IsValidChild(IEntity entity)
        {
            return false;
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

        public override string ToString()
        {
            return string.Format("{0} {1}", TimeSpentFormatted, User.Name);
        }

        #endregion

        #region IComparable Methods

        public virtual int CompareTo(object obj)
        {
            if (!(obj is TimeEntry))
            {
                throw new InvalidCastException("This object is not of type TimeReg");
            }

            var relativeValue = 0;
            //switch (SortExpression)
            //{
            //    case "Id":
            //        relativeValue = this.Id.CompareTo(((TimeReg)obj).Id);
            //        break;
            //    case "StartTime":
            //        relativeValue = this.StartTime.CompareTo(((TimeReg)obj).StartTime);
            //        break;
            //    case "EndTime":
            //        relativeValue = this.EndTime.CompareTo(((TimeReg)obj).EndTime);
            //        break;
            //    case "PauseTime":
            //        relativeValue = this.PauseTime.CompareTo(((TimeReg)obj).PauseTime);
            //        break;
            //    case "TotalTime":
            //        relativeValue = this.TotalTime.CompareTo(((TimeReg)obj).TotalTime);
            //        break;
            //    default:
            //        goto case "Id";
            //}
            //if (TimeReg.SortDirection == SortDirection.Ascending)
            //    relativeValue *= -1;
            return relativeValue;
        }

        #endregion
    }
}