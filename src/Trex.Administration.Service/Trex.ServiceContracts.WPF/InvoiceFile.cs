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
    public partial class InvoiceFile: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int InvoiceID
        {
            get { return _invoiceID; }
            set
            {
                if (_invoiceID != value)
                {
                    ChangeTracker.RecordOriginalValue("InvoiceID", _invoiceID);
                    if (!IsDeserializing)
                    {
                        if (Invoice != null && Invoice.ID != value)
                        {
                            Invoice = null;
                        }
                    }
                    _invoiceID = value;
                    OnPropertyChanged("InvoiceID");
                }
            }
        }
        private int _invoiceID;
    
        [DataMember]
        public int InvoiceFileID
        {
            get { return _invoiceFileID; }
            set
            {
                if (_invoiceFileID != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'InvoiceFileID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _invoiceFileID = value;
                    OnPropertyChanged("InvoiceFileID");
                }
            }
        }
        private int _invoiceFileID;
    
        [DataMember]
        public byte[] File
        {
            get { return _file; }
            set
            {
                if (_file != value)
                {
                    _file = value;
                    OnPropertyChanged("File");
                }
            }
        }
        private byte[] _file;
    
        [DataMember]
        public int FileType
        {
            get { return _fileType; }
            set
            {
                if (_fileType != value)
                {
                    _fileType = value;
                    OnPropertyChanged("FileType");
                }
            }
        }
        private int _fileType;

        #endregion

        #region Navigation Properties
    
        [DataMember]
        public Invoice Invoice
        {
            get { return _invoice; }
            set
            {
                if (!ReferenceEquals(_invoice, value))
                {
                    var previousValue = _invoice;
                    _invoice = value;
                    FixupInvoice(previousValue);
                    OnNavigationPropertyChanged("Invoice");
                }
            }
        }
        private Invoice _invoice;

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
            Invoice = null;
        }

        #endregion

        #region Association Fixup
    
        private void FixupInvoice(Invoice previousValue)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.InvoiceFiles.Contains(this))
            {
                previousValue.InvoiceFiles.Remove(this);
            }
    
            if (Invoice != null)
            {
                if (!Invoice.InvoiceFiles.Contains(this))
                {
                    Invoice.InvoiceFiles.Add(this);
                }
    
                InvoiceID = Invoice.ID;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Invoice")
                    && (ChangeTracker.OriginalValues["Invoice"] == Invoice))
                {
                    ChangeTracker.OriginalValues.Remove("Invoice");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Invoice", previousValue);
                }
                if (Invoice != null && !Invoice.ChangeTracker.ChangeTrackingEnabled)
                {
                    Invoice.StartTracking();
                }
            }
        }

        #endregion

    }
}
