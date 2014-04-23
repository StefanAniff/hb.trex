using System;
using Microsoft.Practices.Unity;
using Trex.Core.Attributes;
using Trex.Core.Interfaces;

namespace Trex.Core.Implemented
{
    public abstract class ScreenBase : IScreen
    {
        protected ScreenBase(Guid guid, IUnityContainer unityContainer)
        {
            Guid = guid;
            IsActive = false;
            Container = unityContainer;
            Initialize();
        }

        protected IUnityContainer Container { get; set; }

        #region IScreen Members

        public Guid Guid { get; protected set; }

        public IView MasterView { get; protected set; }

        public bool IsActive { get; set; }

        #endregion

        public ScreenAttribute GetScreenAttribute()
        {
            ScreenAttribute attribute = null;
            foreach (var attr in GetType().GetCustomAttributes(typeof (ScreenAttribute), true))
            {
                attribute = attr as ScreenAttribute;
                break;
            }
            return attribute;
        }

        protected abstract void Initialize();
    }
}