#region

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Infrastructure.Implemented;
using Trex.ServiceContracts;

#endregion

namespace Trex.Invoices.InvoiceTemplatesScreen.InvoiceTemplatesListView
{
    public class InvoiceTemplatesListViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IUserSession _userSession;
        private ObservableCollection<InvoiceTemplate> _invoiceTemplates;
        private InvoiceTemplate _selectedTemplate;
        private bool _uploadVisible;
        private int _lastSelection;

        public DelegateCommand<object> CreateNewTemplateCommand { get; set; }
        public DelegateCommand<object> UploadTemplateCommand { get; set; }
        public DelegateCommand<object> SaveCommand { get; set; }
        public DelegateCommand<object> DeleteCommand { get; set; }
        public DelegateCommand<object> UploadInvoiceTemplate { set; get; }
        public DelegateCommand<object> DownloadTemplateCommand { get; set; }

        public InvoiceTemplatesListViewModel(IDataService dataService, IUserSession userSession)
        {
            _dataService = dataService;
            _userSession = userSession;
            CreateNewTemplateCommand = new DelegateCommand<object>(ExecuteCreateNewTemplate);
            UploadTemplateCommand = new DelegateCommand<object>(ExecuteUploadTemplateCommand, CanUploadTemplate);
            SaveCommand = new DelegateCommand<object>(ExecuteSaveCommand, CanSave);
            DeleteCommand = new DelegateCommand<object>(ExecuteDeleteCommand, CanDelete);
            LoadTemplates();
            UploadInvoiceTemplate = new DelegateCommand<object>(ExecuteUploadInvoiceTemplate, canExecuteUploadInvoiceTemplate);
            DownloadTemplateCommand = new DelegateCommand<object>(ExecuteDownloadTemplate, CanDownloadTemplate);
        }

        public InvoiceTemplate SelectedTemplate
        {
            get { return _selectedTemplate; }
            set
            {
                _selectedTemplate = value;
                DownloadTemplateCommand.RaiseCanExecuteChanged();
                UploadInvoiceTemplate.RaiseCanExecuteChanged();
                Update();
            }
        }

        public ObservableCollection<InvoiceTemplate> InvoiceTemplates
        {
            get { return _invoiceTemplates; }
            set
            {
                _invoiceTemplates = value;
                OnPropertyChanged("InvoiceTemplates");
            }
        }

        public string TemplateName
        {
            get
            {
                if (_selectedTemplate != null)
                    return _selectedTemplate.TemplateName;
                return string.Empty;
            }
            set
            {
                _selectedTemplate.TemplateName = value;
                OnPropertyChanged("TemplateName");
            }
        }

        public bool TemplateStandardPrint
        {
            get
            {
                if (SelectedTemplate != null)
                    return SelectedTemplate.StandardInvoicePrint;
                return false;
            }
            set
            {
                SelectedTemplate.StandardInvoicePrint = value;
                UploadInvoiceTemplate.RaiseCanExecuteChanged();
                OnPropertyChanged("TemplateStandardPrint");
            }
        }

        public bool TemplateStandardMail
        {
            get
            {
                if (SelectedTemplate != null)
                    return SelectedTemplate.StandardInvoiceMail;
                return false;
            }
            set
            {
                SelectedTemplate.StandardInvoiceMail = value;
                UploadInvoiceTemplate.RaiseCanExecuteChanged();
                OnPropertyChanged("TemplateStandardMail");
            }
        }

        public bool TemplateStandardSpecification
        {
            get
            {
                if (SelectedTemplate != null)
                    return SelectedTemplate.StandardSpecification;
                return false;
            }
            set
            {
                SelectedTemplate.StandardSpecification = value;
                UploadInvoiceTemplate.RaiseCanExecuteChanged();
                OnPropertyChanged("TemplateStandardSpecification");
            }
        }

        public bool TemplateCreditNoteMail
        {
            get
            {
                if (SelectedTemplate != null)
                    return SelectedTemplate.StandardCreditNoteMail;
                return false;
            }
            set
            {
                SelectedTemplate.StandardCreditNoteMail = value;
                UploadInvoiceTemplate.RaiseCanExecuteChanged();
                OnPropertyChanged("TemplateCreditNoteMail");
            }
        }

        public bool TemplateCreditNotePrint
        {
            get
            {
                if (SelectedTemplate != null)
                    return SelectedTemplate.StandardCreditNotePrint;
                return false;
            }
            set
            {
                SelectedTemplate.StandardCreditNotePrint = value;
                UploadInvoiceTemplate.RaiseCanExecuteChanged();
                OnPropertyChanged("TemplateCreditNotePrint");
            }
        }

        public bool IsTemplateSelected
        {
            get { return _selectedTemplate != null; }
        }

        public bool UploadVisible
        {
            get { return _uploadVisible; }
            set
            {
                _uploadVisible = value;
                OnPropertyChanged("UploadVisible");
            }
        }

