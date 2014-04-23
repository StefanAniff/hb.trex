using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Security;
using StructureMap;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure;
using Trex.Server.Infrastructure.Implemented;

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

                DynamicConnectionProvider.DynamicString = ConfigurationManager.ConnectionStrings["TrexBase"].ConnectionString;

                var clientRepository = ObjectFactory.GetInstance<IClientRepository>();
                var client = clientRepository.FindClientByCustomerId(customerId);

                if (!client.IsActivated)
                    return false;

               return Membership.ValidateUser(userName, password);
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public void LoginAsHttpUser(string userName, string customerId)
        {
            //try
            //{
            //    Membership.ApplicationName = customerId;

            //    FormsAuthentication.SetAuthCookie(userName, true);
            //}
            //catch (Exception)
            //{ }
        }

        private void OnError(Exception ex)
        {
            Logger.Log(ex);
        }

    }
}
