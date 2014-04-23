#region

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.Invoices.Commands;
using Trex.ServiceContracts;
using Trex.ServiceContracts.Model;

#endregion

namespace Trex.Invoices.Dialogs.EditInvoiceLinesView
{
    public class EditInvoiceLinesViewModel : ViewModelBase, IViewModel
    {
        private readonly IDataService _dataService;
        private readonly DelegateCommand<InvoiceLine> _invoiceLineChanged;
        private readonly DelegateCommand<TimeEntry> _timeEntryChanged;
        private readonly IUserRepository _userRepository;
        private Invoice _invoice;

        private ObservableCollection<InvoiceLineListItemViewModel> _invoiceLines;
        private bool _selectAll;
        private InvoiceLineListItemViewModel _selectedInvoiceLine;
        private ObservableCollection<TimeEntryListItemViewModel> _timeEntries;

        public EditInvoiceLinesViewModel(Invoice invoice, IDataService dataService, IUserRepository userRepository)
        {
            _invoice = invoice;
            _dataService = dataService;
            _userRepository = userRepository;
            _timeEntryChanged = new DelegateCommand<TimeEntry>(ExecuteTimeEntryChanged);
            _invoiceLineChanged = new DelegateCommand<InvoiceLine>(ExecuteInvoiceLineChanged);

            //SaveAndCloseCommand = new DelegateCommand<object>(ExecuteSaveAndClose, CanApplyChanges);
            ApplyChangesCommand = new DelegateCommand<object>(ExecuteApplyChanges, CanApplyChanges);
            CancelChangesCommand = new DelegateCommand<object>(ExecuteCancelChanges);

            CreateNewInvoiceLine = new DelegateCommand<object>(ExecuteCreateNewInvoiceLine, CanCreateNewInvoiceLine);

            DeleteInvoiceLine = new DelegateCommand<object>(ExecuteDeleteInvoiceLine, CanDeleteInvoiceLine);

            InternalCommands.TimeEntryChanged.RegisterCommand(_timeEntryChanged);
            InternalCommands.InvoiceLineChanged.RegisterCommand(_invoiceLineChanged);

            LoadTimeEntries();
            LoadInvoiceLines();
        }

        public DelegateCommand<object> ApplyChangesCommand { get; set; }
        public DelegateCommand<object> CancelChangesCommand { get; set; }
        public DelegateCommand<object> CreateNewInvoiceLine { get; set; }
        public DelegateCommand<object> EditInvoiceLine { get; set; }
        public DelegateCommand<object> DeleteInvoiceLine { get; set; }
        //public DelegateCommand<object> SaveAndCloseCommand { get; set; }

        public bool SelectAll
        {
            get { return _selectAll; }
            set
            {
                _selectAll = value;

                ToggleSelections(value);
                OnPropertyChanged("SelectAll");
            }
        }

