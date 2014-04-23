using System;
using System.Runtime.Serialization;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class TimeEntry
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public double TimeSpent { get; set; }

        [DataMember]
        public double BillableTime { get; set; }

        [DataMember]
        public DateTime StartTime { get; set; }

        [DataMember]
        public DateTime EndTime { get; set; }

        [DataMember]
        public double? PricePrHour { get; set; }

        [DataMember]
        public Task Task { get; set; }

        [DataMember]
        public Guid TaskGuid { get; set; }

        [DataMember]
        public int TaskId { get; set; }

        [DataMember]
        public bool Billable { get; set; }

        [DataMember]
        public User User { get; set; }

        [DataMember]
        public TimeEntryType TimeEntryType{get;set;}

        [DataMember]
        public DateTime? ChangeDate { get; set; }

        [DataMember]
        public User ChangedBy { get; set; }

        [DataMember]
        public int? InvoiceId { get; set; }
    }
}