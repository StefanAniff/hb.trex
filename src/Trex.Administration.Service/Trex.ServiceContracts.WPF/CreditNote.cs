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
    [KnownType(typeof(Invoice))]
    [KnownType(typeof(TimeEntry))]
    public partial class CreditNote: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int ID
        {
            get { return _iD; }
            set
            {
                if (_iD != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'ID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _iD = value;
                    OnPropertyChanged("ID");
                }
            }
        }
        private int _iD;
    
        [DataMember]
        public Nullable<int> InvoiceID
        {
            get { return _invoiceID; }
            set
            {
                if (_invoiceID != value)
                {
                    ChangeTracker.RecordOriginalValue("InvoiceID", _invoiceID);
                    if (!IsDeserializing)
                    {
                        if (Invoices != null && Invoices.ID != value)
                        {
                            Invoices = null;
                        }
                    }
                    _invoiceID = value;
                    OnPropertyChanged("InvoiceID");
                }
            }
        }
        private Nullable<int> _invoiceID;
    
        [DataMember]
        public Nullable<int> TimeEntryID
        {
            get { return _timeEntryID; }
            set
            {
                if (_timeEntryID != value)
                {
                    ChangeTracker.RecordOriginalValue("TimeEntryID", _timeEntryID);
                    if (!IsDeserializing)
                    {
                        if (TimeEntries != null && TimeEntries.TimeEntryID != value)
                        {
                            TimeEntries = null;
                        }
                    }
                    _timeEntryID = value;
                    OnPropertyChanged("TimeEntryID");
                }
            }
        }
        private Nullable<int> _timeEntryID;
    
        [DataMember]
        public Nullable<int> TaskID
        {
            get { return _taskID; }
            set
            {
                if (_taskID != value)
                {
                    _taskID = value;
                    OnPropertyChanged("TaskID");
                }
            }
        }
        private Nullable<int> _taskID;
    
        [DataMember]
        public Nullable<System.DateTime> StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged("StartTime");
                }
            }
        }
        private Nullable<System.DateTime> _startTime;
    
        [DataMember]
        public Nullable<System.DateTime> EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    OnPropertyChanged("EndTime");
                }
            }
        }
        private Nullable<System.DateTime> _endTime;
    
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
        public Nullable<double> PauseTime
        {
            get { return _pauseTime; }
            set
            {
                if (_pauseTime != value)
                {
                    _pauseTime = value;
                    OnPropertyChanged("PauseTime");
                }
            }
        }
        private Nullable<double> _pauseTime;
    
        [DataMember]
        public Nullable<double> BillableTime
        {
            get { return _billableTime; }
            set
            {
                if (_billableTime != value)
                {
                    _billableTime = value;
                    OnPropertyChanged("BillableTime");
                }
            }
        }
        private Nullable<double> _billableTime;
    
        [DataMember]
        public Nullable<bool> Billable
        {
            get { return _billable; }
            set
            {
                if (_billable != value)
                {
                    _billable = value;
                    OnPropertyChanged("Billable");
                }
            }
        }
        private Nullable<bool> _billable;
    
        [DataMember]
        public Nullable<double> Price
        {
            get { return _price; }
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged("Price");
                }
            }
        }
        private Nullable<double> _price;
    
        [DataMember]
        public Nullable<double> TimeSpent
        {
            get { return _timeSpent; }
            set
            {
                if (_timeSpent != value)
                {
                    _timeSpent = value;
                    OnPropertyChanged("TimeSpent");
                }
            }
        }
        private Nullable<double> _timeSpent;
    
        [DataMember]
        public Nullable<System.Guid> Guid
        {
            get { return _guid; }
            set
            {
                if (_guid != value)
                {
                    _guid = value;
                    OnPropertyChanged("Guid");
                }
            }
        }
        private Nullable<System.Guid> _guid;
    
        [DataMember]
        public Nullable<int> TimeEntryTypeId
        {
            get { return _timeEntryTypeId; }
            set
            {
                if (_timeEntryTypeId != value)
                {
                    _timeEntryTypeId = value;
                    OnPropertyChanged("TimeEntryTypeId");
                }
            }
        }
        private Nullable<int> _timeEntryTypeId;
    
        [DataMember]
        public Nullable<System.DateTime> ChangeDate
        {
            get { return _changeDate; }
            set
            {
                if (_changeDate != value)
                {
                    _changeDate = value;
                    OnPropertyChanged("ChangeDate");
                }
            }
        }
        private Nullable<System.DateTime> _changeDate;
    
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
        public Nullable<int> ClientSourceId
        {
            get { return _clientSourceId; }
            set
            {
                if (_clientSourceId != value)
                {
                    _clientSourceId = value;
                    OnPropertyChanged("ClientSourceId");
                }
            }
        }
        private Nullable<int> _clientSourceId;
    
        [DataMember]
        public Nullable<int> ChangedBy
        {
            get { return _changedBy; }
            set
            {
                if (_changedBy != value)
                {
                    _changedBy = value;
                    OnPropertyChanged("ChangedBy");
                }
            }
        }
        private Nullable<int> _changedBy;
    
        [DataMember]
        public Nullable<int> UserID
        {
            get { return _userID; }
            set
            {
                if (_userID != value)
                {
                    _userID = value;
                    OnPropertyChanged("UserID");
                }
            }
        }
        private Nullable<int> _userID;
    
        [DataMember]
        public Nullable<System.DateTime> DocumentDate
        {
            get { return _documentDate; }
            set
            {
                if (_documentDate != value)
                {
                    _documentDate = value;
                    OnPropertyChanged("DocumentDate");
                }
            }
        }
        private Nullable<System.DateTime> _documentDate;

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public Invoice Invoices
        {
            get { return _invoices; }
            set
            {
                if (!ReferenceEquals(_invoices, value))
                {
                    var previousValue = _invoices;
                    _invoices = value;
                    FixupInvoices(previousValue);
                    OnNavigationPropertyChanged("Invoices");
                }
            }
        }
        private Invoice _invoices;
    
        [DataMember]
        public TimeEntry TimeEntries
        {
            get { return _timeEntries; }
            set
            {
                if (!ReferenceEquals(_timeEntries, value))
                {
                    var previousValue = _timeEntries;
                    _timeEntries = value;
                    FixupTimeEntries(previousValue);
                    OnNavigationPropertyChanged("TimeEntries");
                }
            }
        }
        private TimeEntry _timeEntries;

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
            Invoices = null;
            TimeEntries = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupInvoices(Invoice previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.CreditNote.Contains(this))
            {
                previousValue.CreditNote.Remove(this);
            }
    
            if (Invoices != null)
            {
                if (!Invoices.CreditNote.Contains(this))
                {
                    Invoices.CreditNote.Add(this);
                }
    
                InvoiceID = Invoices.ID;
            }
            else if (!skipKeys)
            {
                InvoiceID = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Invoices")
                    && (ChangeTracker.OriginalValues["Invoices"] == Invoices))
                {
                    ChangeTracker.OriginalValues.Remove("Invoices");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Invoices", previousValue);
                }
                if (Invoices != null && !Invoices.ChangeTracker.ChangeTrackingEnabled)
                {
                    Invoices.StartTracking();
                }
            }
        }
    
        private void FixupTimeEntries(TimeEntry previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.CreditNote.Contains(this))
            {
                previousValue.CreditNote.Remove(this);
            }
    
            if (TimeEntries != null)
            {
                if (!TimeEntries.CreditNote.Contains(this))
                {
                    TimeEntries.CreditNote.Add(this);
                }
    
                TimeEntryID = TimeEntries.TimeEntryID;
            }
            else if (!skipKeys)
            {
                TimeEntryID = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("TimeEntries")
                    && (ChangeTracker.OriginalValues["TimeEntries"] == TimeEntries))
                {
                    ChangeTracker.OriginalValues.Remove("TimeEntries");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("TimeEntries", previousValue);
                }
                if (TimeEntries != null && !TimeEntries.ChangeTracker.ChangeTrackingEnabled)
                {
                    TimeEntries.StartTracking();
                }
            }
        }

        #endregion

    }
}