        public InvoiceLineListItemViewModel SelectedInvoiceLine
        {
            get { return _selectedInvoiceLine; }
            set
            {
                _selectedInvoiceLine = value;
                OnPropertyChanged("SelectedInvoiceLine");
                DeleteInvoiceLine.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<InvoiceLineListItemViewModel> InvoiceLines
        {
            get { return _invoiceLines; }
            set
            {
                _invoiceLines = value;
                OnPropertyChanged("InvoiceLines");
            }
        }

        public ObservableCollection<TimeEntryListItemViewModel> TimeEntries
        {
            get { return _timeEntries; }
            set
            {
                _timeEntries = value;
                OnPropertyChanged("TimeEntries");
            }
        }

        #region IViewModel Members

        public override void Close()
        {
            base.Close();
            InternalCommands.TimeEntryChanged.UnregisterCommand(_timeEntryChanged);
            InternalCommands.InvoiceLineChanged.UnregisterCommand(_invoiceLineChanged);
        }

        #endregion

        private void ExecuteSaveAndClose(object obj)
        {
            ExecuteApplyChanges(obj);
            InternalCommands.ManageInvoiceLinesCompleted.Execute(null);
        }

        private void ExecuteInvoiceLineChanged(InvoiceLine obj)
        {
            _dataService.SaveInvoiceLine(obj);
            ApplyChangesCommand.RaiseCanExecuteChanged();
            //SaveAndCloseCommand.RaiseCanExecuteChanged();
        }

        private void ExecuteCancelChanges(object obj)
        {
            foreach (var timeEntryListItemViewModel in TimeEntries)
            {
                timeEntryListItemViewModel.TimeEntry.CancelChanges();

                timeEntryListItemViewModel.Update();
            }

            foreach (var invoiceLine in _invoice.InvoiceLines)
            {
                invoiceLine.CancelChanges();
            }

            _invoice.CancelChanges();

            InternalCommands.ManageInvoiceLinesCompleted.Execute(null);
        }

        private void ExecuteTimeEntryChanged(TimeEntry timeEntry)
        {
            ApplyChangesCommand.RaiseCanExecuteChanged();
            //SaveAndCloseCommand.RaiseCanExecuteChanged();
        }

        private bool CanApplyChanges(object arg)
        {
            return true;
            //return (_invoice.HasChanges || TimeEntries != null && _invoice.TimeEntries.Any(t => t.HasChanges))
            //       || (InvoiceLines != null && _invoice.InvoiceLines.Any(i => i.HasChanges));
            return true;
        }

        private void LoadInvoiceLines()
        {
            InvoiceLines = new ObservableCollection<InvoiceLineListItemViewModel>();
            foreach (var invoiceLine in _invoice.InvoiceLines)
            {
                invoiceLine.AcceptChanges();
                InvoiceLines.Add(new InvoiceLineListItemViewModel(invoiceLine));
            }
            ApplyChangesCommand.RaiseCanExecuteChanged();
            //SaveAndCloseCommand.RaiseCanExecuteChanged();
        }

        private void LoadTimeEntries()
        {
            _dataService.GetTimeEntriesForInvoicing(_invoice.StartDate, _invoice.EndDate, _invoice.CustomerInvoiceGroupId).Subscribe
                (
                    notBookedTimeEntries =>
                        {
                            TimeEntries = new ObservableCollection<TimeEntryListItemViewModel>();

                            foreach (var timeEntry in notBookedTimeEntries)
                            {
                                timeEntry.AcceptChanges();
                                TimeEntries.Add(new TimeEntryListItemViewModel(timeEntry, _invoice, _userRepository));
                            }

                            foreach (var timeEntry in _invoice.TimeEntries)
                            {
                                timeEntry.AcceptChanges();
                                TimeEntries.Add(new TimeEntryListItemViewModel(timeEntry, _invoice, _userRepository));
                            }
                            ApplyChangesCommand.RaiseCanExecuteChanged();
                            //SaveAndCloseCommand.RaiseCanExecuteChanged();
                        }
                );
        }

        private bool CanDeleteInvoiceLine(object arg)
        {
            return _selectedInvoiceLine != null;
        }

        private void ExecuteDeleteInvoiceLine(object obj)
        {
            _selectedInvoiceLine.InvoiceLine.MarkAsDeleted();

            InvoiceLines.Remove(_selectedInvoiceLine);
            ApplyChangesCommand.RaiseCanExecuteChanged();
            //SaveAndCloseCommand.RaiseCanExecuteChanged();
            //_dataService.SaveInvoice(_invoice).Subscribe(
            //    result =>
            //    {
            //        _invoice = result;

            //        _invoice.MarkAsUnchanged();
            //        _invoice.AcceptChanges();
            //        this.Update();
            //        LoadInvoiceLines();
            //        LoadTimeEntries();
            //    }

            //    );
        }

        private void ExecuteCreateNewInvoiceLine(object obj)
        {
            var invoiceLine = new InvoiceLine { InvoiceID = _invoice.ID, UnitType = (int)UnitTypes.Other, Text = string.Empty };
            _invoice.InvoiceLines.Add(invoiceLine);
            InvoiceLines.Add(new InvoiceLineListItemViewModel(invoiceLine));
            ApplyChangesCommand.RaiseCanExecuteChanged();
            //SaveAndCloseCommand.RaiseCanExecuteChanged();
        }

        private bool CanCreateNewInvoiceLine(object arg)
        {
            return true;
        }

        private void ToggleSelections(bool select)
        {
            foreach (var timeEntryListItemViewModel in TimeEntries)
            {
                timeEntryListItemViewModel.Approved = select;
            }
        }

        private void ExecuteApplyChanges(object obj)
        {
            //if (TimeEntries.Any(t => t.HasChanges))
            //{
            //    ApplicationCommands.SystemBusy.Execute("Generating invoice lines");
            //    _dataService.RecalculateInvoice(_invoice).Subscribe(
            //        invoice =>
            //            {
            //                _invoice = invoice;

            //                _invoice.AcceptChanges();
            //                this.Update();
            //                LoadInvoiceLines();
            //                LoadTimeEntries();

            //                ApplicationCommands.SystemIdle.Execute(null);
            //            }
            //        );
            //}
            //else
            //{
            //    _dataService.SaveInvoice(_invoice).Subscribe(
            //        invoice =>
            //            {
            //                _invoice = invoice;

            //                _invoice.AcceptChanges();
            //                this.Update();
            //                LoadInvoiceLines();
            //                LoadTimeEntries();

            //                ApplicationCommands.SystemIdle.Execute(null);
            //            }
            //        );
            //}
        }
    }
}
