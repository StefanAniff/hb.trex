using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Reporting.ReportScreen.ReportScreenMasterView
{
    public class PredefinedSearchViewModel
    {
        public PredefinedSearchViewModel(string name,DateTime fromDate, DateTime toDate)
        {
            Name = name;
            FromDate = fromDate;
            ToDate = toDate;
            
        }
        public string Name { get; set; }
        public DateTime FromDate { get; set;}
        public DateTime ToDate { get; set; }
    }
}
