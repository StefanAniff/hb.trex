#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.Infrastructure.Implemented;
using Trex.Invoices.Commands;
using Trex.Invoices.Interfaces;
using Trex.Invoices.InvoiceManagementScreen.Interfaces;
using Trex.ServiceContracts;

#endregion

namespace Trex.Invoices.InvoiceManagementScreen.InvoiceView
{
    public class InvoiceViewModel : ViewModelBase, IInvoiceViewModel
    {
        private readonly IDataService _dataService;
        private readonly IUserRepository _userRepository;
        private readonly IUserSession _userSession;
        private ObservableCollection<CustomerInvoiceGroup> _customerInvoiceGroup;
        private ObservableCollection<InvoiceLineListItemViewModel> _invoiceLine;

        private ObservableCollection<InvoiceListItemViewModel> _invoices;
        private InvoiceListItemViewModel _selectedInvoice;
        private InvoiceLineListItemViewModel _selectedInvoiceLine;
        private ObservableCollection<InvoiceListItemViewModel> _selectedinvoices;
        private ObservableCollection<TimeEntryListItemViewModel> _timeEntries;

        public DelegateCommand<object> DownloadSpecificationCommand { get; set; }
        public DelegateCommand<object> DownloadInvoiceCommand { get; set; }
        public DelegateCommand<object> RecalculateInvoices { get; set; }
        
        public InvoiceViewModel(IDataService dataService, IUserSession userSession, IUserRepository userRepository)
        {
            _dataService = dataService;
            _userSession = userSession;
            _userRepository = userRepository;

            InvoiceLines = new ObservableCollection<InvoiceLineListItemViewModel>();
            TimeEntries = new ObservableCollection<TimeEntryListItemViewModel>();

            InternalCommands.EditInvoiceCompleted.RegisterCommand(
                new DelegateCommand<InvoiceListItemView>(ExecuteEditInvoiceCompleted));
            InternalCommands.GenerateInvoiceLines.RegisterCommand(
                new DelegateCommand<object>(ExecuteGenerateInvoiceLines));

            InternalCommands.FindInvoiceFromID.RegisterCommand(new DelegateCommand<int?>(FindInvoiceFromID));

            InternalCommands.GetInvoicesFromMany.RegisterCommand(
                new DelegateCommand<ObservableCollection<CustomersInvoiceView>>(ExecuteGetInvoicesFromMany));
            InternalCommands.SendInvoiceEmail.RegisterCommand(new DelegateCommand<object>(ExecuteSendInvoiceEmail));

            InternalCommands.LoadInvoiceLines.RegisterCommand(
                new DelegateCommand<InvoiceListItemViewModel>(LoadInvoiceLines));

            InternalCommands.LoadTimeEntries.RegisterCommand(
                new DelegateCommand<InvoiceListItemViewModel>(LoadTimeEntries));
            InternalCommands.FinalizeInvoice.RegisterCommand(new DelegateCommand<int?>(FinalizeInvoice));
            InternalCommands.GenerateCreditnote.RegisterCommand(new DelegateCommand<object>(GenerateCreditnote));

            InternalCommands.UpdateSelectedInvoiceItems.RegisterCommand(
                new DelegateCommand<ObservableCollection<object>>(UpdateSelectedItems));
            InternalCommands.AddDummyInvoiceLines.RegisterCommand(new DelegateCommand(AddDummyInvoiceLines));

            InternalCommands.RemoveInvoiceFromInvoicesList.RegisterCommand(
                new DelegateCommand<InvoiceListItemViewModel>(RemoveInvoiceFromInvoicesList));

            InternalCommands.PreviewInvoice.RegisterCommand(new DelegateCommand<object>(PreviewInvoice));
            InternalCommands.PreviewSpecification.RegisterCommand(new DelegateCommand<object>(PreviewSpecification));

            InternalCommands.ReloadInvoices.RegisterCommand(new DelegateCommand<object>(ExecuteReloadInvoices));

            InternalCommands.SeeAllInvoices.RegisterCommand(new DelegateCommand<bool?>(SeeAllInvoices));
            InternalCommands.ResetSelectedInvoices.RegisterCommand(new DelegateCommand<object>(ResetLastInvoiceSelected));

            InternalCommands.SeeDebitList.RegisterCommand(new DelegateCommand<bool?>(SeeDebitList));

            InternalCommands.RegenerateInvoiceFiles.RegisterCommand(new DelegateCommand(RegenerateInvoiceFiles));

            InternalCommands.SaveInvoiceComment.RegisterCommand(new DelegateCommand<string>(SaveInvoiceComment));
            
            InternalCommands.ExcludeTimeEntry.RegisterCommand(new DelegateCommand<TimeEntryListItemViewModel>(ExcludeTimeEntry));
          
            LoadInvoiceTemplates();
            _getListOfInvoicesToFinalize = new ObservableCollection<int>();
        }
        
