using System;
using System.Xml.Serialization;
using Trex.SmartClient.Core.Model.Consts;
using Trex.SmartClient.Core.Exceptions;

namespace Trex.SmartClient.Core.Model
{
    [Serializable]
    public class TimeEntry : IComparable<TimeEntry>
    {
        private TimeSpan _timeSpent;
        private const int CLIENT_SOURCE_ID = 3;

        public Guid Guid { get; set; }
        public string Description { get; set; }

        [XmlIgnore]
        public TimeSpan TimeSpent
        {
            get { return _timeSpent; }
            set { _timeSpent = value; }
        }

        public TimeEntryHistory TimeEntryHistory { get; set; }

        [XmlElement("TimeSpent")]
        public string XmlTimeSpent
        {
            get { return TimeSpent.ToString(); }
            set
            {
                TimeSpan timeSpent = TimeSpan.Zero;
                TimeSpan.TryParse(value, out timeSpent);

                TimeSpent = timeSpent;
            }
        }



        [XmlIgnore]
        public TimeSpan BillableTime { get; set; }

        [XmlElement("BillableTime")]
        public string XmlBillableTime
        {
            get { return BillableTime.ToString(); }
            set
            {
                TimeSpan billableTime = TimeSpan.Zero;
                TimeSpan.TryParse(value, out billableTime);

                BillableTime = billableTime;
            }
        }

        [XmlIgnore] //Not persisted in DB
        private string _tempoaryTaskName;

        public string TempoaryTaskName
        {
            get { return string.IsNullOrEmpty(_tempoaryTaskName) ? HelperTextConsts.UnassignedTask : _tempoaryTaskName + "*"; }
            set
            {
                var newValue = value.Trim('*');
                if (newValue != HelperTextConsts.UnassignedTask)
                {
                    _tempoaryTaskName = newValue;
                }
                else
                {
                    _tempoaryTaskName = null;
                }
            }
        }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Task Task { get; set; }
        public double? PricePrHour { get; set; }
        public bool IsSynced { get; set; }
        public string SyncResponse { get; set; }

        public bool HasSyncError
        {
            get
            {
                return !string.IsNullOrEmpty(SyncResponse);
            }
        }

        public bool Billable { get; set; }
        public bool IsStopped { get; set; }
        public TimeEntryType TimeEntryType { get; set; }
        public int ClientSourceId { get; set; }
        public bool Invoiced { get; set; }

        public DateTime CreateDate { get; set; }

        public void Reset()
        {
            TimeSpent = BillableTime = TimeSpan.Zero;
            EndTime = StartTime;
            IsSynced = false;
        }

        public TimeEntry()
        {
        }

        /// <summary>
        /// Creates a timeentry with default values
        /// </summary>
        public static TimeEntry Create()
        {
            return new TimeEntry
            {
                Guid = Guid.NewGuid(),
                BillableTime = TimeSpan.Zero,
                TimeSpent = TimeSpan.Zero,
                Description = string.Empty,
                Task = null,
                PricePrHour = null,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                IsSynced = false,
                SyncResponse = string.Empty,
                Billable = true,
                TimeEntryHistory = new TimeEntryHistory(),
                CreateDate = DateTime.Now,
                ClientSourceId = CLIENT_SOURCE_ID
            };
        }

        /// <summary>
        /// Creates a new timeentry based on a given timeentry (with timespent Zero)
        /// </summary>
        public static TimeEntry Create(TimeEntry timeEntry)
        {

            if (timeEntry == null)
            {
                return Create();
            }
            return new TimeEntry
            {
                Guid = Guid.NewGuid(),
                BillableTime = TimeSpan.Zero,
                TimeSpent = TimeSpan.Zero,
                Description = string.Empty,
                Task = timeEntry.Task,
                PricePrHour = timeEntry.PricePrHour,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                IsSynced = false,
                SyncResponse = string.Empty,
                Billable = true,
                TimeEntryHistory = new TimeEntryHistory(),
                ClientSourceId = timeEntry.ClientSourceId,
                CreateDate = timeEntry.CreateDate
            };
        }

        public static TimeEntry Create(Guid guid, Task task, TimeEntryType timeEntryType, TimeSpan timeSpent, TimeSpan billableTime, string description, DateTime startTime, DateTime endTime, double? pricePrHour,
            bool isSynced, string syncResponse, bool billable, TimeEntryHistory timeEntryHistory, bool isStopped, DateTime createdDate, int clientSourceId, bool invoiced)
        {
            if (task == null)
                throw new MandatoryParameterMissingException("Error creating Timeentry: Task cannot be null");

            return new TimeEntry
            {
                Guid = guid,
                TimeEntryType = timeEntryType,
                BillableTime = billableTime,
                TimeSpent = timeSpent,
                Description = description,
                Task = task,
                IsStopped = isStopped,
                PricePrHour = pricePrHour,
                CreateDate = createdDate,
                StartTime = startTime,
                EndTime = endTime,
                IsSynced = isSynced,
                SyncResponse = syncResponse,
                Billable = billable,
                TimeEntryHistory = timeEntryHistory,
                ClientSourceId = clientSourceId,
                Invoiced = invoiced
            };
        }


        public int CompareTo(TimeEntry other)
        {
            return Guid.CompareTo(other.Guid);
        }
    }
}