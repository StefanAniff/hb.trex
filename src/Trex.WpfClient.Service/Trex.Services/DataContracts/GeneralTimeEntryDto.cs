using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class GeneralTimeEntryDto
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
        public int TaskId { get; set; }

        [DataMember]
        public int ProjectId { get; set; }

        [DataMember]
        public Guid TaskGuid { get; set; }

        [DataMember]
        public bool Billable { get; set; }

        [DataMember]
        public int TimeEntryTypeId { get; set; }

        [DataMember]
        public DateTime? ChangeDate { get; set; }

        [DataMember]
        public int ClientSourceId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }
    }
}