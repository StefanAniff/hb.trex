using System.ServiceModel;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Service.CustomBehavior;

namespace Trex.SmartClient.Service.Helpers
{
    public class ServiceFactory
    {
        private readonly IAppSettings _appSettings;

        public ServiceFactory(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public TrexPortalService.TrexSlServiceClient GetServiceClient(ILoginSettings loginSettings)
        {
            var client = new TrexPortalService.TrexSlServiceClient();
            client.Endpoint.Address = new EndpointAddress(_appSettings.TrexWcfServiceEndpointUri);  
          
            if (client.Endpoint.Behaviors.Find<EndpointBehavior>() == null && loginSettings != null)
            {
                var customBehavior = new EndpointBehavior(loginSettings.UserName, loginSettings.Password, loginSettings.CustomerId);
                client.Endpoint.Behaviors.Add(customBehavior);
            }

            return client;
        }

        public AuthenticationService.AuthenticationServiceClient GetAuthenticationClient(ILoginSettings loginSettings)
        {
            var client = new AuthenticationService.AuthenticationServiceClient();
            client.Endpoint.Address = new EndpointAddress(_appSettings.AuthenticationServiceUri);

            if (client.Endpoint.Behaviors.Find<EndpointBehavior>() == null && loginSettings != null)
            {
                var customBehavior = new EndpointBehavior(loginSettings.UserName, loginSettings.Password, loginSettings.CustomerId);
                client.Endpoint.Behaviors.Add(customBehavior);
            }
            return client;
        }
    }
}
