using System;
using System.Runtime.Serialization;

namespace Trex.Common.DataTransferObjects
{
    [DataContract]
    public class CompanyDto
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
        public string PhoneNumber { get; set; }

        [DataMember]
        public string CellPhoneNumber { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public string CreatedByName { get; set; }

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
        public bool InheritsTimeEntryTypes { get; set; }

        [DataMember]
        public DateTime? ChangeDate { get; set; }

        [DataMember]
        public int PaymentTermNumberOfDays { get; set; }

        [DataMember]
        public bool PaymentTermIncludeCurrentMonth { get; set; }

        [DataMember]
        public string Address2 { get; set; }

        #region Equality members

        protected bool Equals(CompanyDto other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CompanyDto) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        #endregion
    }
}