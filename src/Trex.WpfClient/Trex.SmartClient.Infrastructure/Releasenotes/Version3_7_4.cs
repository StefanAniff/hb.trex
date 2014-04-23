using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_7_4 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Grouping on 'Week, month, year' fixes");
                list.Add("- Inactive tasks now show up behind bottomview. Not infront");
                list.Add("- Label rotation when there is multiple groups in report view");
                list.Add("- Made nonbillable checkbox harder to hit");
                list.Add("- Should handle 'lost connection' better");
                list.Add("- Improvements to editing a task from Reports view.");
                return list.ToArray();
            }
        }
    }
}