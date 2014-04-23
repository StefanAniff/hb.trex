using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using log4net;

namespace Trex.SmartClient.Core.Implemented
{
    /// <summary>
    /// Use this attribute on properties not to be hooked up for dirtyCheck.
    /// </summary>
    public class NoDirtyCheck : Attribute { }

    public class DirtyStateHandler : IDisposable
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<object> _changedObjects = new List<object>();
        private readonly List<object> _removedObjects = new List<object>();
        private readonly ISet<object> _dirtyList = new HashSet<object>();
        private readonly Dictionary<object, List<string>> _changedProperties = new Dictionary<object, List<string>>();
        private readonly List<INotifyPropertyChanged> _monitoredItems = new List<INotifyPropertyChanged>();
        private readonly List<INotifyCollectionChanged> _monitoredCollections = new List<INotifyCollectionChanged>();

        public bool IsDirty { get; set; }
        public bool Disabled { get; set; }

        public IEnumerable<object> ChangedObjects
        {
            get { return _changedObjects; }
        }

        /// <summary>
        /// When items are removed from a colleciton
        /// they are stored here
        /// </summary>
        public IEnumerable<object> RemovedObjects
        {
            get { return _removedObjects; }
        }

        public Dictionary<object, List<string>> ChangedProperties
        {
            get { return _changedProperties; }
        }

        /// <summary>
        /// Initialize a dirty check on the object graph starting with the object passed as parameter.
        /// This means adding event handlers to INotifyPropertyChanged.PropertyChanged event and
        /// INotifyCollectionChanged.CollectionChanged event for all relevant objects in the graph.
        /// The event handlers maintain the list of changed objects, and marks the graph as dirty.
        /// </summary>
        /// <param name="data">The root object used in the dirty check</param>
        public void InitializeDirtyCheck(object data)
        {
            _dirtyList.Clear();
            _changedObjects.Clear();
            _removedObjects.Clear();
            _changedProperties.Clear();

            TryResetCollection(data);
            TryClearMonitoredItems();
            DoInitializeDirtyCheck(data);
        }

        private static void TryResetCollection(object data)
        {
            var collectionExtended = data as IObservableCollectionExtended;
            if (collectionExtended != null)
                collectionExtended.ClearRemovedItems();
        }

        /// <summary>
        /// Called recursively.
        /// </summary>
        /// <param name="data"></param>
        private void DoInitializeDirtyCheck(object data)
        {
            if (data == null) return;
            if (data is String) return;
            if (!(data is INotifyPropertyChanged || data is INotifyCollectionChanged || data is IEnumerable)) return;

            if (_dirtyList.Contains(data))
            {
                return;
            }
            _dirtyList.Add(data);
            DoInitialize(data);

            var piArray = data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);            

            foreach (var info in piArray)
            {
                if (info.GetCustomAttributes(typeof(NoDirtyCheck), false).Length > 0) continue;
                if (info.PropertyType.IsValueType) continue;
                if (info.Name == "Item") continue;//what is the correct way of handling not to invoke Indexers??
                if (info.CanRead)
                {
                    try
                    {
                        var propertyValue = info.GetValue(data, null);
                        DoInitializeDirtyCheck(propertyValue);
                    }
                    catch (Exception ex)
                    {
                        // Note: Set [NoDirtyCheck] attribute on properties that causes exceptions and which should not be dirtyChecked.
                        _log.Warn(string.Format("DirtyStateHandler: An Error has Occured while Invoking Property {0} on object:\n{1}", info.Name, data), ex);
                    }
                }
            }
        }

        private void TryClearMonitoredItems()
        {
            if (_monitoredItems != null && _monitoredItems.Count != 0)
            {
                foreach (var monitoredItem in _monitoredItems)
                {
                    monitoredItem.PropertyChanged -= NotifyPropertyChanged;
                }    
                _monitoredItems.Clear();
            }
                
            if (_monitoredCollections != null && _monitoredCollections.Count != 0)
            {
                foreach (var monitoredCollection in _monitoredCollections)
                {
                    monitoredCollection.CollectionChanged -= NotifyCollectionChanged;
                }
                _monitoredCollections.Clear();
            }
            
        }

        private void DoInitialize(object data)
        {
            if (data == null) return;

            //ObservableCollections implements both INotifyPropertyChanged and INotifyCollectionChanged, hence we test for both without an 'else'
            var npc = data as INotifyPropertyChanged;
            if (npc != null)
            {
                npc.PropertyChanged += NotifyPropertyChanged;
                _monitoredItems.Add(npc);
            }

            var coll = data as INotifyCollectionChanged;
            if (coll != null)
            {
                coll.CollectionChanged += NotifyCollectionChanged;
                _monitoredCollections.Add(coll);
            }

            var ieColl = data as IEnumerable;//All relevant collections implement IEnumerable

            if (ieColl == null) 
                return;

            foreach (var item in ieColl)
            {
                DoInitializeDirtyCheck(item);
            }
        }

        private void NotifyCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Disabled)
                return;

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    DoInitializeDirtyCheck(item);
                    if (!_changedObjects.Contains(item))
                    {
                        _changedObjects.Add(item);
                        _changedProperties.Add(item, new List<string>());
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    _changedObjects.Remove(item);
                    _changedProperties.Remove(item);
                    if (!_removedObjects.Contains(item)) _removedObjects.Add(item);
                }
            }

            DetermineIfDirty();
        }

        private void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Disabled)
                return;

            var propertyInfo = sender.GetType().GetProperty(e.PropertyName);
            if (propertyInfo == null)
                return;

            if (propertyInfo.GetCustomAttributes(typeof(NoDirtyCheck), false).Length > 0) 
                return;

            if (!_changedObjects.Contains(sender))
            {
                _changedObjects.Add(sender);
                _changedProperties.Add(sender, new List<string>());
            }

            List<string> props;
            if (_changedProperties.TryGetValue(sender, out props))
            {
                if (!(props.Contains(e.PropertyName))) props.Add(e.PropertyName);
                if (propertyInfo == null) return;
                var propertyValue = propertyInfo.GetValue(sender, null);
                DoInitializeDirtyCheck(propertyValue);
            }

            DetermineIfDirty();
        }

        private void DetermineIfDirty()
        {
            IsDirty = (_changedObjects.Count != 0 || _removedObjects.Count != 0);
        }

        public void Dispose()
        {
            _changedObjects.Clear();
            _removedObjects.Clear();
            _dirtyList.Clear();
            _changedProperties.Clear();
            _monitoredItems.Clear();
            _monitoredCollections.Clear();
        }
    }
}