using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.Administration.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Infrastructure.Implemented;
using Trex.ServiceContracts;

namespace Trex.Administration.UserAdministrationScreen.MainView
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IExceptionHandlerService _exceptionHandlerService;
        private readonly IUserRepository _userRepository;
        private ObservableCollection<UserRowViewModel> _users;

        public MainViewModel(IDataService dataService, IExceptionHandlerService exceptionHandlerService, IUserRepository userRepository)
        {
            CreateCommand = new DelegateCommand<object>(ExecuteCreateNewUser);
            EditCommand = new DelegateCommand<object>(ExecuteEditUser, CanEditUser);
            InviteCommand = new DelegateCommand<object>(ExecuteInviteUsers);
            SaveCommand = new DelegateCommand<object>(ExecuteSaveCommand, CanExecuteSave);
            CancelCommand = new DelegateCommand<object>(ExecuteCancel, CanExecuteSave);
            EditPricesCommand = new DelegateCommand<object>(ExecuteEditUserPrices, CanEditUserPrices);
            DeleteUserCommand = new DelegateCommand<object>(ExecuteDeleteUser, CanDeleteUser);
            InternalCommands.InviteUsersCompleted.RegisterCommand(new DelegateCommand<object>(InviteUsersCompleted));
            InternalCommands.CreateNewUserCompleted.RegisterCommand(new DelegateCommand<int?>(CreateNewUserCompleted));
            InternalCommands.DeleteUser.RegisterCommand(new DelegateCommand<User>(ExecuteDeleteUser));
            InternalCommands.ReloadUsers.RegisterCommand(new DelegateCommand(BindUsers));

            _dataService = dataService;
            _exceptionHandlerService = exceptionHandlerService;
            _userRepository = userRepository;

            BindUsers();
        }

        private bool CanDeleteUser(object arg)
        {
            if (_selectedUser != null)
                if (_selectedUser.User.UserTimeEntryStats != null)
                {
                    return _selectedUser.User.UserTimeEntryStats.NumOfTimeEntries == 0;
                }
                else
                {
                    return true;
                }

            return false;

        }

        public DelegateCommand<object> DeleteUserCommand { get; set; }

        private bool CanEditUserPrices(object arg)
        {
            if (SelectedUser == null)
                return false;

            var canEdit = SelectedUser != null;


            if (canEdit)
                canEdit = UserContext.Instance.User.HasPermission(Permissions.EditUserPricesPermission);

            if (!canEdit && SelectedUser.User.UserID == UserContext.Instance.User.UserID)
            {
                canEdit = UserContext.Instance.User.HasPermission(Permissions.EditSelfPermission);
            }
            return canEdit;
        }

        private void ExecuteEditUserPrices(object obj)
        {
            InternalCommands.EditUserPricesStart.Execute(SelectedUser.User);
        }

        private bool CanEditUser(object arg)
        {
            if (SelectedUser == null)
                return false;

            var canEdit = SelectedUser != null;


            if (canEdit)
                canEdit = UserContext.Instance.User.HasPermission(Permissions.EditUserPermission);

            if (!canEdit && SelectedUser.User.UserID == UserContext.Instance.User.UserID)
            {
                canEdit = UserContext.Instance.User.HasPermission(Permissions.EditSelfPermission);
            }
            return canEdit;
        }

        private void ExecuteEditUser(object obj)
        {
            InternalCommands.EditUserStart.Execute(SelectedUser.User);
        }


        private UserRowViewModel _selectedUser;
        public UserRowViewModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        public DelegateCommand<object> EditCommand { get; set; }

        public DelegateCommand<object> EditPricesCommand { get; set; }

        public ObservableCollection<UserRowViewModel> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged("Users");
            }
        }

        public DelegateCommand<object> InviteCommand { get; set; }
        public DelegateCommand<object> CreateCommand { get; set; }
        public DelegateCommand<object> SaveCommand { get; set; }
        public DelegateCommand<object> CancelCommand { get; set; }

        private void InviteUsersCompleted(object obj)
        {
            //TODO
        }

        private void ExecuteInviteUsers(object obj)
        {
            InternalCommands.InviteUsersStart.Execute(null);
        }

        private void ExecuteDeleteUser(object obj)
        {
            if (MessageBox.Show("Are you sure you want to delete " + _selectedUser.Name + "?", "Delete user", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;
            _selectedUser.User.MarkAsDeleted();
            _dataService.DeleteUser(_selectedUser.User).Subscribe(
                result =>
                {
                    _selectedUser.User.AcceptChanges();
                    _userRepository.Users.Remove(_selectedUser.User);

                    BindUsers();

                }
                    , _exceptionHandlerService.OnError
                );
        }

        private bool _showInactive;
        public bool ShowInactive
        {
            get { return _showInactive; }
            set
            {
                _showInactive = value;

                OnPropertyChanged("ShowInactive");
                BindUsers();
            }
        }

        private void BindUsers()
        {
            Users = new ObservableCollection<UserRowViewModel>();

            foreach (var user in _userRepository.Users.OrderBy(u => u.Name))
            {
                var addUser = _showInactive || (!_showInactive && !user.Inactive);
                if (addUser)
                    Users.Add(new UserRowViewModel(user, _dataService));
            }

            SelectedUser = null;


        }

        private void CreateNewUserCompleted(int? obj)
        {
            if(obj == null) return;
            BindUsers();
        }

        private void ExecuteCancel(object obj)
        {
            foreach (var userRowViewModel in Users)
            {
                if (userRowViewModel.HasChanged)
                {
                    userRowViewModel.CancelChanges();
                }
            }
            SaveCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }

        private bool CanExecuteSave(object arg)
        {
            if (Users != null)
            {
                return Users.FirstOrDefault(u => u.HasChanged) != null;
            }
            return false;
        }

        private void ExecuteSaveCommand(object obj)
        {
            foreach (var userRowViewModel in Users)
            {
                if (userRowViewModel.HasChanged)
                {
                    _dataService.SaveUser(userRowViewModel.User).Subscribe(
                        result =>
                        userRowViewModel.SubmitChanges()
                        );
                }
            }
            SaveCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }

        private void ExecuteCreateNewUser(object obj)
        {
            InternalCommands.CreateNewUserStart.Execute(null);
        }


        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            EditCommand.RaiseCanExecuteChanged();
            EditPricesCommand.RaiseCanExecuteChanged();
            DeleteUserCommand.RaiseCanExecuteChanged();
        }
    }
}