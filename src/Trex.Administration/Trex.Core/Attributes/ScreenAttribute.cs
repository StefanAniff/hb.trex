using System;

namespace Trex.Core.Attributes
{
    public class ScreenAttribute : Attribute
    {
        public string Name { get; set; }
        public bool CanBeDeactivated { get; set; }
        public string AllowedRoles { get; set; }
    }
}