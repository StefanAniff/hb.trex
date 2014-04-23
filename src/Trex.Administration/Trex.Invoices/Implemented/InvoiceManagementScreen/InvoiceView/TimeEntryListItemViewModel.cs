#region

using System;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Invoices.Commands;
using Trex.ServiceContracts;

#endregion

namespace Trex.Invoices.InvoiceManagementScreen.InvoiceView
{
    public class TimeEntryListItemViewModel : ViewModelBase
    {
        private readonly InvoiceListItemView _invoice;
        private readonly TimeEntry _timeEntry;
        private readonly IUserRepository _userRepository;
        private readonly IDataService _dataService;

        public TimeEntryListItemViewModel(TimeEntry timeEntry, InvoiceListItemView invoice, IUserRepository userRepository, IDataService dataService)
        {
            _timeEntry = timeEntry;
            _invoice = invoice;
            _userRepository = userRepository;
            _dataService = dataService;
            ExcludeTimeEntryCommand = new DelegateCommand(ExecuteExcludeTimeEntry);
        }

        public TimeEntryListItemViewModel(CreditNote timeEntry, InvoiceListItemView invoice, IUserRepository userRepository, IDataService dataService)
        {
            _timeEntry = ConvertCreditNoteToTimeEntry(timeEntry);
            _invoice = invoice;
            _userRepository = userRepository;
            _dataService = dataService;
            ExcludeTimeEntryCommand = new DelegateCommand(ExecuteExcludeTimeEntry, () => TimeEntry.IsStopped);
        }

        private void ExecuteExcludeTimeEntry()
        {
            
            InternalCommands.ExcludeTimeEntry.Execute(this);
        }

        private TimeEntry ConvertCreditNoteToTimeEntry(CreditNote timeEntry)
        {
            //TODO refactor, maybe remake _dataservice.GetFinalizedInvoiceDataByInvoiceId
            var time = new TimeEntry
                           {
                               CreateDate = timeEntry.CreateDate,
                               StartTime = (DateTime)timeEntry.StartTime,
                               Description = timeEntry.Description,
                               TimeSpent = (double)timeEntry.TimeSpent,
                               Price = (double)timeEntry.Price,
                               UserID = (int)timeEntry.UserID,
                               TaskID = (int)timeEntry.TaskID,
                               Billable = (bool)timeEntry.Billable,
                               Task = timeEntry.TimeEntries.Task
                           };

            return time;
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
                return _timeEntry.Task.Project.ProjectName;
            }
        }

        public string TaskName
        {
            get
            {
                return _timeEntry.Task.TaskName;
            }
        }

        public string Description
        {
            get { return _timeEntry.Description; }
            set
            {
                _timeEntry.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public string BillableTime
        {
            get { return _timeEntry.TimeSpent.ToString("N"); }
        }

        public double Price
        {
            get { return Math.Round(_timeEntry.Price, 2); }
            set
            {
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
                OnPropertyChanged("Billable");
            }
        }

        public bool Approved
        {
            get { return _timeEntry.InvoiceId.HasValue; }
            //set
            //{
            //    if (value)
            //    {
            //        _timeEntry.InvoiceId = _invoice.ID;
            //        if (_invoice.TimeEntries.SingleOrDefault(te => te.TimeEntryID == _timeEntry.TimeEntryID) == null)
            //            _invoice.TimeEntries.Add(_timeEntry);
            //    }
            //    else
            //    {
            //        _timeEntry.InvoiceId = null;
            //        var existingTimeEntry =
            //            _invoice.TimeEntries.SingleOrDefault(te => te.TimeEntryID == _timeEntry.TimeEntryID);

            //        if (existingTimeEntry != null)
            //            _invoice.TimeEntries.Remove(existingTimeEntry);
            //    }
            //    OnPropertyChanged("Approved");
            //}
        }

        public DelegateCommand ExcludeTimeEntryCommand { get; set; }

        protected override void OnPropertyChanged(string propertyName)
        {
            _dataService.SaveTimeEntry(_timeEntry).Subscribe(asd =>
                                                                 {
                                                                     InternalCommands.GenerateInvoiceLines.Execute(null);
                                                                     //InternalCommands.UpdateExclVAT.Execute(TimeEntry.InvoiceId);
                                                                 });
            base.OnPropertyChanged(propertyName);
        }
    }
}