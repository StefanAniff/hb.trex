using System;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Core.Implemented
{
    public class SubMenuInfo
    {
        public string DisplayName { get; private set; }
        public string SubMenuName { get; private set; }
        public object Argument { get; private set; }
        public IMenuInfo Parent { get; private set; }
        public Guid Guid { get; private set; }

        public bool IsActive { get; set; }

        public void AddArguement(object argument)
        {
            Argument = argument;
        }

        public static SubMenuInfo Create(string displayName, string submenuview, IMenuInfo parent)
        {
            return new SubMenuInfo
            {
                DisplayName = displayName,
                SubMenuName = submenuview,
                Parent = parent,
                Guid = Guid.NewGuid(),
            };
        }        
    }
}
