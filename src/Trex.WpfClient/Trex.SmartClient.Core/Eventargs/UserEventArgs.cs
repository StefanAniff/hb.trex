using Trex.SmartClient.Core.Model;

namespace Trex.SmartClient.Core.Eventargs
{
    public class UserEventArgs:System.EventArgs
    {
        public UserEventArgs(User user)
        {
            User = user;
        }

        public User User { get; set; }
    }
}