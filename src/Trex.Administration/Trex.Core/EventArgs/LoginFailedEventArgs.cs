namespace Trex.Core.EventArgs
{
    public class LoginFailedEventArgs : System.EventArgs
    {
        public LoginFailedEventArgs(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; set; }
    }
}