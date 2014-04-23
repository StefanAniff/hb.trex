using System;
using System.Collections.Generic;
using System.Configuration;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using Trex.Server.Core;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.State;
using log4net;

namespace Trex.Server.Infrastructure.ServiceStack
{
    public class CustomCredentialsAuthProvider : CredentialsAuthProvider
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMembershipProvider _membershipProvider;

        private static readonly ILog Logging = LogManager.GetLogger("TRex." + typeof(CustomCredentialsAuthProvider).Name);

        public CustomCredentialsAuthProvider(IClientRepository clientRepository, IMembershipProvider membershipProvider)
        {
            _clientRepository = clientRepository;
            _membershipProvider = membershipProvider;
        }

        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        {
            try
            {
                var customerId = authService.RequestContext.GetHeader("CustomerId");
                var client = _clientRepository.FindClientByCustomerId(customerId);
                if (!client.IsActivated)
                {
                    return false;
                }

                if (_membershipProvider.ValidateUser(userName, password))
                {
                    TenantConnectionProvider.DynamicString = client.ConnectionString;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw new Exception("Error. Try again.", ex);
            }
        }

        public override void OnAuthenticated(IServiceBase authService, IAuthSession session, IOAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            //Fill the IAuthSession with data which you want to retrieve in the app eg:
            //session.FirstName = "some_firstname_from_db";
            session.Id = authService.RequestContext.GetHeader("CustomerId");
            authService.SaveSession(session, SessionExpiry);
        }
    }
}