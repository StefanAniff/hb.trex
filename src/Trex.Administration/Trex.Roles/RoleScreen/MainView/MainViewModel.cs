#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.Infrastructure.Implemented;
using Trex.Roles.Commands;
using Trex.ServiceContracts;

#endregion

namespace Trex.Roles.RoleScreen.MainView
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IExceptionHandlerService _exceptionHandlerService;
        private readonly IUserSession _userSession;
        private int _clientApplicationId;
        private bool _isListEnabled;

        private ObservableCollection<PermissionViewModel> _permissionsList =
            new ObservableCollection<PermissionViewModel>();

        private ObservableCollection<RoleItem> _rolesList = new ObservableCollection<RoleItem>();

        private RoleItem _selectedRole;
        private int _selectedTab;

        public MainViewModel(IUserSession userSession, IDataService dataService,
                             IExceptionHandlerService exceptionHandlerService)
        {
            _dataService = dataService;
            _exceptionHandlerService = exceptionHandlerService;
            _userSession = userSession;

            InternalCommands.CreateNewRoleCompleted.RegisterCommand(new DelegateCommand<object>(BindRoles));
            InternalCommands.DeleteRoleCompleted.RegisterCommand(new DelegateCommand<object>(BindRoles));

            CreateNewRoleCommand = new DelegateCommand<object>(ExecuteCreateNewRole);
            DeleteRoleCommand = new DelegateCommand<object>(ExecuteDeleteRole, CanDeleteRole);
            SaveChangesCommand = new DelegateCommand<object>(ExecuteSaveChanges, CanSave);
            RefreshRolesCommand = new DelegateCommand<object>(ExecuteRefreshRoles);

            BindRoles(null);

            _clientApplicationId = 1;
        }

        public DelegateCommand<object> CreateNewRoleCommand { get; set; }
        public DelegateCommand<object> DeleteRoleCommand { get; set; }
        public DelegateCommand<object> SaveChangesCommand { get; set; }
        public DelegateCommand<object> RefreshRolesCommand { get; set; }

        public ObservableCollection<RoleItem> RolesList
        {
            get { return _rolesList; }
            set { _rolesList = value; }
        }

        public ObservableCollection<PermissionViewModel> PermissionsList
        {
            get { return _permissionsList; }
            set { _permissionsList = value; }
        }

        public RoleItem SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                OnPropertyChanged("SelectedRole");
                SaveChangesCommand.RaiseCanExecuteChanged();
                DeleteRoleCommand.RaiseCanExecuteChanged();
                if (_selectedRole != null)
                    BindPermissions(_selectedRole.RoleName);
            }
        }

        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                _clientApplicationId = value + 1;
                if (SelectedRole != null)
                {
                    BindPermissions(SelectedRole.RoleName);
                }
                OnPropertyChanged("SelectedTab");
            }
        }

        public bool IsListEnabled
        {
            get { return _isListEnabled; }
            set
            {
                _isListEnabled = value;
                OnPropertyChanged("IsListEnabled");
            }
        }

        private bool CanSave(object arg)
        {
            return _selectedRole != null;
        }

        private bool CanDeleteRole(object arg)
        {
            return _selectedRole != null;
        }

        private void ExecuteDeleteRole(object obj)
        {
            InternalCommands.DeleteRoleStart.Execute(SelectedRole.Role);
        }

        private void ExecuteCreateNewRole(object obj)
        {
            InternalCommands.CreateNewRoleStart.Execute(null);
        }

        private void BindRoles(object obj)
        {
            _rolesList.Clear();

            _dataService.GetRoles().Subscribe(roles =>
                                                  {
                                                      _rolesList.Clear();

                                                      foreach (var role in roles)
                                                      {
                                                          _rolesList.Add(new RoleItem(role));
                                                      }
                                                  }, _exceptionHandlerService.OnError);
        }

        private void ExecuteSaveChanges(object obj)
        {
            var permissionsSelected = PermissionsList.Where(p => p.IsEnabled).Select(p => p.UserPermission);

            _dataService.UpdatePermissions(new ObservableCollection<UserPermission>(permissionsSelected),
                                           _selectedRole.Role, _clientApplicationId).Subscribe(
                                               result => MessageBox.Show("Changes successfully saved"));
        }

        private void BindPermissions(string roleName)
        {
            if (roleName != null)
            {
                IsListEnabled = _userSession.CurrentUser.HasPermission(Permissions.ChangeRolePermission);
                var permissionsInRole = new List<UserPermission>();

                _dataService.GetPermissionsForRole(roleName, _clientApplicationId).Subscribe(
                    permissionsInRole.AddRange,
                    () => BindPermissions(permissionsInRole)
                    );
            }
        }

        private void BindPermissions(List<UserPermission> permissionsInRole)
        {
            PermissionsList.Clear();

            _dataService.GetAllPermissionsByClientId(_clientApplicationId).Subscribe(
                permissions =>
                    {
                        foreach (var userPermission in permissions)
                        {
                            var isEnabled =
                                permissionsInRole.SingleOrDefault(p => p.PermissionID == userPermission.PermissionID) !=
                                null;

                            PermissionsList.Add(new PermissionViewModel(userPermission, isEnabled));
                        }
                    }
                );
        }

        private void ExecuteRefreshRoles(object obj)
        {
            _permissionsList.Clear();
            BindRoles(null);
        }
    }
}