using Trex.Core.Implemented;
using Trex.ServiceContracts;

namespace Trex.Roles.RoleScreen.MainView
{
    public class PermissionViewModel : ViewModelBase
    {
        
        private bool _isEnabled;

        public PermissionViewModel(UserPermission userPermission, bool isEnabled)
        {
            UserPermission = userPermission;
            Permission = userPermission.Permission;
            IsEnabled = isEnabled;
        }

        public UserPermission UserPermission { get; set; }

        private string _permission;
        public string Permission
        {
            get { return _permission; }
            set
            {
                _permission = value;
                OnPropertyChanged("Permission");
            }
        }


        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
    }
}
