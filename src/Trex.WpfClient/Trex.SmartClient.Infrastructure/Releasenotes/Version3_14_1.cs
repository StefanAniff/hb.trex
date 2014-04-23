using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_14_1 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>
                    {
                        "- Copy previous month now copies hours using most frequent day-layout pr. weekday",
                        "- Changed total-row-input to also apply on statustypes supporting hours",
                        "- Statustype hours are now included in statistics as Internal (% Illness)",
                        "- For the keyboard-haters, added double-click selection in project-result popup",
                        "- Other minor fixes and cleanups"
                    };
                return list.ToArray();
            }
        }
    }
}