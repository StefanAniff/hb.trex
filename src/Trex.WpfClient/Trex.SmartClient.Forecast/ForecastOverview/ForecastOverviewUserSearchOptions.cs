using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.Forecast.ForecastOverview
{
    public class ForecastOverviewUserSearchOptions : ViewModelBase
    {
        private readonly IForecastUserListPresetRepository _userListPresetRepository;
        private ObservableCollection<ForecastUserDto> _users = new ObservableCollection<ForecastUserDto>();
        private ObservableCollection<ForecastUserDto> _selectedUsers = new ObservableCollection<ForecastUserDto>();
        private ObservableCollection<UserListPreset> _userListPresets = new ObservableCollection<UserListPreset>();
        private UserListPreset _selectedUserListPreset;

        #region Commands

        public DelegateCommand<object> SaveNewUserPresetListCommand { get; private set; }
        public DelegateCommand<object> LoadUserPresetListCommand { get; private set; } 
        public DelegateCommand<object> DeletePresetListCommand { get; private set; }
        public DelegateCommand<object> DisableEditUsersPresetCommand { get; private set; }
        public DelegateCommand<object> EnableEditUsersPresetCommand { get; private set; }


        #endregion

        public ForecastOverviewUserSearchOptions(IForecastUserListPresetRepository userListPresetRepository)
        {
            _userListPresetRepository = userListPresetRepository;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SaveNewUserPresetListCommand = new DelegateCommand<object>(_ =>
                {
                    if (SelectedUsers.Count == 0)
                        return;

                    var newPreset = new UserListPreset
                        {
                            Name = "<New bookmark>",
                            Users = new List<ForecastUserDto>(SelectedUsers),
                            IsEditMode = true
                        };

                    UserListPresets.Insert(0, newPreset);

                    SelectedUserListPreset = newPreset;
                    UpdateStorage();
                }, x => UserListPresets.All(y => !y.IsEditMode));

            LoadUserPresetListCommand = new DelegateCommand<object>(x =>
                {
                    if (SelectedUserListPreset == null)
                        return;

                    SelectedUsers.Clear();
                    foreach (var forecastUserDto in SelectedUserListPreset.Users)
                    {
                        SelectedUsers.Add(forecastUserDto);
                    }
                }, x => UserListPresets.All(y => !y.IsEditMode));

            DeletePresetListCommand = new DelegateCommand<object>(x =>
                {
                    var toRemove = x as UserListPreset;
                    if (x == null)
                        return;
                    UserListPresets.Remove(toRemove);
                    UpdateStorage();
                }, x => UserListPresets.All(y => !y.IsEditMode));

            DisableEditUsersPresetCommand = new DelegateCommand<object>(x =>
                {
                    var usersPreset = x as UserListPreset;
                    if (usersPreset == null)
                        return;

                    // Name cant be empty
                    if (string.IsNullOrEmpty(usersPreset.Name))
                        return;

                    usersPreset.IsEditMode = false;

                    // Just edited one. Update local storage
                    if (!usersPreset.IsEditMode)
                       UpdateStorage();
                }, x =>
                    {
                        var usersPreset = x as UserListPreset;
                        return usersPreset != null && usersPreset.IsEditMode;
                    });

            EnableEditUsersPresetCommand = new DelegateCommand<object>(x =>
                {
                    var usersPreset = SelectedUserListPreset;
                    if (usersPreset == null)
                        return;

                    usersPreset.IsEditMode = true;
                }, x =>
                    {
                        var usersPreset = SelectedUserListPreset;
                        return usersPreset != null && !usersPreset.IsEditMode;
                    });
        }

        private void UpdateStorage()
        {
            // Design time its null
            if (_userListPresetRepository == null)
                return;

            _userListPresetRepository.OverWriteAllWith(
                UserListPresets.Select(
                    usr => ForecastUserSearchPreset.Create(usr.Name, new List<int>(usr.Users.Select(y => y.UserId)))));
        }

        public ObservableCollection<ForecastUserDto> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(() => Users);
            }
        }

        public ObservableCollection<ForecastUserDto> SelectedUsers
        {
            get { return _selectedUsers; }
            set
            {
                _selectedUsers = value;
                OnPropertyChanged(() => SelectedUsers);
            }
        }

        public void InitializeUsers(IEnumerable<ForecastUserDto> newUsers)
        {
            Users.Clear();
            foreach (var newUser in newUsers.OrderBy(x => x.Name))
            {
                Users.Add(newUser);
            }
            BuildUserListPresets();
        }

        private void BuildUserListPresets()
        {
            if (_userListPresetRepository == null)
                return;

            var rawPresets = _userListPresetRepository.GetAll();
            UserListPresets = new ObservableCollection<UserListPreset>(rawPresets.Select(x => new UserListPreset
            {
                Name = x.Name,
                Users = new List<ForecastUserDto>(x.UserIds.Select(usrId => Users.SingleOrDefault(usr => usr.UserId.Equals(usrId))))
            }));
        }

        public ObservableCollection<UserListPreset> UserListPresets
        {
            get { return _userListPresets; }
            set
            {
                _userListPresets = value;
                OnPropertyChanged(() => UserListPresets);
            }
        }

        public UserListPreset SelectedUserListPreset
        {
            get { return _selectedUserListPreset; }
            set
            {
                _selectedUserListPreset = value;
                OnPropertyChanged(() => _selectedUserListPreset);
            }
        }

        public void InitializeUserListPresets(IEnumerable<UserListPreset> newPresets)
        {
            UserListPresets.Clear();
            foreach (var userListPreset in newPresets)
            {
                UserListPresets.Add(userListPreset);
            }
        }

        public class UserListPreset : ViewModelBase
        {
            private string _name;
            private bool _isEditMode;

            public List<ForecastUserDto> Users { get; set; }

            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    OnPropertyChanged(() => Name);
                }
            }

            public bool IsEditMode
            {
                get { return _isEditMode; }
                set
                {
                    _isEditMode = value;
                    OnPropertyChanged(() => IsEditMode);
                }
            }

            public string Description
            {
                get
                {
                    if (Users == null)
                        return "None";

                    return Users
                        .OrderBy(x => x.Name)
                        .Aggregate(string.Empty, (current, usr) =>  (!string.IsNullOrEmpty(current)) 
                                                                        ? current + "\n" + usr.Name 
                                                                        : usr.Name);
                }
            }
        }

    }
}