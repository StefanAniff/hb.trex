using System;
using System.Runtime.Serialization;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class CustomerStatsDto
    {

        [DataMember]
        public double InventoryValue { get; set; }

        [DataMember]
        public DateTime FirstEntry { get; set; }

        [DataMember]
        public int DistinctPrices { get; set; }

    }
}