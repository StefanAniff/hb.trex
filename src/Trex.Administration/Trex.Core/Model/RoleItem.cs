using Trex.Core.Implemented;
using Trex.ServiceContracts;

namespace Trex.Core.Model
{
    public class RoleItem : ViewModelBase
    {
        public RoleItem(Role role)
        {
            Role = role;
           
        }

        public Role Role { get; set; }

        public string RoleName
        {
            get { return Role.Title; }
           
        }
    }
}