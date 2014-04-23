using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.Administration.Commands;
using Trex.Administration.Resources;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Infrastructure.Implemented;
using Trex.ServiceContracts;
using System;

namespace Trex.Administration.UserAdministrationScreen.MainView
{
    public class UserRowViewModel : ViewModelBase
    {

        private User _user;
        private readonly IDataService _dataService;

        public UserRowViewModel(User user, IDataService dataService)
        {
            EditPricesCommand = new DelegateCommand<object>(ExecuteEditPrices, CanEditPrices);
            DeleteCommand = new DelegateCommand<object>(ExecuteDeleteUser, CanDeleteUser);
            ToggleActivationCommand = new DelegateCommand<object>(ExecuteToggleActivation);
            EditCommand = new DelegateCommand<object>(ExecuteEdit);

            _user = user;
            _dataService = dataService;
        }

        public User User { get { return _user; } }

        public string ActivationButtonText
        {
            get
            {
                if (_user.Inactive)
                {
                    return MainViewResources.ActivateButtonText;
                }
                return MainViewResources.DeactivateButtonText;
            }
        }

        public bool HasChanged { get { return _user.ChangeTracker.State == ObjectState.Modified; } }

        public bool CanEdit
        {
            get
            {
                var canEdit = UserContext.Instance.User.HasPermission(Permissions.EditUserPermission);

                if (!canEdit && _user.UserID == UserContext.Instance.User.UserID)
                {
                    canEdit = UserContext.Instance.User.HasPermission(Permissions.EditSelfPermission);
                }
                return canEdit;
            }
        }

        public string Department { get { return _user.Department; } }

        public string Location { get { return _user.Location; } }

        public string UserName
        {
            get { return _user.UserName; }
            set
            {
                _user.UserName = value;
                OnPropertyChanged("UserName");
            }
        }

        public int TimeEntries
        {
            get
            {
                if (_user.UserTimeEntryStats != null)
                    return _user.UserTimeEntryStats.NumOfTimeEntries.Value;

                return 0;

            }
        }

        public double TotalTime
        {
            get
            {
                if (_user.UserTimeEntryStats != null)
                    return _user.UserTimeEntryStats.TotalTimeSpent.Value;
                return 0d;

            }
        }

        public double TotalBillableTime
        {
            get
            {
                if (_user.UserTimeEntryStats != null)
                    return _user.UserTimeEntryStats.TotalBillable.Value;
                return 0d;


            }
        }

        public string Name
        {
            get { return _user.Name; }
            set
            {
                _user.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public double Price
        {
            get { return _user.Price; }
            set
            {
                _user.Price = value;
                OnPropertyChanged("Price");
            }
        }

        public bool Inactive
        {
            get { return _user.Inactive; }
            set
            {
                _user.Inactive = value;
                OnPropertyChanged("Inactive");
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

        public string Roles
        {
            get
            {
                var rolesString = new StringBuilder();
                foreach (var role in _user.Roles)
                {
                    rolesString.Append(role);
                    rolesString.Append(",");
                }
                if (rolesString.Length > 0)
                {
                    rolesString.Length = rolesString.Length - 1;
                }

                return rolesString.ToString();
            }
        }

        public DelegateCommand<object> EditPricesCommand { get; set; }
        public DelegateCommand<object> DeleteCommand { get; set; }
        public DelegateCommand<object> EditCommand { get; set; }
        public DelegateCommand<object> ToggleActivationCommand { get; set; }

        private void ExecuteEdit(object obj)
        {
            InternalCommands.EditUserStart.Execute(_user);
        }

        private void ExecuteToggleActivation(object obj)
        {
            if (_user.Inactive)
            {
                InternalCommands.ActivateUser.Execute(_user);
            }

            else
            {
                var messageBox = MessageBox.Show(MainViewResources.DeactivateUser, "Deactivate user", MessageBoxButton.OKCancel);
                if (messageBox == MessageBoxResult.OK)
                {
                    InternalCommands.DeActivateUser.Execute(_user);
                }
            }
        }

        private void ExecuteDeleteUser(object obj)
        {
            var messageBox = MessageBox.Show(MainViewResources.DeleteUserConfirmText, "Delete user", MessageBoxButton.OKCancel);
            if (messageBox == MessageBoxResult.OK)
            {
                InternalCommands.DeleteUser.Execute(_user);
            }
        }

        private bool CanDeleteUser(object arg)
        {
            //TODO: Not implemented
            return false;
            //return _user.NumOfTimeEntries == 0;
        }

        private bool CanEditPrices(object arg)
        {
            //Todo Check rights
            return true;
        }

        private void ExecuteEditPrices(object obj)
        {
            InternalCommands.EditUserPricesStart.Execute(_user);
        }

        public void SubmitChanges()
        {

            _dataService.SaveUser(User).Subscribe(
                result => _user.AcceptChanges()
                );


        }


        public void CancelChanges()
        {
            _user.CancelChanges();

            Update();
        }
    }
}