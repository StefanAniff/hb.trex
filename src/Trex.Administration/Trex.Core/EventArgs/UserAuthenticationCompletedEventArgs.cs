namespace Trex.Core.EventArgs
{
    public class UserAuthenticationCompletedEventArgs : System.EventArgs
    {
        public UserAuthenticationCompletedEventArgs(bool result)
        {
            Result = result;
        }

        public bool Result { get; set; }
    }
}