using System;

namespace Trex.Core.Model
{
    public class CustomerStats
    {
        public double InventoryValue { get; set; }
        public DateTime FirstEntry { get; set; }
        public double DistinctPrices { get; set; }
    }
}