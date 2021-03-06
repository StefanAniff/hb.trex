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
    [DataContract(IsReference = true)]
    public partial class TimeEntryView: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int TimeEntryID
        {
            get { return _timeEntryID; }
            set
            {
                if (_timeEntryID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'TimeEntryID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _timeEntryID = value;
                    OnPropertyChanged("TimeEntryID");
                }
            }
        }
        private int _timeEntryID;
    
        [DataMember]
        public string TaskName
        {
            get { return _taskName; }
            set
            {
                if (_taskName != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'TaskName' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _taskName = value;
                    OnPropertyChanged("TaskName");
                }
            }
        }
        private string _taskName;
    
        [DataMember]
        public double BillableTime
        {
            get { return _billableTime; }
            set
            {
                if (_billableTime != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'BillableTime' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _billableTime = value;
                    OnPropertyChanged("BillableTime");
                }
            }
        }
        private double _billableTime;
    
        [DataMember]
        public double TimeSpent
        {
            get { return _timeSpent; }
            set
            {
                if (_timeSpent != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'TimeSpent' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _timeSpent = value;
                    OnPropertyChanged("TimeSpent");
                }
            }
        }
        private double _timeSpent;
    
        [DataMember]
        public System.DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'StartTime' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _startTime = value;
                    OnPropertyChanged("StartTime");
                }
            }
        }
        private System.DateTime _startTime;
    
        [DataMember]
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                if (_customerName != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'CustomerName' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _customerName = value;
                    OnPropertyChanged("CustomerName");
                }
            }
        }
        private string _customerName;
    
        [DataMember]
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                if (_projectName != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'ProjectName' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _projectName = value;
                    OnPropertyChanged("ProjectName");
                }
            }
        }
        private string _projectName;
    
        [DataMember]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private string _name;
    
        [DataMember]
        public bool Billable
        {
            get { return _billable; }
            set
            {
                if (_billable != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'Billable' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _billable = value;
                    OnPropertyChanged("Billable");
                }
            }
        }
        private bool _billable;
    
        [DataMember]
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        private string _description;
    
        [DataMember]
        public string Location
        {
            get { return _location; }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged("Location");
                }
            }
        }
        private string _location;
    
        [DataMember]
        public string Department
        {
            get { return _department; }
            set
            {
                if (_department != value)
                {
                    _department = value;
                    OnPropertyChanged("Department");
                }
            }
        }
        private string _department;
    
        [DataMember]
        public Nullable<int> InvoiceId
        {
            get { return _invoiceId; }
            set
            {
                if (_invoiceId != value)
                {
                    _invoiceId = value;
                    OnPropertyChanged("InvoiceId");
                }
            }
        }
        private Nullable<int> _invoiceId;
    
        [DataMember]
        public double Price
        {
            get { return _price; }
            set
            {
                if (_price != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'Price' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _price = value;
                    OnPropertyChanged("Price");
                }
            }
        }
        private double _price;
    
        [DataMember]
        public System.DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'EndTime' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _endTime = value;
                    OnPropertyChanged("EndTime");
                }
            }
        }
        private System.DateTime _endTime;
    
        [DataMember]
        public System.Guid Guid
        {
            get { return _guid; }
            set
            {
                if (_guid != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'Guid' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _guid = value;
                    OnPropertyChanged("Guid");
                }
            }
        }
        private System.Guid _guid;
    
        [DataMember]
        public int TimeEntryTypeId
        {
            get { return _timeEntryTypeId; }
            set
            {
                if (_timeEntryTypeId != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'TimeEntryTypeId' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _timeEntryTypeId = value;
                    OnPropertyChanged("TimeEntryTypeId");
                }
            }
        }
        private int _timeEntryTypeId;
    
        [DataMember]
        public Nullable<System.DateTime> CreateDate
        {
            get { return _createDate; }
            set
            {
                if (_createDate != value)
                {
                    _createDate = value;
                    OnPropertyChanged("CreateDate");
                }
            }
        }
        private Nullable<System.DateTime> _createDate;
    
        [DataMember]
        public int ClientSourceId
        {
            get { return _clientSourceId; }
            set
            {
                if (_clientSourceId != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'ClientSourceId' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _clientSourceId = value;
                    OnPropertyChanged("ClientSourceId");
                }
            }
        }
        private int _clientSourceId;
    
        [DataMember]
        public int TaskID
        {
            get { return _taskID; }
            set
            {
                if (_taskID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'TaskID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _taskID = value;
                    OnPropertyChanged("TaskID");
                }
            }
        }
        private int _taskID;
    
        [DataMember]
        public int ProjectID
        {
            get { return _projectID; }
            set
            {
                if (_projectID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'ProjectID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _projectID = value;
                    OnPropertyChanged("ProjectID");
                }
            }
        }
        private int _projectID;
    
        [DataMember]
        public int CustomerID
        {
            get { return _customerID; }
            set
            {
                if (_customerID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'CustomerID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _customerID = value;
                    OnPropertyChanged("CustomerID");
                }
            }
        }
        private int _customerID;

        #endregion

        #region ChangeTracking
    
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
            {
                ChangeTracker.State = ObjectState.Modified;
            }
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        protected virtual void OnNavigationPropertyChanged(String propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged{ add { _propertyChanged += value; } remove { _propertyChanged -= value; } }
        private event PropertyChangedEventHandler _propertyChanged;
        private ObjectChangeTracker _changeTracker;
    
        [DataMember]
        public ObjectChangeTracker ChangeTracker
        {
            get
            {
                if (_changeTracker == null)
                {
                    _changeTracker = new ObjectChangeTracker();
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
                return _changeTracker;
            }
            set
            {
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
                }
                _changeTracker = value;
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
            }
        }
    
        private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                ClearNavigationProperties();
            }
        }
    
        protected bool IsDeserializing { get; private set; }
    
        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            IsDeserializing = true;
        }
    
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            IsDeserializing = false;
            ChangeTracker.ChangeTrackingEnabled = true;
        }
    
        protected virtual void ClearNavigationProperties()
        {
        }

        #endregion

    }
}
