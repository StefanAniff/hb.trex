using System.Runtime.Serialization;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class UserCustomerInfo
    {
        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public double PricePrHour { get; set; }

    }
}