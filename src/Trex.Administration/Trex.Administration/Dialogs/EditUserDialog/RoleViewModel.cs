using Trex.Core.Implemented;
using Trex.ServiceContracts;

namespace Trex.Administration.Dialogs.EditUserDialog
{
    public class RoleViewModel : ViewModelBase
    {
        public RoleViewModel(Role role)
        {
            Role = role;

        }

        public Role Role { get; set; }

        public string RoleName
        {
            get { return Role.Title; }

        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
    }
}