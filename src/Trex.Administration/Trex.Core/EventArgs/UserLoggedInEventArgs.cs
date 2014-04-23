using Trex.Core.Services;

namespace Trex.Core.EventArgs
{
    public class UserLoggedInEventArgs : System.EventArgs
    {
        public UserLoggedInEventArgs(ILoginSettings loginSettings, bool loginSucceeded)
        {
            LoginSettings = loginSettings;
            LoginSucceeded = loginSucceeded;
        }

        public ILoginSettings LoginSettings { get; set; }

        public bool LoginSucceeded { get; set; }
    }
}