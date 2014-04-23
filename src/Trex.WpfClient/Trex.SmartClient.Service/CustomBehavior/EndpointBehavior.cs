using System.ServiceModel.Description;

namespace Trex.SmartClient.Service.CustomBehavior
{
    public class EndpointBehavior : IEndpointBehavior
    {
        private string userName;
        private string userPassword;
        private string customerId;

        public EndpointBehavior(string userName, string userPassword, string customerId)
        {
            this.userName = userName;
            this.userPassword = userPassword;
            this.customerId = customerId;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            var inspector = new MessageInspector(this.userName, this.userPassword, this.customerId);
            clientRuntime.MessageInspectors.Add(inspector);
            ModifyDataContractSerializerBehavior(endpoint);
        }
        private static void ModifyDataContractSerializerBehavior(ServiceEndpoint endpoint)
        {
            foreach (OperationDescription operation in endpoint.Contract.Operations)
            {
                var behavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
                behavior.MaxItemsInObjectGraph = 2147483647;
            }
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

}
