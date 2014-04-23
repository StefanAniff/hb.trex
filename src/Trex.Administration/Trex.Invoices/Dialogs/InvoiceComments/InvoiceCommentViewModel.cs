using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Controls.GridView;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Invoices.Commands;
using Trex.Invoices.InvoiceManagementScreen.InvoiceView;
using Trex.ServiceContracts;
using System.Linq;

namespace Trex.Invoices.Dialogs.InvoiceComments
{
    public class InvoiceCommentViewModel : ViewModelBase
    {
        private ObservableCollection<UserComments> _commentSource;

        public struct UserComments
        {
            public string UserName { get; set; }
            public string Comment { get; set; }
            public string Time { get; set; }
        }

        public DelegateCommand<string> SendClick { get; set; }
        public DelegateCommand<int?> CloseClick { get; set; }
        private IDataService _dataService;
        private IUserRepository _userRepository;
        private InvoiceListItemViewModel _invoice;
        private IUserSession _userSession;
        private string _invMsg;
        public string InvoiceCommentTitle { get; set; }

        public string InvMsg
        {
            get { return _invMsg; }
            set
            {
                _invMsg = value;
                OnPropertyChanged("InvMsg");
            }
        }

        public ObservableCollection<UserComments> CommentSource
        {
            get { return _commentSource; }
            set
            {
                _commentSource = value;
                OnPropertyChanged("CommentSource");
            }
        }

        public InvoiceCommentViewModel(InvoiceListItemViewModel invoice, IDataService dataService, IUserRepository userRepository, IUserSession userSession)
        {
            _dataService = dataService;
            _userRepository = userRepository;
            _invoice = invoice;
            _userSession = userSession;
            InternalCommands.SendComment.RegisterCommand(new DelegateCommand<string>(ExecuteSendClick));
            SendClick = new DelegateCommand<string>(ExecuteSendClick);
            CloseClick = new DelegateCommand<int?>(ExecuteCloseClick);
            CommentSource = new ObservableCollection<UserComments>();

            InvoiceCommentTitle = "Invoice comments for: " + invoice.InvoiceID;
            LoadComments();

        }

        private void ExecuteCloseClick(int? obj)
        {
            InternalCommands.ReloadInvoiceCommentField.Execute(_invoice.Id);
        }

        private void LoadComments()
        {
            CommentSource.Clear();
            _dataService.LoadInvoiceComments((int)_invoice.Id).Subscribe(c =>
            {
                foreach (var invoiceComment in c)
                {
                    var user = _userRepository.GetById(invoiceComment.UserID);
                    CommentSource.Add(new UserComments { Comment = invoiceComment.Comment, UserName = user.Name + ": ", Time = invoiceComment.TimeStamp.ToShortDateString() });
                }
            });

        }

        private void LoadLastComments()
        {
            _dataService.LoadInvoiceComments((int)_invoice.Id).Subscribe(c =>
            {
                var user = _userRepository.GetById(c.Last().UserID);
                CommentSource.Add(new UserComments { Comment = c.Last().Comment, UserName = user.Name + ": ", Time = c.Last().TimeStamp.ToShortDateString() });
            });

        }

        private void ExecuteSendClick(string obj)
        {
            string co = obj ?? InvMsg;
            _dataService.SaveInvoiceComment(co, (int)_invoice.Id, _userSession.CurrentUser.UserID).Subscribe(c =>
                                                                                                                     {
                                                                                                                         LoadLastComments();
                                                                                                                         InternalCommands.ReloadInvoiceCommentField.Execute(_invoice.Id);
                                                                                                                     });

        }
    }
}
