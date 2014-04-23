using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;
using Rebus;
using Rebus.Configuration;
using Rebus.Transports.Msmq;
using Rebus.Unity;
using ServiceStack.Logging.Log4Net;
using Trex.Server.Core.Services;
using Trex.Server.Core.Unity;
using Trex.Server.Infrastructure;
using Trex.Server.Infrastructure.Implemented;
using Trex.Server.Infrastructure.Implemented.Factories;
using Trex.Server.Infrastructure.Listeners;
using Trex.Server.Infrastructure.State;
using Trex.Server.Infrastructure.UnitOfWork;
using TrexSL.Web.Intercepts;
using TrexSL.Web.ServiceInterfaces;
using log4net;
using log4net.Config;

namespace TrexSL.Web
{
    public class BootStrapper
    {
        private static IUnityContainer _container;
        private static readonly ILog Log = LogManager.GetLogger("TRex." + typeof (BootStrapper).Name);


        public void Run()
        {
            _container = UnityContainerManager.GetInstance;

            Log.Info("Configuring Logging");
            ConfigureLogging();
            Log.Info("Configured Logging");
            Log.Info("Configuring Infrastructure");
            ConfigureInfrastructure();
            ConfigureServices();
            ConfigureNhibernate();
            RegisterRepositories();
            ConfigureEntityChangeListeners();
            RegisterAutoMapperMappings.Register();
        }

        private void ConfigureEntityChangeListeners()
        {
            // IVA: Rebus disabled for now!!
            //RegisterListener<TaskChangedEvents>();
            //RegisterListener<TimeEntryChangedEvents>();
        }

        private void ConfigureServices()
        {
            _container.RegisterType<ITrexSlService, TrexSLService>(new TransientLifetimeManager());
            _container.RegisterType<IAuthenticationService, AuthenticationService>(new TransientLifetimeManager());
            _container.RegisterType<IMembershipProvider, TransiantMembershipProvider>(new TransientLifetimeManager());
        }

        private static void ConfigureLogging()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "log4net.config");
            if (File.Exists(path))
            {
                XmlConfigurator.ConfigureAndWatch(new FileInfo(path));
                ServiceStack.Logging.LogManager.LogFactory = new Log4NetFactory(true);
            }
        }


        private void RegisterListener<THandler>()
        {
            var openListenerTypes =
                new[]
                    {
                        typeof (IOnEntityCreated<>),
                        typeof (IOnEntityUpdated<>),
                        typeof (IOnEntityDeleted<>)
                    };

            var serviceTypes = typeof (THandler)
                .GetInterfaces()
                .Where(i => i.IsGenericType && openListenerTypes.Contains(i.GetGenericTypeDefinition()));

            foreach (var serviceType in serviceTypes)
            {
                var listenerName = string.Format("{0} -> {1}", serviceType.FullName, typeof (THandler).FullName);
                Log.InfoFormat("Registering listener: {0}", listenerName);
                _container.RegisterType(serviceType, typeof (THandler), listenerName);
            }
        }


        private void ConfigureInfrastructure()
        {
            _container.RegisterType<IAppSettings, ApplicationSettings>();
            _container.RegisterType<IEmailComposer, EmailComposer>();
            _container.RegisterType<IUserSession, HttpUserSession>();
            _container.RegisterType<IPermissionService, PermissionService>();
            _container.RegisterType<IRoleManagementService, RoleManagementService>();
            _container.RegisterType<ITimeEntryTypeFactory, TimeEntryTypeFactory>();
            _container.RegisterType<IUserManagementService, UserManagementService>();
            _container.RegisterType<ITaskFactory, TaskFactory>();
            _container.RegisterType<ITimeEntryFactory, TimeEntryFactory>();
            _container.RegisterType<IPriceService, PriceService>();
            _container.RegisterType<IForecastMonthFactory, ForecastMonthMonthFactory>();
            _container.RegisterType<IEventPublisher, RebusEventPublisher>();
            _container.RegisterType<IDomainSettings, DomainSettings>(new ContainerControlledLifetimeManager());
            //ConfigureRebus(); // IVA: Rebus disabled for now
        }


        protected virtual void ConfigureRebus()
        {
            var configurer = Configure.With(new UnityContainerAdapter(_container))
                                      .Transport(t => t.UseMsmqInOneWayClientMode());

            configurer.Subscriptions(s => s.StoreInSqlServer(ConfigurationManager.ConnectionStrings["TrexBase"].ConnectionString, "rebussubscriptions").EnsureTableIsCreated());

            var rebusBus = configurer
                .MessageOwnership(d => d.FromRebusConfigurationSection())
                .CreateBus()
                .Start();

            _container.RegisterInstance<IBus>(new DelayBusOperationsDecorator(rebusBus));
        }

        private static void RegisterRepositories()
        {
            _container.RegisterType<IClientRepository, ClientRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));
            _container.RegisterType<ICustomerRepository, CustomerRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));
            _container.RegisterType<IUserRepository, UserRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));
            _container.RegisterType<IVersionRepository, DBVersionRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));
            _container.RegisterType<IProjectRepository, ProjectRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));
            _container.RegisterType<ITaskRepository, TaskRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));
            _container.RegisterType<ITimeEntryRepository, TimeEntryRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));
            _container.RegisterType<ITimeEntryTypeRepository, TimeEntryTypeRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));
            _container.RegisterType<IUserRepository, UserRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));
            _container.RegisterType<IForecastRepository, ForecastRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));
            _container.RegisterType<IHolidayRepository, HolidayRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));
            _container.RegisterType<IForecastTypeRepository, ForecastTypeRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));
            _container.RegisterType<IDomainSettingRepository, DomainSettingRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));
            _container.RegisterType<IForecastMonthRepository, ForecastMonthRepository>(new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));

            _container.RegisterType(typeof(IRepository<>), typeof(GenericRepository<>), new InjectionConstructor(_container.Resolve<IActiveSessionManager>()));         
        }

        private void ConfigureNhibernate()
        {
            _container.RegisterType<INHibernateSessionFactory, MsSqlSessionFactory>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IActiveSessionManager, RequestResponseSessionManager>();
            _container.RegisterType<IRequestState, WcfRequestState>();

            _container.RegisterType<IUnitOfWork, UnitOfWork>();
            _container.RegisterType<IErrorHandler, WcfErrorHandler>();

        }
    }
}
