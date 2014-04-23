using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_12_0 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>();
                list.Add("- Added Registration type to tasks.");
                list.Add("- Registrationtype: 'Projection' is now ignored in totals.");
                list.Add("- Registration type is a new column in reports view.");

                return list.ToArray();
            }
        }
    }
}