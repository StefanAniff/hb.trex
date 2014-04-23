using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Infrastructure.Releasenotes
{
    public abstract class VersionInfoBase : IRelease
    {
        public string Version
        {
            get
            {
                var name = GetType().Name;
                return name.Replace("Version", "").Replace('_', '.');
            }
        }

        public int VersionSize
        {
            get
            {
                var name = GetType().Name;
                var stripped = name.Replace("Version", "").Replace("_", "");
                return int.Parse(stripped);
            }
        }

        public abstract string[] ReleaseNotes { get; }
    }
}
