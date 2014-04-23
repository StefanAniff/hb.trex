using System;

namespace Trex.Core.Services
{
    public interface IAuthenticationService
    {
       // event EventHandler<UserAuthenticationCompletedEventArgs> UserAuthenticationCompleted;
        // event EventHandler<UserLoggedInEventArgs> UserLoginCompleted;
        IObservable<bool> AuthenticateUser(string userName, string password, string customerId);
        IObservable<bool> LoginAsHttpUser(string userName, string password, string customerId);
        //void LoginUser(string userName, string password, bool persistLogin);
        //bool HasPersistedLogin();
        //ILoginSettings GetPersistedLogin();
    }
}