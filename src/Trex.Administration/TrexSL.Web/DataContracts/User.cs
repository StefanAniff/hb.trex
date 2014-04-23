using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public bool Inactive { get; set; }

        [DataMember]
        public List<Project> Projects { get; set; }

        [DataMember]
        public int NumOfTimeEntries { get; set; }

        [DataMember]
        public double TotalTime { get; set; }

        [DataMember]
        public double TotalBillableTime { get; set; }

        [DataMember]
        public List<string> Permissions { get; set; }

        [DataMember]
        public List<string> Roles { get; set; }

        /// <summary>
        /// Gets or sets the customer info.
        /// </summary>
        /// <value>The customer info.</value>
        [DataMember]
        public virtual List<UserCustomerInfo> CustomerInfo { get; set; }
    }
}