using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class UserCreateEventArgs : System.EventArgs
    {
        public UserCreateEventArgs(User user, bool success, string responseMessage)
        {
            User = user;
            Success = success;
            ResponseMessage = responseMessage;
        }

        public User User { get; set; }
        public bool Success { get; set; }
        public string ResponseMessage { get; set; }
    }
}