#region

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Infrastructure.Implemented;
using Trex.Invoices.Commands;
using Trex.ServiceContracts;
using System.Linq;

#endregion

namespace Trex.Invoices.InvoiceManagementScreen.InvoiceView
{
    public class InvoiceListItemViewModel : ViewModelBase
    {
        private readonly IDataService _data;
        public DelegateCommand<object> DeleteInvoice { get; set; }
        public DelegateCommand<object> ToggleDelivered { get; set; }
        
        public InvoiceListItemViewModel(InvoiceListItemView invoice, IDataService dataservice)
        {
            _data = dataservice;
            Invoice = invoice;
            DeleteInvoice = new DelegateCommand<object>(ExecuteDeleteInvoice, CanExecuteDeleteinvoice);
            ToggleDelivered = new DelegateCommand<object>(ExecuteToggleDelivered, CanExecuteToggleDelivered);
            
            InternalCommands.ReloadInvoiceCommentField.RegisterCommand(new DelegateCommand<int?>(ReloadComment));

            ToggleDeliver(false);

            InternalCommands.UpdateExclVAT.RegisterCommand(new DelegateCommand<int?>(UpdateExclVAT));
        }

        private void ReloadComment(int? invoiceId)
        {
            if(invoiceId == this.Id)
                _data.LoadInvoiceComments((int)this.Id).Subscribe(c =>
                                                                      {
                                                                          if(c != null && c.Count != 0)
                                                                            InvoiceComment = c.Last().Comment;
                                                                      });
        }

        private void ToggleDeliver(bool isClicked)
        {
            var mail_notSent = new BitmapImage(new Uri("mail-open.png", UriKind.RelativeOrAbsolute));
            var letter_notSent = new BitmapImage(new Uri("script-text.png", UriKind.RelativeOrAbsolute));
            var sent = new BitmapImage(new Uri("mail-sent.png", UriKind.RelativeOrAbsolute));

            if (isClicked)
            {
                if (Invoice.Delivered)
                {
                    MessageBoxResult msg = MessageBox.Show("Are you sure you want to check this invoices as not sent?", "Unsend invoice",
                                    MessageBoxButton.OKCancel);
                    if (msg == MessageBoxResult.OK)
                    {
                        Invoice.Delivered = false;
                        Invoice.DeliveredDate = null;

                        if (Invoice.SendFormat == 1) //mail
                            DeliveredField = mail_notSent;

                        if (Invoice.SendFormat == 2) //print
                            DeliveredField = letter_notSent;
                    }
                }
                else
                {
                    MessageBoxResult msg = MessageBox.Show("Are you sure you want to check this invoices as sent?", "Send invoice",
                                    MessageBoxButton.OKCancel);
                    if (msg == MessageBoxResult.OK)
                    {
                        Invoice.Delivered = true;
                        Invoice.DeliveredDate = DateTime.Now.Date;
                        DeliveredField = sent;
                    }
                }
            }
            else
            {
                if (!Invoice.Delivered)
                {
                    if (Invoice.SendFormat == 1) //mail
                        DeliveredField = mail_notSent;

                    if (Invoice.SendFormat == 2) //print
                        DeliveredField = letter_notSent;
                }
                else
                    DeliveredField = sent;
            }
            InternalCommands.RaiseSendEmailCanExecute.Execute(null);
        }


        private void ExecuteToggleDelivered(object isClicked)
        {
            ToggleDeliver(true);
        }

        private bool CanExecuteToggleDelivered(object arg)
        {
            if (Invoice.Closed || Invoice.InvoiceID == null)
                return false;

            return true;
        }


        private void UpdateExclVAT(int? obj)
        {
            if (Invoice.ID == obj)
                _data.RecalculateexclVat((int)obj).Subscribe(r => this.ExclVAT = r);
        }

        public InvoiceListItemView Invoice { set; get; }

        public double ButtonOpacity { get; set; }

        private void ExecuteDeleteInvoice(object obj)
        {
            _data.DeleteInvoiceDraft((int)Id).Subscribe(r =>
                                                            {
                                                                InternalCommands.RemoveInvoiceFromInvoicesList.Execute(this);
                                                                InternalCommands.RefreshCustomers.Execute(null);
                                                            });
        }

        private bool CanExecuteDeleteinvoice(object obj)
        {
            if (Invoice.InvoiceID == null)
            {
                ButtonOpacity = 1;
                return true;
            }
            ButtonOpacity = 0;
            return false;
        }

        public Brush Color
        {
            get
            {
                if (ExclVAT < 0)
                    return new SolidColorBrush(Colors.Red);
                return new SolidColorBrush(Colors.Black);
            }
        }

        public Brush DueDateColor
        {
            get
            {
                if(DueDate < DateTime.Now)
                    return new SolidColorBrush(Colors.Red);
                return new SolidColorBrush(Colors.Black);
            }
        }

        public string InvoiceID
        {
            get
            {
                if (Invoice.InvoiceID == null)
                    return "Draft";
                return Invoice.InvoiceID.ToString();
            }
        }

        public string CustomerName
        {
            get
            {
                return Invoice.CustomerName;
            }
        }

