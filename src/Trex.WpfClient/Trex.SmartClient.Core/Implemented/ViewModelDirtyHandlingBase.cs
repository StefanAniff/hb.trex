using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Trex.SmartClient.Core.Implemented
{
    /// <summary>
    /// Support for dirty checks
    /// </summary>
    public interface INotifyDirtyState
    {
        bool IsDirty
        {
            get;
        }

        /// <summary>
        /// Initialize for a dirty check. Clears the dirty state and adds event handlers to notification events.
        /// </summary>
        void InitializeDirtyCheck();

        /// <summary>
        /// A list of properties that have changed.
        /// </summary>
        Dictionary<object, List<string>> ChangedProperties { get; }
    }

    public class ViewModelDirtyHandlingBase : ValidationEnabledViewModelBase, INotifyDirtyState
    {
        private DirtyStateHandler _dsh;
        private Func<DirtyStateHandler> _dshProvider;

        [Dependency]
        public Func<DirtyStateHandler> DshProvider
        {
            get { return _dshProvider; }
            set
            {
                _dshProvider = value;
                _dsh = _dshProvider();
            }
        }

        public virtual void InitializeDirtyCheck()
        {
            if (_dsh != null)
                _dsh.Dispose();

            _dsh = DshProvider();
            DisableDirtyHandling = false;
            _dsh.InitializeDirtyCheck(this);
        }

        public virtual bool IsDirty
        {
            get
            {
                return _dsh.IsDirty;
            }
        }

        public Dictionary<object, List<string>> ChangedProperties
        {
            get
            {
                return _dsh == null ? new Dictionary<object, List<string>>() : _dsh.ChangedProperties;
            }
        }

        protected IEnumerable<object> ChangedObjects
        {
            get
            {
                return _dsh.ChangedObjects;
            }
        }

        public IEnumerable<object> RemovedObjects
        {
            get
            {
                return _dsh.RemovedObjects;
            }
        }

        public bool DisableDirtyHandling
        {
            get
            {
                return _dsh.Disabled;
            }
            set
            {
                _dsh.Disabled = value;
            }
        }
    }
}