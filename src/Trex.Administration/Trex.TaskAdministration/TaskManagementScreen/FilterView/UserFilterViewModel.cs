using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.TaskManagementScreen.FilterView
{
    public class UserFilterViewModel : ViewModelBase
    {
        public UserFilterViewModel(User user)
        {
            User = user;
            UserRemoveClick = new DelegateCommand<object>(RemoveUser);
        }

        public string Name
        {
            get { return User.Name; }
        }

        public DelegateCommand<object> UserRemoveClick { get; set; }

        public User User { get; set; }

        public void RemoveUser(object user)
        {
            InternalCommands.UserDeselected.Execute(User);
        }
    }
}