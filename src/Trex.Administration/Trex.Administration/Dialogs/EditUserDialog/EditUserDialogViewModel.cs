using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.Administration.Commands;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.ServiceContracts;
using System;

namespace Trex.Administration.Dialogs.EditUserDialog
{
    public class EditUserDialogViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IUserRepository _userRepository;

        private readonly User _user;
        private readonly User _tempUser;
        
        private string _responseMessage;
        private ObservableCollection<RoleViewModel> _roles;

        private bool _isBusy;

        public EditUserDialogViewModel(User user, IDataService dataService, IUserRepository userRepository)
        {
            _roles = new ObservableCollection<RoleViewModel>();

            
            _user = user;
            _tempUser = user.DeepCopy();

            _dataService = dataService;
            _userRepository = userRepository;
            SaveCommand = new DelegateCommand<object>(ExcecuteSave, CanExecuteSave);
            CancelCommand = new DelegateCommand<object>(ExecuteCancel, obj => !IsBusy);
            ResetPasswordCommand = new DelegateCommand<object>(ExecuteResetPassword, obj => !IsBusy);
            SendEmail = true;
            LoadRoles();
            LoadDepartments();
            LoadLocations();

        }

        private void LoadLocations()
        {
            Locations = new ObservableCollection<string>();
            var locations = _userRepository.Users.Select(u => u.Location).Distinct();
            foreach (var location in locations)
            {
                Locations.Add(location);
            }
        }

        private void LoadDepartments()
        {
            Departments = new ObservableCollection<string>();
            var departments = _userRepository.Users.Select(u => u.Department).Distinct();
            foreach (var department in departments)
            {
                Departments.Add(department);
            }
        }

        public bool IsNewUser
        {
            get { return _user.UserID == 0; }
        }

        public bool IsExistingUser
        {
            get { return _user.UserID > 0; }
        }

        public bool CanEditUserName
        {
            get { return _user.UserID == 0; }
        }

        public string PasswordHeaderText
        {
            get
            {
                if (_user.UserID == 0)
                {
                    return "Password";
                }

                return "Change Password";
            }
        }

        public bool ChangePassword
        {
            get { return _user.UserID == 0; }
        }

        public string UserName
        {
            get { return _user.UserName; }
            set
            {
                _user.UserName = value;
                OnPropertyChanged(UserName);
            }
        }

        public string FullName
        {
            get { return _user.Name; }
            set
            {
                _user.Name = value;
                OnPropertyChanged("FullName");
            }
        }

        public string Email
        {
            get { return _user.Email; }
            set
            {
                _user.Email = value;
                OnPropertyChanged("Email");
            }
        }

        public double DefaultPrice
        {
            get { return _user.Price; }
            set
            {
                _user.Price = value;
                OnPropertyChanged("Price");
            }
        }

        public string ResponseMessage
        {
            get { return _responseMessage; }
            set
            {
                _responseMessage = value;
                OnPropertyChanged("ResponseMessage");
            }
        }

        public bool IsActive
        {
            get { return !_user.Inactive; }
            set
            {
                _user.Inactive = !value;
                OnPropertyChanged("IsActive");
            }
        }

        public string Department
        {
            get { return _user.Department; }
            set
            {
                _user.Department = value;
                OnPropertyChanged("Department");
            }
        }

        public string Location
        {
            get { return _user.Location; }
            set
            {
                _user.Location = value;
                OnPropertyChanged("Location");
            }
        }


        private ObservableCollection<string> _departments;
        public ObservableCollection<string> Departments
        {
            get { return _departments; }
            set
            {
                _departments = value;
                OnPropertyChanged("Departments");
            }
        }

        private ObservableCollection<string> _locations;
        public ObservableCollection<string> Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                OnPropertyChanged("Locations");
            }
        }

        public ObservableCollection<RoleViewModel> Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
                OnPropertyChanged("Roles");
            }
        }

        private bool _sendEmail;
        public bool SendEmail
        {
            get { return _sendEmail; }
            set
            {
                _sendEmail = value;
                OnPropertyChanged("SendEmail");
            }
        }


        public DelegateCommand<object> SaveCommand { get; set; }
        public DelegateCommand<object> CancelCommand { get; set; }
        public DelegateCommand<object> ResetPasswordCommand { get; set; }

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
                SaveCommand.RaiseCanExecuteChanged();
                CancelCommand.RaiseCanExecuteChanged();
                ResetPasswordCommand.RaiseCanExecuteChanged();
            }
        }

        private void ExecuteResetPassword(object obj)
        {
            var language = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            IsBusy = true;
            _dataService.ResetPassword(_user,language).Subscribe(
                success =>
                {

                    if (success)
                    {
                        MessageBox.Show(
                            "Password successfully reset, and an email has been sent to the user with a new password");

                    }
                    else
                    {
                        MessageBox.Show("Password reset failed.");
                        
                    }


                }
                , () => IsBusy = false);
        }

        private void LoadRoles()
        {
            _dataService.GetRoles().Subscribe(
                roles =>
                {

                    foreach (var role in roles)
                    {
                        var roleViewModel = new RoleViewModel(role);
                        if (_user.Roles.SingleOrDefault(r => r.Equals(role.Title)) != null)
                        {
                            roleViewModel.IsSelected = true;
                        }
                        Roles.Add(roleViewModel);
                    }

                }
                );
        }

        private void ExecuteCancel(object obj)
        {
            _user.UserName = _tempUser.UserName;
            _user.Name = _tempUser.Name;
            _user.Email = _tempUser.Email;
            _user.Price = _tempUser.Price;

            _user.Roles = _tempUser.Roles;
            _user.Department = _tempUser.Department;
            _user.Location = _tempUser.Location;
            InternalCommands.CreateNewUserCompleted.Execute(null);
        }

        private bool CanExecuteSave(object arg)
        {
            if (IsBusy)
                return false;

            var canSave = !string.IsNullOrEmpty(_user.UserName) && !string.IsNullOrEmpty(_user.Name) && !string.IsNullOrEmpty(_user.Email);           
            return canSave;
        }

        private void ExcecuteSave(object obj)
        {

            ApplicationCommands.SystemBusy.Execute(null);
            _user.Roles = Roles.Where(r => r.IsSelected).Select(r => r.RoleName).ToList();
 
            IsBusy = true;
            if (_user.UserID == 0)
            {
                var language = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;                
                _dataService.CreateUser(_user, SendEmail,language).Subscribe(response =>
                                                                                {
                                                                                    if (!response.Success)
                                                                                    {
                                                                                       MessageBox.Show(response.Response);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        _user.UserID =response.User.UserID;
                                                                                        _user.AcceptChanges();
                                                                                        _userRepository.Users.Add(_user);
                                                                                        InternalCommands.CreateNewUserCompleted.Execute(1);
                                                                                    }
                                                                                    ApplicationCommands.SystemIdle.Execute(null);
                                                                                }, exception =>
                                                                                    {
                                                                                        ApplicationCommands.SystemIdle.Execute(null);
                                                                                        MessageBox.Show(string.Format("Create user failed with:\n{0}", exception.ToString()));
                                                                                    }, () => { IsBusy = false; });
            }
            else
            {
                IsBusy = true;
                _dataService.SaveUser(_user).Subscribe(
                    result =>
                    {
                        _user.AcceptChanges();
                        InternalCommands.CreateNewUserCompleted.Execute(1);
                        ApplicationCommands.SystemIdle.Execute(null);
                    }, () => { IsBusy = false; });
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}