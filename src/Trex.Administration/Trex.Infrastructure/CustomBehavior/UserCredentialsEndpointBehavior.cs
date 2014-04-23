using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Trex.Infrastructure.CustomBehavior
{
    public class UserCredentialsEndpointBehavior : IEndpointBehavior
    {
        private readonly string _customerId;
        private readonly string _userName;
        private readonly string _userPassword;

        public UserCredentialsEndpointBehavior(string userName, string userPassword, string customerId)
        {
            _userName = userName;
            _userPassword = userPassword;
            _customerId = customerId;
        }

        #region IEndpointBehavior Members

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var inspector = new MessageInspector(_userName, _userPassword, _customerId);
            clientRuntime.MessageInspectors.Add(inspector);
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) {}

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) {}

        public void Validate(ServiceEndpoint endpoint) {}

        #endregion
    }
}