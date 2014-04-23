using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen.FilterView
{
    public class FilterViewModel : ViewModelBase, IFilterViewModel
    {
        private readonly IDataService _dataService;
        private Visibility _applyFilterButtonVisibility;
        private bool _billableOnly;
        private DateTime? _fromDate;
        private bool _invoiced;
        private bool _notInvoiced;
        private Visibility _resetFilterButtonVisibility;
        private User _selectedUser;
        private TimeEntryFilter _timeEntryFilter;
        private DateTime? _toDate;
        private AutoCompleteFilterPredicate<object> _userFilter;
        private bool _hideEmptyTasks;
        private bool _hideEmptyProjects;

        public FilterViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Users = new List<User>();
            SelectedUsers = new ObservableCollection<UserFilterViewModel>();
            ApplyFilterCommand = new DelegateCommand<object>(ApplyFilter);
            ResetFilterCommand = new DelegateCommand<object>(ResetFilter);



            _timeEntryFilter = new TimeEntryFilter();
            LoadUsers();
            InitUserFilter();

            Invoiced = true;
            NotInvoiced = true;

            InternalCommands.UserSelected.RegisterCommand(new DelegateCommand<object>(UserSelected));
            InternalCommands.UserDeselected.RegisterCommand(new DelegateCommand<User>(RemoveUser));
        }

        private void LoadUsers()
        {
            _dataService.GetAllUsers().Subscribe(
                users =>
                {
                    Users = users.ToList();
                    OnPropertyChanged("Users");
                    OnPropertyChanged("HasConsultants");
                }

                );
        }

        public AutoCompleteFilterPredicate<object> UserFilter
        {
            get { return _userFilter; }
            set
            {
                _userFilter = value;
                OnPropertyChanged("UserFilter");
            }
        }

        public List<User> Users { get; set; }

      

        public bool BillableOnly
        {
            get { return _billableOnly; }
            set
            {
                _billableOnly = value;
                if (_timeEntryFilter != null)
                {
                    _timeEntryFilter.BillableOnly = value;
                }
                OnPropertyChanged("BillableOnly");
            }
        }

        public bool Invoiced
        {
            get { return _invoiced; }
            set
            {
                _invoiced = value;
                if (_timeEntryFilter != null)
                {
                    _timeEntryFilter.ShowInvoiced = value;
                }
                OnPropertyChanged("Invoiced");
            }
        }

        public bool NotInvoiced
        {
            get { return _notInvoiced; }
            set
            {
                _notInvoiced = value;
                if (_timeEntryFilter != null)
                {
                    _timeEntryFilter.ShowNotInvoiced = value;
                }
                OnPropertyChanged("NotInvoiced");
            }
        }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;

                    if (_selectedUser == null)
                    {
                        return;
                    }
                }
            }
        }

        public ObservableCollection<UserFilterViewModel> SelectedUsers { get; set; }

        public Visibility ApplyFilterButtonVisibility
        {
            get { return _applyFilterButtonVisibility; }
            set
            {
                _applyFilterButtonVisibility = value;
                OnPropertyChanged("ApplyFilterButtonVisibility");
            }
        }

        public Visibility ResetFilterButtonVisibility
        {
            get { return _resetFilterButtonVisibility; }
            set
            {
                _resetFilterButtonVisibility = value;
                OnPropertyChanged("ResetFilterButtonVisibility");
            }
        }

        public bool HasConsultants
        {
            get { return SelectedUsers.Count > 0; }
        }

        public DelegateCommand<object> ApplyFilterCommand { get; set; }

        public DelegateCommand<object> ResetFilterCommand { get; set; }

        private void ApplyFilter(object obj)
        {
            InternalCommands.UserFilterChanged.Execute(_timeEntryFilter);
        }

        private void ResetFilter(object obj)
        {
            _timeEntryFilter = new TimeEntryFilter();            
            BillableOnly = false;
            Invoiced = true;
            NotInvoiced = true;
            HideEmptyProjects = false;
            HideEmptyTasks = false;
            SelectedUsers.Clear();
            OnPropertyChanged("SelectedUsers");
            OnPropertyChanged("HasConsultants");
            InternalCommands.UserFilterChanged.Execute(null);
        }

        private void InitUserFilter()
        {
            UserFilter = (searchString, item) =>
                             {
                                 var user = item as User;
                                 var filter = searchString.ToLower();
                                 if (user != null)
                                 {
                                     return user.Name.ToLower().Contains(filter);
                                 }

                                 return false;
                             };
        }



        public void UserSelected(object obj)
        {
            if (SelectedUser == null)
            {
                return;
            }

            if (SelectedUsers.Count(u => u.User.UserID == SelectedUser.UserID) == 0)
            {
                if (_timeEntryFilter != null)
                {
                    _timeEntryFilter.AddUser(SelectedUser);
                }
                SelectedUsers.Add(new UserFilterViewModel(SelectedUser));
                OnPropertyChanged("SelectedUsers");
                OnPropertyChanged("HasConsultants");
            }
        }

        public void RemoveUser(User user)
        {
            var userToRemove = SelectedUsers.SingleOrDefault(u => u.User.UserID == user.UserID);
            if (userToRemove != null)
            {
                if (_timeEntryFilter != null)
                {
                    _timeEntryFilter.RemoveUser(user);
                }
                SelectedUsers.Remove(userToRemove);
                OnPropertyChanged("SelectedUsers");
                OnPropertyChanged("HasConsultants");
            }
        }

        public bool HideEmptyTasks
        {
            get { return _hideEmptyTasks; }
            set
            {
                _hideEmptyTasks = value;
                if (_timeEntryFilter != null)
                {
                    _timeEntryFilter.HideEmptyTasks = _hideEmptyTasks;
                }
                OnPropertyChanged("HideEmptyTasks");
            }
        }

        public bool HideEmptyProjects
        {
            get { return _hideEmptyProjects; }
            set
            {
                _hideEmptyProjects = value;
                if (_timeEntryFilter != null)
                    _timeEntryFilter.HideEmptyProjects = _hideEmptyProjects;
                OnPropertyChanged("HideEmptyProjects");
            }
        }
    }
}