using System;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Core.Implemented
{
    public class SubMenuInfo
    {
        private bool _isActive;
        public string DisplayName { get; private set; }
        public Type SubMenuType { get; set; }
        public string SubMenuName { get { return SubMenuType != null ? SubMenuType.Name : null; } }
        public object Argument { get; private set; }
        public IMenuInfo Parent { get; private set; }
        public Guid Guid { get; private set; }

        public event EventHandler IsActiveChanged;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                if (IsActiveChanged != null)
                    IsActiveChanged.Invoke(this, null);
            }
        }

        public void AddArgument(object argument)
        {
            Argument = argument;
        }

        public static SubMenuInfo Create(string displayName, Type viewType, IMenuInfo parent)
        {
            return new SubMenuInfo
            {
                DisplayName = displayName,
                SubMenuType = viewType,           
                Parent = parent,
                Guid = Guid.NewGuid(),
            };
        }
    }
}
