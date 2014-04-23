using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
//using Trex.Core.Services;
using Trex.Core.Services;
//using Trex.Infrastructure.AuthenticationService;
using Trex.ServiceModel.Model;

namespace Trex.Infrastructure.Implemented
{
    public class AuthenticationService : IAuthenticationService
    {
        #region IAuthenticationService Members

        private readonly Func<string,string,string,IObservable<bool>> _authenticateUser;

        public AuthenticationService(ILoginSettings loginSettings)
        {
            var service = ServiceFactory.GetServiceClient(loginSettings);

            _authenticateUser = Observable.FromAsyncPattern<string,string,string,bool>(service.BeginValidateUserWithCustomerId, service.EndValidateUserWithCustomerId);
        }


        public IObservable<bool> AuthenticateUser(string userName, string passWord, string customerId)
        {
            return _authenticateUser(userName, passWord, customerId).ObserveOnDispatcher();
        }

        public IObservable<bool> LoginAsHttpUser(string userName, string password, string customerId)
        {
            ////TO-DO
            //var authService = ServiceFactory.GetAuthServiceClient();
            //authService.LoginAsHttpUserCompleted += authService_LoginAsHttpUserCompleted;
            ////password to user state?
            //authService.LoginAsHttpUserAsync(userName, password, customerId);
            return null;
        }

        #endregion

        //private void authService_LoginAsHttpUserCompleted(object sender, AsyncCompletedEventArgs e) {}

        //private void ValidateUserCompleted(object sender, ValidateUserCompletedEventArgs e)
        //{
        //    if (UserAuthenticationCompleted != null)
        //    {
        //        UserAuthenticationCompleted(this, new UserAuthenticationCompletedEventArgs(e.Result));
        //    }
        //}
    }
}