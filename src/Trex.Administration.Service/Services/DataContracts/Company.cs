using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class Company
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool Inactive { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public List<Project> Projects { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string CellPhoneNumber { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public User CreatedBy { get; set; }

        [DataMember]
        public string StreetAddress { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string City { get; set; }
        
        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string ContactName { get; set; }

        [DataMember]
        public string ContactPhone { get; set; }

        [DataMember]
        public List<TimeEntryType> TimeEntryTypes { get; set; }

        [DataMember]
        public bool InheritsTimeEntryTypes { get; set; }

        [DataMember]
        public DateTime? ChangeDate { get; set; }

        [DataMember]
        public User ChangedBy { get; set; }

        [DataMember]
        public int PaymentTermNumberOfDays { get; set; }

        [DataMember]
        public bool PaymentTermIncludeCurrentMonth { get; set; }

        [DataMember]
        public string Address2 { get; set; }

       
    }
}