        #region Commands

        private void ExecuteDeleteCommand(object obj)
        {
            var msgResult = MessageBox.Show("Are you sure you want to delete the template?", "Delete template",
                                         MessageBoxButton.OKCancel);

            if (msgResult == MessageBoxResult.OK)
                _dataService.DeleteInvoiceTemplate(SelectedTemplate)
                    .Subscribe(
                        result => LoadTemplates()
                    );
        }

        private void ExecuteSaveCommand(object obj)
        {
            _lastSelection = SelectedTemplate.TemplateId;
            _dataService.SaveInvoiceTemplate(SelectedTemplate)
                .Subscribe(result => LoadTemplates(_lastSelection));
        }

        private void ExecuteUploadTemplateCommand(object obj)
        {
            UploadVisible = false;
        }

        private void ExecuteCreateNewTemplate(object obj)
        {
            var invoiceTemplate = new InvoiceTemplate
                                      {
                                          CreateDate = DateTime.Now,
                                          CreatedBy = UserContext.Instance.User.Name,
                                          TemplateName = "No name"
                                      };

            _dataService.SaveInvoiceTemplate(invoiceTemplate).Subscribe(i => LoadTemplates());
        }

        private void ExecuteUploadInvoiceTemplate(object obj)
        {
            try
            {
                // Create an instance of the open file dialog box.
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Multiselect = false;
                dlg.Filter = "Word templates (.dotx)|*.dotx|Text files (.txt.)|*.txt";

                bool? userClickedOK = dlg.ShowDialog();

                if (userClickedOK == true)
                {
                    //UploadFile(SelectedTemplate.TemplateId, dlg.File.OpenRead());
                    var data = SaveFile(dlg.File.OpenRead());

                    int templateType = 0;

                    if (TemplateStandardPrint)
                        templateType = 1;
                    else if (TemplateStandardMail)
                        templateType = 2;
                    else if (TemplateStandardSpecification)
                        templateType = 3;
                    else if (TemplateCreditNoteMail)
                        templateType = 4;
                    else if (TemplateCreditNotePrint)
                        templateType = 5;

                    _dataService.ValidateTemplate(data, templateType).Subscribe(r =>
                                                                      {
                                                                          if (r.Success)
                                                                          {
                                                                              _dataService.SaveInvoiceTemplateFile(data, SelectedTemplate.TemplateId).Subscribe();
                                                                              Execute.InUIThread(() => MessageBox.Show(r.Response));
                                                                          }

                                                                          else
                                                                              Execute.InUIThread(() => MessageBox.Show(r.Response));

                                                                      });


                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("The file cannot be read. Please close the file.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool canExecuteUploadInvoiceTemplate(object obj)
        {
            if (TemplateCreditNoteMail || TemplateCreditNotePrint || TemplateStandardMail || TemplateStandardPrint || TemplateStandardSpecification)
                return true;
            return false;
        }

        private void ExecuteDownloadTemplate(object obj)
        {
            if (_selectedTemplate != null)
            {
                var dlg = new SaveFileDialog { Filter = "Word templates (*.dotx)|*.dotx", };

                if (dlg.ShowDialog() == true)
                {
                    _dataService.Downloadtemplate(SelectedTemplate.TemplateId)
                        .Subscribe(r =>
                                       {
                                           if (r == null)
                                               MessageBox.Show("No file to download", "Error", MessageBoxButton.OK);
                                           else
                                               using (Stream sw = (Stream)dlg.OpenFile())
                                               {
                                                   sw.Write(r, 0, r.Length);
                                                   sw.Flush();
                                                   sw.Close();
                                               }
                                       });

                }
            }
        }

        private bool CanSave(object arg)
        {
            return _selectedTemplate != null;
        }

        private bool CanDelete(object arg)
        {
            return IsTemplateSelected;
        }

        private bool CanUploadTemplate(object arg)
        {
            return IsTemplateSelected;
        }

        private bool CanDownloadTemplate(object arg)
        {
            return _selectedTemplate != null;
        }

        #endregion

        private void LoadTemplates(int lastSelection = 0)
        {
            _dataService.GetAllInvoiceTemplates().Subscribe(
                templates =>
                {
                    InvoiceTemplates = new ObservableCollection<InvoiceTemplate>();
                    foreach (var template in templates)
                    {
                        InvoiceTemplates.Add(template);
                    }
                    if (lastSelection != 0)
                        SelectedTemplate = InvoiceTemplates.First(x => x.TemplateId == _lastSelection);
                }
                );
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            UploadTemplateCommand.RaiseCanExecuteChanged();

            DeleteCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }

        private byte[] SaveFile(Stream stream)
        {
            byte[] buffer;

            var binaryReader = new BinaryReader(stream);
            var totalBytes = stream.Length;

            // read entire file into buffer
            buffer = binaryReader.ReadBytes((Int32)totalBytes);

            binaryReader.Close();

            return buffer;
        }
    }
}