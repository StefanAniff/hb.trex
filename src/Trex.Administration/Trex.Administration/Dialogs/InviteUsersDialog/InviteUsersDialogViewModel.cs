using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Trex.Administration.Commands;
using Trex.Core.Implemented;
using Trex.Core.Model;
using Trex.Core.Services;
using System;


namespace Trex.Administration.Dialogs.InviteUsersDialog
{
    public class InviteUsersDialogViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IUserSession _userSession;
        
        private string _emails;
        private bool _isEmailsBoxEnabled;
        private ObservableCollection<RoleItem> _rolesList = new ObservableCollection<RoleItem>();
        private RoleItem _selectedRole;

        public InviteUsersDialogViewModel( IDataService dataService, IUserSession userSession)
        {
            _dataService = dataService;
            
            _userSession = userSession;

            ListTheRoles();

            InviteCommand = new DelegateCommand<object>(ExcecuteInvite, CanExecuteInvite);
            CloseCommand = new DelegateCommand<object>(ExecuteClose);

            IsEmailsBoxEnabled = false;
        }

        public DelegateCommand<object> InviteCommand { get; set; }
        public DelegateCommand<object> CloseCommand { get; set; }

        public ObservableCollection<RoleItem> RolesList
        {
            get { return _rolesList; }
            set { _rolesList = value; }
        }

        public RoleItem SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;

                if (value != null)
                {
                    IsEmailsBoxEnabled = true;
                }
                else
                {
                    IsEmailsBoxEnabled = false;
                }

                OnPropertyChanged("SelectedRole");
            }
        }

        public bool IsEmailsBoxEnabled
        {
            get { return _isEmailsBoxEnabled; }
            set
            {
                _isEmailsBoxEnabled = value;
                OnPropertyChanged("IsEmailsBoxEnabled");
            }
        }

        public string Emails
        {
            get { return _emails; }
            set
            {
                _emails = value;
                OnPropertyChanged("Emails");
            }
        }

        private void ExecuteClose(object obj)
        {
            InternalCommands.InviteUsersCompleted.Execute(null);
        }

        private bool CanExecuteInvite(object arg)
        {
            //throw new NotImplementedException();
            return true;
        }

        private void ExcecuteInvite(object obj)
        {
            //TODO:Fix this
            //_dataService.SendInvitationEmail(_userSession.CustomerId, SelectedRole.Role, Emails).Subscribe(
            //    result => ExecuteClose(null));
            

        }

        private void ListTheRoles()
        {
            _rolesList.Clear();
           

            _dataService.GetRoles().Subscribe(

                 roles =>
                 {
                     _rolesList.Clear();
                     foreach (var role in roles)
                     {
                         _rolesList.Add(new RoleItem(role));
                     }
                 }


                );

        }


    }
}