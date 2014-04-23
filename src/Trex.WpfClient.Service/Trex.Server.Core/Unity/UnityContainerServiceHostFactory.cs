using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Trex.Server.Core.Unity
{
    public class UnityContainerServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new UnityServiceHost(serviceType, baseAddresses);
        }
    }
}