        private void ExcludeTimeEntry(TimeEntryListItemViewModel timeEntry)
        {
            if (timeEntry.TimeEntry.IsStopped) return;
           
                _dataService.ExcludeTimeEntry(timeEntry.TimeEntry);

                TimeEntries.Remove(timeEntry);
                InternalCommands.GenerateInvoiceLines.Execute(null);
                //_invoiceService.RecalculateInvoice(SelectedInvoice.Invoice);
        }

        private void SaveInvoiceComment(string comment)
        {
            if (SelectedInvoice.Id != null)
                _dataService.SaveInvoiceComment(comment, (int) SelectedInvoice.Id, _userSession.CurrentUser.UserID);
        }

        private void SeeDebitList(bool? value)
        {

            if (value == false)
            {
                InternalCommands.UpdateInvoiceList.Execute(null);
                return;
            }

            if (Invoices != null)
                Invoices.Clear();
            else
                Invoices = new ObservableCollection<InvoiceListItemViewModel>();
            _dataService.GetDebitList().Subscribe(i =>
                                                      {
                                                          if (i == null)
                                                              return;
                                                          foreach (var invoiceListItemView in i)
                                                          {
                                                              Invoices.Add(new InvoiceListItemViewModel(invoiceListItemView, _dataService));
                                                          }

                                                      });

        }

        private void FindInvoiceFromID(int? obj)
        {
            if (obj != null)
                _dataService.GetInvoicebyInvoiceID((int)obj)
                    .Subscribe(i =>
                                   {
                                       Invoices = new ObservableCollection<InvoiceListItemViewModel>();
                                       Invoices.Add(new InvoiceListItemViewModel(i, _dataService));
                                   });

        }

        public ObservableCollection<InvoiceTemplate> InvoiceTemplates { get; set; }

        public InvoiceListItemViewModel SelectedInvoice
        {
            get { return _selectedInvoice; }
            set
            {
                TimeEntries = new ObservableCollection<TimeEntryListItemViewModel>();
                InvoiceLines = new ObservableCollection<InvoiceLineListItemViewModel>();
                if (value != null)
                {
                    _selectedInvoice = value;
                    OnPropertyChanged("SelectedInvoice");
                    LoadInvoiceLines(_selectedInvoice);
                    LoadTimeEntries(_selectedInvoice);
                    InternalCommands.InvoiceSelected.Execute(SelectedInvoice.Invoice);
                }
            }
        }

        public ObservableCollection<InvoiceListItemViewModel> Invoices
        {
            get { return _invoices; }
            set
            {
                _invoices = value;
                OnPropertyChanged("Invoices");
            }
        }

        public void PreviewInvoice(object obj)
        {
            if (SelectedInvoice != null)
            {
                _dataService.PreviewInvoice((Guid)SelectedInvoice.Guid, 2);
            }
        }

        public void PreviewSpecification(object obj)
        {
            if (SelectedInvoice != null)
            {
                _dataService.PreviewInvoice((Guid)SelectedInvoice.Guid, 3);
            }
        }

        public InvoiceLineListItemViewModel SelectedInvoiceLine
        {
            get { return _selectedInvoiceLine; }
            set
            {
                if (value != null)
                {
                    _selectedInvoiceLine = value;
                    //_selectedInvoiceLine.InvoiceLine.AcceptChanges();
                    OnPropertyChanged("SelectedInvoice");
                }
            }
        }

