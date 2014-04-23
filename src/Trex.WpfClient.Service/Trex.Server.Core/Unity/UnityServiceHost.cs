using System;
using System.ServiceModel;
using Microsoft.Practices.Unity;
using VMDe.Core.Unity;

namespace Trex.Server.Core.Unity
{
    public class UnityServiceHost : ServiceHost
    {
        private readonly IUnityContainer _unityContainer;
        public UnityServiceHost()
        {
            _unityContainer = UnityContainerManager.GetInstance;
        }

        public UnityServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            _unityContainer = UnityContainerManager.GetInstance;
        }

        protected override void OnOpening()
        {
            new UnityServiceBehavior(_unityContainer).AddToHost(this);
            base.OnOpening();
        }
    }
}