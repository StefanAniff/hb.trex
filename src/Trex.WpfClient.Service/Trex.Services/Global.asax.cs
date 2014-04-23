using System;
using System.Web;
using Funq;
using Microsoft.Practices.Unity;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Configuration;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Core.Unity;
using Trex.Server.Infrastructure.Implemented;
using Trex.Server.Infrastructure.ServiceStack;
using Trex.Server.Infrastructure.UnitOfWork;
using VMDe;

namespace TrexSL.Web
{
    public class Global : HttpApplication
    {
        public class TrexAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public TrexAppHost()
                : base("TRex Web", typeof(TrexAppHost).Assembly)
            {
                JsConfig.DateHandler = JsonDateHandler.ISO8601;
            }

            public void LocalInit()
            {
                var bootStrapper = new BootStrapper();
                bootStrapper.Run();
                Init();
            }

            public override void Configure(Container container)
            {
                //Plugins.Add(new AuthFeature(() => new AuthUserSession(), new IAuthProvider[] { container.Resolve<IRenewablesAuthProvider>() }));
                var authFeature = new AuthFeature(() => new AuthUserSession(), new IAuthProvider[]
                    {
                        new CustomCredentialsAuthProvider(container.Resolve<IClientRepository>(), container.Resolve<IMembershipProvider>()), 
                        new BasicAuthProvider()
                    });
                authFeature.IncludeAssignRoleServices = false;
                Plugins.Add(authFeature);

                container.Register<ICacheClient>(new MemoryCacheClient());
            }

            public override IServiceRunner<TRequest> CreateServiceRunner<TRequest>(ActionContext actionContext)
            {
                return new RetryServiceRunner<TRequest>(this, actionContext, Container.TryResolve<IActiveSessionManager>()); //Cached per Service Action
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            var _container = UnityContainerManager.GetInstance;
            var helloAppHost = new TrexAppHost();
            var container = helloAppHost.Container;
            container.Adapter = new MyUnityContainer(_container);
            helloAppHost.LocalInit();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }

    public class MyUnityContainer : IContainerAdapter
    {
        private readonly IUnityContainer _container;

        public MyUnityContainer(IUnityContainer container)
        {
            _container = container;
        }
        public T TryResolve<T>()
        {
            try
            {
                if (_container.IsRegistered<T>())
                    return _container.Resolve<T>();
            }
            catch (Exception)
            {
                return default(T);
            }
            return default(T);
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}