        public ObservableCollection<InvoiceLineListItemViewModel> InvoiceLines
        {
            get { return _invoiceLine; }
            set
            {
                _invoiceLine = value;
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

        public ObservableCollection<CustomerInvoiceGroup> CustomerInvoiceGroup
        {
            get { return _customerInvoiceGroup; }
            set
            {
                _customerInvoiceGroup = value;
                OnPropertyChanged("CustomerInvoiceGroup");
            }
        }

        private void UpdateSelectedItems(ObservableCollection<object> obj)
        {
            _selectedinvoices = new ObservableCollection<InvoiceListItemViewModel>();
            foreach (var o in obj.Cast<InvoiceListItemViewModel>())
            {
                _selectedinvoices.Add(o);
            }
        }

        public void RemoveInvoiceFromInvoicesList(InvoiceListItemViewModel obj)
        {
            Invoices.Remove(obj);
        }

        private ObservableCollection<int> _getListOfInvoicesToFinalize;
        public void GetListOfInvoicesToFinalize(int? specificInvoice = null)
        {
            if (specificInvoice == null)
            {
                foreach (var invoiceListItemViewModel in _selectedinvoices)
                {
                    _getListOfInvoicesToFinalize.Add((int)invoiceListItemViewModel.Id);
                }
            }
            else
                _getListOfInvoicesToFinalize.Add((int)specificInvoice);
        }

        public void FinalizeInvoice(int? previewMode)
        {
            try
            {
                ApplicationCommands.SystemBusy.Execute("Generating invoice pdf");

                var temp = new ObservableCollection<int>();
                if (_getListOfInvoicesToFinalize.Count == 0)
                {
                    GetListOfInvoicesToFinalize();
                    temp = _getListOfInvoicesToFinalize;
                }
                else
                    temp = _getListOfInvoicesToFinalize;

                bool isPreview = (previewMode == 0 ? false : true);

                _dataService.FinalizeInvoices(temp, isPreview).Subscribe(x =>
                {
                    if (x == null)
                    {
                        ApplicationCommands.SystemIdle.Execute(null);
                        MessageBox.Show("No files to be printed", "No files", MessageBoxButton.OK);
                    }
                    else
                    {
                        var toPrint = new List<int>();

                        ApplicationCommands.SystemIdle.Execute(null);
                        foreach (var response in x)
                        {
                            if (!response.Success)
                                MessageBox.Show(
                                    "Invoice " + response.InvoiceId + " could not be created.\n" +
                                    response.Response, "Error", MessageBoxButton.OK);
                            else
                            {
                                if (previewMode == 1)
                                    PreviewInvoice(null);
                                else if (previewMode == 2)
                                    PreviewSpecification(null);

                                if (response.ToPrint)
                                    toPrint.Add(response.InvoiceId);
                            }
                        }
                        if (!isPreview)
                            ExecuteReloadInvoices(null);
                    }
                    _getListOfInvoicesToFinalize = new ObservableCollection<int>();
                    InternalCommands.ReloadInvoiceSearchFilter.Execute(null);
                    ApplicationCommands.SystemIdle.Execute(null);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExecuteSendInvoiceEmail(object obj)
        {
            foreach (var invoiceListItemViewModel in _selectedinvoices)
            {
                if (invoiceListItemViewModel.Id != null)
                    _dataService.GetInvoiceById((int)invoiceListItemViewModel.Id).Subscribe(r =>
                        _dataService.SendInvoiceMail(r).Subscribe(s =>
                        {
                            if (!s.Success)
                            {
                                Execute.InUIThread(() => MessageBox.Show(s.Response));
                                return;
                            }
                            Execute.InUIThread(() => MessageBox.Show(s.Response));
                            Execute.InUIThread(() => InternalCommands.ReloadInvoices.Execute(null));
                        }));
            }
        }

        private int? _lastInvoiceSelectedId;
        
        private void ExecuteReloadInvoices(object obj)
        {
            _lastInvoiceSelectedId = SelectedInvoice.Id;

            InternalCommands.RefreshCustomers.Execute(null);
            InternalCommands.UpdateInvoiceList.Execute(null);
        }

        private void LoadTimeEntries(InvoiceListItemViewModel invoice)
        {
            if (invoice.InvoiceID == "Draft" && !invoice.IsCreditNote)
                _dataService.GetInvoiceDataByInvoiceId(invoice.Id.GetValueOrDefault()).Subscribe(
                    re =>
                    {
                        if (TimeEntries.Count > 0)
                            TimeEntries.Clear();

                        foreach (var timeEntry in re)
                        {
                            timeEntry.AcceptChanges();
                            TimeEntries.Add(new TimeEntryListItemViewModel(timeEntry, SelectedInvoice.Invoice,
                                                                           _userRepository, _dataService));
                        }
                    });
            else
                _dataService.GetFinalizedInvoiceDataByInvoiceId(invoice.Id.GetValueOrDefault()).Subscribe(
                    re =>
                    {
                        if (TimeEntries.Count > 0)
                            TimeEntries.Clear();

                        foreach (var timeEntry in re)
                        {
                            timeEntry.AcceptChanges();
                            TimeEntries.Add(new TimeEntryListItemViewModel(timeEntry, SelectedInvoice.Invoice,
                                                                           _userRepository, _dataService));
                        }
                    });
        }

        public void LoadInvoiceLines(InvoiceListItemViewModel invoice)
        {
            if (invoice == null)
                invoice = SelectedInvoice;

            _dataService.GetInvoiceLinesByInvoiceID((int)invoice.Id).Subscribe(
                re =>
                {
                    if (InvoiceLines.Count > 0)
                        InvoiceLines.Clear();
                    foreach (var invoiceLine in re)
                    {
                        invoiceLine.AcceptChanges();
                        InvoiceLines.Add(new InvoiceLineListItemViewModel(invoiceLine, _dataService));
                    }
                    AddDummyInvoiceLines();
                });
        }

        public void AddDummyInvoiceLines()
        {
            if (SelectedInvoice.InvoiceID == "Draft")
            {
                var test = new InvoiceLineListItemViewModel(new InvoiceLine { InvoiceID = (int)SelectedInvoice.Id },
                                                            _dataService) { IsTemp = true, UnitType = 1 };
                InvoiceLines.Add(test);
            }
        }

        private void LoadInvoiceTemplates()
        {
            _dataService.GetAllInvoiceTemplates().Subscribe(
                templates => { InvoiceTemplates = templates; }
                );
        }

        private void ExecuteEditInvoiceCompleted(InvoiceListItemView obj)
        {
            var invoiceLine = Invoices.SingleOrDefault(i => i.Id == obj.ID);
            if (invoiceLine == null)
                Invoices.Add(new InvoiceListItemViewModel(obj, _dataService));
        }

        private void ResetLastInvoiceSelected(object obj)
        {
            _lastInvoiceSelectedId = null;
        }

        private void GenerateCreditnote(object obj)
        {
            _dataService.GenerateCreditnote((int)SelectedInvoice.Id, _userSession.CurrentUser.UserID).Subscribe(
                s =>
                {
                    if (!s.Success)
                        MessageBox.Show(s.Response, "Error", MessageBoxButton.OK);
                    else
                    {
                        GetListOfInvoicesToFinalize(s.InvoiceId);
                        FinalizeInvoice(0);
                    }
                });
        }

        private void ExecuteGetInvoicesFromMany(ObservableCollection<CustomersInvoiceView> t)
        {
            try
            {
                var tmp = new ObservableCollection<int>();
                foreach (var customerInvoiceView in t)
                {
                    tmp.Add(customerInvoiceView.CustomerID);
                }

                _dataService.GetInvoicesByCustomerId(tmp).Subscribe(i =>
                    {
                        if (i != null)
                        {
                            Invoices = SeeAll == true ? TakeAll(i) : TakeOnlyNoneClosed(i);

                            if (_lastInvoiceSelectedId != null &&
                                Invoices.Select(id => id.Id).Contains(_lastInvoiceSelectedId))
                                SelectedInvoice = Invoices.First(x => x.Id == _lastInvoiceSelectedId);

                            if (SelectedInvoice != null && _lastInvoiceSelectedId != null)
                            {
                                LoadInvoiceLines(SelectedInvoice);
                                LoadTimeEntries(SelectedInvoice);
                            }
                            else
                            {
                                TimeEntries.Clear();
                                InvoiceLines.Clear();
                            }
                        }
                        else
                            MessageBox.Show("Something went wrong in a call on the server", "Error", MessageBoxButton.OK);
                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        private ObservableCollection<InvoiceListItemViewModel> TakeAll(IEnumerable<InvoiceListItemView> invoiceListItemViews)
        {
            var temp = new ObservableCollection<InvoiceListItemViewModel>();
            foreach (var invoiceListItemView in invoiceListItemViews)
            {
                temp.Add(new InvoiceListItemViewModel(invoiceListItemView, _dataService));
            }

            return temp;
        }

        private ObservableCollection<InvoiceListItemViewModel> TakeOnlyNoneClosed(IEnumerable<InvoiceListItemView> invoiceListItemViews)
        {
            var temp = new ObservableCollection<InvoiceListItemViewModel>();
            foreach (var invoiceListItemView in invoiceListItemViews.Where(i => i.Closed == false))
            {
                temp.Add(new InvoiceListItemViewModel(invoiceListItemView, _dataService));
            }

            return temp;
        }

        public bool? SeeAll { get; set; }

        public void SeeAllInvoices(bool? seeAll)
        {
            SeeAll = seeAll;
        }

        private void ExecuteGenerateInvoiceLines(object obj)
        {
            _dataService.GenerateInvoiceLines((int)SelectedInvoice.Id)
                                              .Subscribe(i =>
                                                             {
                                                                 LoadInvoiceLines(SelectedInvoice);
                                                                 InternalCommands.UpdateExclVAT.Execute(SelectedInvoice.Id);
                                                             });
        }
        
        private void RegenerateInvoiceFiles()
        {
            if (SelectedInvoice.Id != null)
            {
                _dataService.DeleteInvoiceFiles((int)SelectedInvoice.Id).Subscribe(r => Execute.InUIThread(() => MessageBox.Show(r.Response)));
            }
        }
        
        public bool IsReadOnly
        {
            get
            {
                if (_selectedInvoice.InvoiceID != "Draft")
                    return true;
                return false;
            }
        }
    }
}

