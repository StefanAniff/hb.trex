using System;
using System.Collections.Generic;

namespace Trex.Invoices.Implemented
{
    public class GenerateParameters
    {
        public GenerateParameters()
        {
            CustomerID = new List<int>();
        }
        public List<int> CustomerID { get; set; }
        public DateTime? Endtime { get; set; }
        public DateTime? StartTime { get; set; }
    }
}