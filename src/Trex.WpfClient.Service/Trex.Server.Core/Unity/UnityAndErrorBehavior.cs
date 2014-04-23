using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using VMDe.Core.Unity;

namespace Trex.Server.Core.Unity
{
    /// <summary>
    /// Automatically implements dependencyinjection and errorhandling
    /// </summary>
    public class UnityAndErrorBehavior : Attribute, IServiceBehavior
    {
        public UnityInstanceProvider InstanceProvider { get; set; }

        public UnityAndErrorBehavior()
        {
            InstanceProvider = new UnityInstanceProvider { Container = UnityContainerManager.GetInstance };
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
            {
                var cd = cdb as ChannelDispatcher;
                if (cd != null)
                {
                    //Adds error handling to the service.. catches all exceptions
                    cd.ErrorHandlers.Add(InstanceProvider.Container.Resolve<IErrorHandler>());
                    foreach (EndpointDispatcher ed in cd.Endpoints)
                    {
                        //Lets unity resolve the service, allowing dependencyinjection
                        InstanceProvider.ServiceType = serviceDescription.ServiceType;
                        ed.DispatchRuntime.InstanceProvider = InstanceProvider;
                    }
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }
    }
}