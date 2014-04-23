using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TaskCost
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public decimal HourPay { get; set; }
        public string Unit { set; get; }
        public decimal TimeSpend { get; set; }
        public decimal VAT { set; get; }
    }

    public class TableData
    {
        public decimal UnitCount { set; get; }
        public string Unit { set; get; }
        public decimal UnitPrice { set; get; }
        public decimal TotalNoVAT { set; get; }
    }
}
