#region

using System;
using System.Linq;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Invoices.Commands;
using Trex.ServiceContracts;

#endregion

namespace Trex.Invoices.Dialogs.EditInvoiceLinesView
{
    public class TimeEntryListItemViewModel : ViewModelBase
    {
        private readonly Invoice _invoice;
        private readonly TimeEntry _timeEntry;
        private readonly IUserRepository _userRepository;

        public TimeEntryListItemViewModel(TimeEntry timeEntry, Invoice invoice, IUserRepository userRepository)
        {
            _timeEntry = timeEntry;
            _invoice = invoice;
            _userRepository = userRepository;
        }

        public DateTime StartTime
        {
            get { return _timeEntry.StartTime; }
        }

        public TimeEntry TimeEntry
        {
            get { return _timeEntry; }
        }

        public bool HasChanges
        {
            get { return _timeEntry.ChangeTracker.State == ObjectState.Modified; }
        }

        public string ProjectName
        {
            get
            {
                return string.Empty;
                //_timeEntry.ProjectName; 
            }
        }

        public string TaskName
        {
            get
            {
                return string.Empty;
                //_timeEntry.TaskName;
            }
        }

        public string Description
        {
            get { return _timeEntry.Description; }
        }

        public double BillableTime
        {
            get { return _timeEntry.BillableTime; }
        }

        public double Price
        {
            get { return _timeEntry.Price; }
            set
            {
                _timeEntry.Price = value;
                _timeEntry.Price = value;
                OnPropertyChanged("Price");
            }
        }

        public string User
        {
            get { return _userRepository.GetById(_timeEntry.UserID).Name; }
        }

        public bool Billable
        {
            get { return _timeEntry.Billable; }
            set
            {
                _timeEntry.Billable = value;
                _timeEntry.Billable = value;
                OnPropertyChanged("Billable");
            }
        }

        public bool Approved
        {
            get { return _timeEntry.InvoiceId.HasValue; }
            set
            {
                if (value)
                {
                    _timeEntry.InvoiceId = _invoice.ID;
                    _timeEntry.InvoiceId = _invoice.ID;
                    if (_invoice.TimeEntries.SingleOrDefault(te => te.TimeEntryID == _timeEntry.TimeEntryID) == null)
                        _invoice.TimeEntries.Add(_timeEntry);
                }
                else
                {
                    _timeEntry.InvoiceId = null;
                    _timeEntry.InvoiceId = null;
                    var existingTimeEntry =
                        _invoice.TimeEntries.SingleOrDefault(te => te.TimeEntryID == _timeEntry.TimeEntryID);

                    if (existingTimeEntry != null)
                        _invoice.TimeEntries.Remove(existingTimeEntry);
                }

                OnPropertyChanged("Approved");
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            InternalCommands.TimeEntryChanged.Execute(this.TimeEntry);
        }
    }
}