using System.ServiceModel;

namespace Trex.Server.Infrastructure.ServiceBehavior
{
    public class ConnectionContext:IExtension<OperationContext>
    {
        public void Attach(OperationContext owner)
        {}

        public void Detach(OperationContext owner)
        {}

        public string ConnectionString { get; set; }
        public string UserName { get; set; }
        public string CustomerId { get; set; }
        public int ClientApplicationType { get; set; }

    }
}