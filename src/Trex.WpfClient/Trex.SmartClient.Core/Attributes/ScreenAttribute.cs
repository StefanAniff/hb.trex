using System;

namespace Trex.SmartClient.Core.Attributes
{
    public class ScreenAttribute:Attribute
    {
        public string Name { get; set; }
        public bool CanBeDeactivated { get; set; }
        public bool IsStartScreen { get; set; }
        public bool OpenAsDialog { get; set; }
    }
}