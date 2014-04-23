//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace Trex.ServiceContracts
{
    
    public partial class UserPermission : INotifyComplexPropertyChanging, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int PermissionID
        {
            get { return _permissionID; }
            set
            {
                if (_permissionID != value)
                {
                    OnComplexPropertyChanging();
                    _permissionID = value;
                    OnPropertyChanged("PermissionID");
                }
            }
        }
        private int _permissionID;
    
        [DataMember]
        public string Permission
        {
            get { return _permission; }
            set
            {
                if (_permission != value)
                {
                    OnComplexPropertyChanging();
                    _permission = value;
                    OnPropertyChanged("Permission");
                }
            }
        }
        private string _permission;

        #endregion

        #region ChangeTracking
    
        private void OnComplexPropertyChanging()
        {
            if (_complexPropertyChanging != null)
            {
                _complexPropertyChanging(this, new EventArgs());
            }
        }
    
        event EventHandler INotifyComplexPropertyChanging.ComplexPropertyChanging { add { _complexPropertyChanging += value; } remove { _complexPropertyChanging -= value; } }
        private event EventHandler _complexPropertyChanging;
    
        private void OnPropertyChanged(String propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged { add { _propertyChanged += value; } remove { _propertyChanged -= value; } }
        private event PropertyChangedEventHandler _propertyChanged;
    
        public static void RecordComplexOriginalValues(String parentPropertyName, UserPermission complexObject, ObjectChangeTracker changeTracker)
        {
            if (String.IsNullOrEmpty(parentPropertyName))
            {
                throw new ArgumentException("String parameter cannot be null or empty.", "parentPropertyName");
            }
    
            if (changeTracker == null)
            {
                throw new ArgumentNullException("changeTracker");
            }
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.PermissionID", parentPropertyName), complexObject == null ? null : (object)complexObject.PermissionID);
            changeTracker.RecordOriginalValue(String.Format(CultureInfo.InvariantCulture, "{0}.Permission", parentPropertyName), complexObject == null ? null : (object)complexObject.Permission);
        }

        #endregion

    }
}
