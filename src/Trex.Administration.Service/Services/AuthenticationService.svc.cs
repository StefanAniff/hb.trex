using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Security;

namespace TrexSL.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AuthenticationService
    {
        [OperationContract]
        public bool ValidateUser(string userName, string password, string customerId)
        {
            try
            {
                Membership.ApplicationName = customerId;
            }
            catch (Exception)
            {
                return false;
            }
            //Token Generation place
            return Membership.ValidateUser(userName, password);
        }

        [OperationContract]
        public void LoginAsHttpUser(string userName)
        {
            FormsAuthentication.SetAuthCookie(userName, true);

        }
    }
}
