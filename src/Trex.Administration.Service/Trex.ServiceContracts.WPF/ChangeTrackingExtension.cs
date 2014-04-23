using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Trex.ServiceContracts
{
    public class ObjectStateChangedEventArgs : EventArgs
    {
        public ObjectState NewState { get; set; }
    }


    public partial class ObjectChangeTracker
    {

        private object _parentObject;


        public event EventHandler<ObjectStateChangedEventArgs> ObjectStateChanged;
        public event EventHandler UpdateHasChanges;

        public bool HasChanges { get; set; }

        protected virtual void OnObjectStateChanged(ObjectState newState)
        {
            if (ObjectStateChanged != null)
            {
                ObjectStateChanged(this, new ObjectStateChangedEventArgs() { NewState = newState });
            }
        }

        protected virtual void OnUpdateHasChanges()
        {
            if (UpdateHasChanges != null)
            {
                UpdateHasChanges(this, new EventArgs());
            }
        }

        // Resets the ObjectChangeTracker to the Unchanged state and
        // rollback the original values as well as the record of changes
        // to collection properties
        public void CancelChanges()
        {
            OnObjectStateChanging(ObjectState.Unchanged);
            // rollback original values
            Type type = _parentObject.GetType();
            foreach (var originalValue in OriginalValues.ToList())
                type.GetProperty(originalValue.Key).SetValue(
                     _parentObject, originalValue.Value, null);
            // create copy of ObjectsAddedToCollectionProperties
            // and ObjectsRemovedFromCollectionProperties
            Dictionary<string, ObjectList> removeCollection =
              ObjectsAddedToCollectionProperties.ToDictionary(n => n.Key, n => n.Value);
            Dictionary<string, ObjectList> addCollection =
              ObjectsRemovedFromCollectionProperties.ToDictionary(n => n.Key, n => n.Value);
            // rollback ObjectsAddedToCollectionProperties
            if (removeCollection.Count > 0)
            {
                foreach (KeyValuePair<string, ObjectList> entry in removeCollection)
                {
                    PropertyInfo collectionProperty = type.GetProperty(entry.Key);
                    IList collectionObject = (IList)collectionProperty.GetValue(_parentObject, null);
                    foreach (object obj in entry.Value.ToList())
                    {
                        collectionObject.Remove(obj);
                    }
                }
            }
            // rollback ObjectsRemovedFromCollectionProperties
            if (addCollection.Count > 0)
            {
                foreach (KeyValuePair<string, ObjectList> entry in addCollection)
                {
                    PropertyInfo collectionProperty = type.GetProperty(entry.Key);
                    IList collectionObject = (IList)collectionProperty.GetValue(_parentObject, null);
                    foreach (object obj in entry.Value.ToList())
                    {
                        collectionObject.Add(obj);
                    }
                }
            }
            OriginalValues.Clear();
            ObjectsAddedToCollectionProperties.Clear();
            ObjectsRemovedFromCollectionProperties.Clear();
            _objectState = ObjectState.Unchanged;
            OnObjectStateChanged(ObjectState.Unchanged);
        }

        public void SetParentObject(object parent)
        {
            this._parentObject = parent;
        }

    }
}
