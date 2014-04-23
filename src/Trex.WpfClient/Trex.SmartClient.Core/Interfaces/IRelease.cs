using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface IRelease
    {
        string Version { get; }
        string[] ReleaseNotes { get; }
        int VersionSize { get; }
    }
}
