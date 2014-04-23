using System.Deployment.Application;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Trex.Dialog.SelectTask.Interfaces;
using Trex.Dialog.SelectTask.Viewmodels;
using Trex.SmartClient.BusyControl;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Infrastructure.Implemented;
using Trex.SmartClient.Infrastructure.Implemented.LocalStorage;
using Trex.SmartClient.Infrastructure.Implemented.LocalStorage.Forecast;
using Trex.SmartClient.Infrastructure.Releasenotes;
using Trex.SmartClient.LoginStatusView;
using Trex.SmartClient.MenuView;
using Trex.SmartClient.Service;
using Trex.SmartClient.TaskModule.Implemented;
using Trex.SmartClient.TaskModule.Interfaces;


namespace Trex.SmartClient
{
    public class BootStrapper : UnityBootstrapper
    {
        private IUserSession _userSession;
        private INotificationService _notificationService;
        private ISyncService _syncService;
        public BootStrapper()
        {
            ApplicationCommands.LoginSucceeded.RegisterCommand(new DelegateCommand<object>(UserLoggedIn));
            ApplicationCommands.ExitApplication.RegisterCommand(new DelegateCommand<object>(ApplicationShutdown));
        }

        private void ApplicationShutdown(object obj)
        {
            var stateService = Container.Resolve<IApplicationStateService>();
            stateService.Save();

            _notificationService.Dispose();
            _syncService.Dispose();

            Application.Current.Shutdown();
        }



        private void UserLoggedIn(object obj)
        {
            StartServices();
        }


        protected override DependencyObject CreateShell()
        {
            StartServices();
            StartUserSession();

            var theShell = new Shell(Container);
            Application.Current.MainWindow = theShell;

            theShell.DataContext = Container.Resolve<IShellViewModel>();

            theShell.Show();
            return theShell;
        }



        private void StartUserSession()
        {
            //Start user session
            _userSession = Container.Resolve<IUserSession>();
        }



        private void StartServices()
        {
            Container.Resolve<IDialogManager>();
            Container.Resolve<IScreenConductor>();
            _notificationService = Container.Resolve<INotificationService>();
            Container.Resolve<IApplicationStateService>();
            _syncService = Container.Resolve<ISyncService>();

            var busyService = Container.Resolve<IBusyService>();
            busyService.BusyView = new FlowerLoadingControl();

        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog { ModulePath = @"." };
        }


        protected override void ConfigureContainer()
        {

            RegisterServices();
            base.ConfigureContainer();
        }

        private void RegisterServices()
        {
            Container.RegisterType<IIsolatedStorageFileProvider, IsolatedStorageFileProvider>(new ContainerControlledLifetimeManager());
            Container.RegisterType<DataSetWrapper>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IForecastDataSetWrapper, ForecastDataSetWrapper>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRegionNames, RegionNames>();
            Container.RegisterType<IScreenFactoryRegistry, ScreenFactoryRegistry>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IMenuRegistry, MenuRegistry>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IUserSession, UserSession>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IMenuViewModel, MenuViewModel>();
            Container.RegisterType<IUserWlanSettingsService, UserWlanSettingsService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IAppSettings, AppSettings>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDialogManager, LoginDialogManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IScreenConductor, ScreenConductor>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IUserSettingsService, AppSettingsUserDataService>();
            Container.RegisterType<IUserPreferences, UserPreferences>();
            Container.RegisterType<IUserService, UserService>();
            Container.RegisterType<IProjectService, ProjectService>();
            Container.RegisterType<ITaskService, TaskService>();
            Container.RegisterType<ITimeEntryService, TimeEntryService>();
            Container.RegisterType<ITimeEntryTypeService, TimeEntryTypeService>();
            Container.RegisterType<ICompanyService, CompanyService>();
            Container.RegisterType<ITaskRepository, ClientTaskRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IProjectRepository, ClientProjectRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ITimeEntryRepository, ClientTimeEntryRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ITimeEntryTypeRepository, ClientTimeEntryTypeRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ILoginStatusViewModel, LoginStatusViewModel>();
            Container.RegisterType<ICompanyRepository, ClientCompanyRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISyncService, SyncService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<INotificationService, NotificationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<INotificationListener, NotificationEventListener>();
            Container.RegisterType<ITaskSearchService, TaskSearchService>();
            Container.RegisterType<IApplicationStateService, ApplicationStateService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IConnectivityService, ConnectivityService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IBusyService, BusyService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ITransitionService, TransitionService>();
            Container.RegisterType<IShellViewModel, ShellViewModel>();
            Container.RegisterType<IVersionViewModel, VersionViewModel>();
            Container.RegisterType<IForecastService, ForecastService>();            
            Container.RegisterType<ICommonDialogs, CommonDialogs>();
            Container.RegisterType<ISelectTaskViewModel, SelectTaskViewModel>();
            Container.RegisterType<IForecastUserListPresetRepository, ForecastUserListPresetRepository>();

            Container.RegisterType<IServiceStackClient, ServiceStackClient>();

            var versions = typeof(Version3_5_0).Assembly.GetTypes()
                                                .Where(t => !t.IsAbstract && t.IsClass)
                                                .SelectMany(t => t.GetInterfaces().Where(i => i == typeof(IRelease))
                                                    .Select(x => t));

            var appsettings = Container.Resolve<IAppSettings>();
            appsettings.DataDirectory = ApplicationDeployment.IsNetworkDeployed ? ApplicationDeployment.CurrentDeployment.DataDirectory : "logs";

            foreach (var version in versions)
            {
                Container.RegisterType(typeof(IRelease), version, version.FullName);
            }
        }
    }
}