        public string CustomerManager
        {
            get
            {
                return Invoice.CustomerManager;
            }
        }

        public ObservableCollection<string> Unit
        {
            get
            {
                var temp = new ObservableCollection<string>();
                return temp;
            }
        }

        public int? Id
        {
            get { return Invoice.ID; }
        }

        public DateTime InvoiceDate
        {
            get { return Invoice.InvoiceDate; }
            set
            {
                Invoice.InvoiceDate = value;
                OnPropertyChanged("InvoiceDate");
            }
        }

        public string DeliveredDate
        {
            get
            {
                if (Invoice.Delivered)
                    return Invoice.DeliveredDate.GetValueOrDefault().ToShortDateString();

                return string.Empty;
            }
        }

        public int? InvoicePeriod
        {
            get { return Invoice.InvoicePeriode; }
        }

        public DateTime? DueDate
        {
            get { return Invoice.DueDate; }
            set
            {
                Invoice.DueDate = value;
                OnPropertyChanged("InvoiceDate");
            }
        }

        [Required]
        public DateTime StartDate
        {
            get { return Invoice.StartDate; }
            set
            {
                if (value > EndDate)
                    return;
                Invoice.StartDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        [Required]
        public DateTime EndDate
        {
            get { return Invoice.EndDate; }
            set
            {
                if (value < StartDate)
                    return;
                Invoice.EndDate = value;
                OnPropertyChanged("EndDate");

            }
        }

        public int CustomerInvoiceGroupId
        {
            get { return Invoice.CustomerInvoiceGroupId; }
        }

        public Guid Guid
        {
            get { return Invoice.Guid; }
        }

        public double? ExclVAT
        {
            get { return Invoice.ExclVAT; }
            set
            {
                Invoice.ExclVAT = value;
                OnPropertyChanged("ExclVAT");
            }
        }

        public string Label
        {
            get
            {
                return Invoice.Label;
            }
        }

        public string Regarding
        {
            get
            {
                return Invoice.Regarding;
            }
            set
            {
                Invoice.Regarding = value;
                OnPropertyChanged("InvoiceDate");
            }
        }

        public string Attention
        {
            get
            {
                if (Invoice.attention == null)
                    return Invoice.CigAttention;
                return Invoice.attention;
            }
        }

        public string SendFormat
        {
            get
            {
                if (Invoice.SendFormat == 1)
                {
                    ReadonlyDelivered = true;
                    return "Mailed";
                }
                ReadonlyDelivered = true;
                return "Printed";
            }
        }

        public bool IsClosed
        {
            get
            {
                return Invoice.Closed;
            }
            set
            {
                if (value)
                {
                    if (
                        MessageBox.Show("Please confirm that you want to close the invoice", "Close invoice",
                                        MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Invoice.Closed = true;
                    }
                }

                else
                {
                    if (
                        MessageBox.Show("Please confirm that you want to reopen the invoice", "Reopen invoice",
                                        MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Invoice.Closed = false;


                    }
                }
                Execute.InUIThread(() => OnPropertyChanged("IsClosed"));
            }
        }

        public bool Delivered
        {
            get { return Invoice.Delivered; }
            set
            {
                Invoice.Delivered = value;
                OnPropertyChanged("Delivered");
            }
        }

        public double AmountPaid
        {
            get { return Invoice.AmountPaid; }
            set
            {
                Invoice.AmountPaid = value;
                OnPropertyChanged("AmountPaid");
            }
        }

        public bool CanEdit
        {
            get
            {
                if (InvoiceID != "Draft")
                    return true;
                return false;
            }
        }

        public bool CanCheckPaid
        {
            get
            {
                if (InvoiceID != "Draft" && Invoice.Delivered)
                    return true;
                return false;
            }
        }

        public bool IsCreditNote
        {
            get { return Invoice.IsCreditNote; }
        }

        public string InvoiceComment
        {
            get { return Invoice.Comment; }
            set
            {
                Invoice.Comment = value;
                OnPropertyChanged("InvoiceComment");
            }
        }

        public bool ReadonlyDelivered { get; set; }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName != "DeliveredDate")
                _data.SaveInvoiceChanges(Invoice).Subscribe(I =>
                {
                    //if (I.Success == false)
                    //    MessageBox.Show(I.Response);
                    //else
                    //{
                    if (propertyName == "EndDate" || propertyName == "StartDate")
                        _data.RecalculateInvoice((int)Id, StartDate, EndDate, CustomerInvoiceGroupId)
                            .Subscribe(r => Execute.InUIThread(() =>
                            {
                                InternalCommands.ReloadInvoices.Execute(null);
                                InternalCommands.RefreshCustomers.Execute(null);
                            }));
                    //}
                });
        }

        private ImageSource _deliveredField;
        public ImageSource DeliveredField
        {
            get
            {
                return _deliveredField;
            }
            set
            {
                _deliveredField = value;
                OnPropertyChanged("CanCheckPaid");
                OnPropertyChanged("DeliveredDate");
                OnPropertyChanged("DeliveredField");
            }
        }
    }
}
