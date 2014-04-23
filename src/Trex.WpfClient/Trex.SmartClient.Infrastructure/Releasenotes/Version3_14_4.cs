using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_14_4 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>
                    {
                        "- Past workplan locking. Past workplans are now locked from the 3rd of current month",
                        "- \"This month\" button",
                        "- Minor model refactoring",
                        "- Minor workplan bugfixes"
                    };
                return list.ToArray();
            }
        }
    }
}