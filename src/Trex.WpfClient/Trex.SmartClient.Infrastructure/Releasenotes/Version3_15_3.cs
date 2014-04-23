using System.Collections.Generic;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public class Version3_15_3 : VersionInfoBase
    {
        public override string[] ReleaseNotes
        {
            get
            {
                var list = new List<string>
                    {
                        "Bugfix:",
                        "- Daily overview graph not rendered",
                        "- Missing scrollbar in WLan settings",
                        "- Crash when closing and no active timeentry exists",
                        "",
                        "- Moved to Azure environment",
                        "   * Download location is here trexdownload.d60.dk", 
                        "   * Admin is here: trexadmin.d60.dk", 
                        "- Time is no longer lost on resume, when shutting down T.Rex with a running task",
                        "- Login errormessage cleanup",
                        "- Fixed GUI lockup, when notfication delay is set to 0",
                        "- Version tooltip now shows which environment client is running in",                        
                    };
                return list.ToArray();
            }
        }
    }
}