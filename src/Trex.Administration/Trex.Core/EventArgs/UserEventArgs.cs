using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class UserEventArgs : System.EventArgs
    {
        public UserEventArgs(User user)
        {
            User = user;
        }

        public User User { get; set; }
    }
}