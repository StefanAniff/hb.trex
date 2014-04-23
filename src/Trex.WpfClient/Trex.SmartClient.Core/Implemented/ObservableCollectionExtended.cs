using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Trex.SmartClient.Core.Implemented
{
    public interface IObservableCollectionExtended
    {
        void ClearRemovedItems();
    }

    /// <summary>
    /// Obervable Collection with Find/FindAll methods, and support for Serialization be decoupling of listeners.
    /// </summary>
    public class ObservableCollectionExtended<T> : ObservableCollection<T>, IObservableCollectionExtended
    {
        private NotifyCollectionChangedEventHandler _collectionChanged;
        private readonly List<T> _removedItems = new List<T>();

        public ObservableCollectionExtended(IEnumerable<T> range)
        {
            AddRange(range);
        }

        public ObservableCollectionExtended()
        {
        }

        public ObservableCollectionExtended(params T[] range)
        {
            AddRange(range);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (_collectionChanged != null) _collectionChanged(this, e);
        }

        public override event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _collectionChanged += value; }
            remove
            {
                if (_collectionChanged != null) 
                    _collectionChanged -= value;
            }
        }


        public void Sort(Comparison<T> comparison)
        {
            var list = Items as List<T>;
            if (list != null)
            {
                list.Sort(comparison);
            }
        }

        /// <summary>
        /// Adds a range
        /// </summary>
        /// <param name="coll"></param>
        public void AddRange(IEnumerable<T> coll)
        {
            foreach (var t in coll)
            {
                Add(t);
            }
        }

        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        public IEnumerable<T> RemovedItems { get { return _removedItems; } }

        protected override void RemoveItem(int index)
        {
            _removedItems.Add(this[index]);
            base.RemoveItem(index);
        }

        public void ClearRemovedItems()
        {
            _removedItems.Clear();
        }

        public void InitializeDirtyCheck()
        {
            ClearRemovedItems();
            foreach (var item in this)
            {
                var canHandleDirty = item as INotifyDirtyState;
                if (canHandleDirty == null)
                    continue;

                canHandleDirty.InitializeDirtyCheck();
            }
        }
    }
}