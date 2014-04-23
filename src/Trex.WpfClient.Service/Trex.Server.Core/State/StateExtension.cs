using System.Collections.Generic;
using System.ServiceModel;

namespace Trex.Server.Infrastructure.State
{
    public class StateExtension : IExtension<OperationContext>
    {
        public StateExtension()
        {
            State = new Dictionary<string, object>();
        }

        public IDictionary<string, object> State { get; private set; }

        // we don't really need implementations for these methods in this case
        public void Attach(OperationContext owner) { }
        public void Detach(OperationContext owner) { }
    }
}