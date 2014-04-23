#region

using System;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.Infrastructure.Implemented;
using TrexSL.Shell.Menu.MenuView;

#endregion

namespace TrexSL.Shell
{
    public class BootStrapper : UnityBootstrapper
    {
        private IUserSession _userSession;

        /// <summary>
        /// 	Creates the shell or main window of the application.
        /// </summary>
        /// <returns> The shell of the application. </returns>

        protected override DependencyObject CreateShell()
        {
            var shellViewModel = new ShellViewModel(this.Container);
            var theShell = new Shell();
            theShell.ViewModel = (shellViewModel);
            Application.Current.RootVisual = theShell;
            StartServices();

            return theShell;
        }

        private void StartServices()
        {
            var screenConductor = Container.Resolve<IScreenConductor>();

            StartUserSession();
        }

        private void StartUserSession()
        {
            _userSession = Container.Resolve<IUserSession>();

            _userSession.Initialize(UserSessionInitialized);
        }

        private void UserSessionInitialized(LoginResponse obj)
        {
            if (!obj.Success)
                ApplicationCommands.GoToLogin.Execute(null);
            else
            {
                ApplicationCommands.LoginSucceeded.Execute(null);
            }
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var catalog = Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(
                new Uri("/TrexSL.Shell;Component/ModuleCatalog.xaml", UriKind.Relative));

            return catalog;
        }

        protected override void ConfigureContainer()
        {
            RegisterServices();

            base.ConfigureContainer();
        }

        private void RegisterServices()
        {
            Container.RegisterType<IDataLoadingNotifier, DataLoadingNotifier>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ICustomerRepository, CustomerRepository>(new ContainerControlledLifetimeManager());

            Container.RegisterType<ICustomerFilterService, CustomerFilterService>(
                new ContainerControlledLifetimeManager());
            Container.RegisterType<IModuleProvider, ModuleProvider>();
            Container.RegisterType<IRegionNames, RegionNames>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IEstimateSettings, EstimateSettings>();
            Container.RegisterType<IAppSettings, AppSettings>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDataService, DataService>(new ContainerControlledLifetimeManager());
            //Container.RegisterType<IExcelExportService, ExcelExportService>();
            Container.RegisterType<IScreenFactoryRegistry, ScreenFactoryRegistry>(
                new ContainerControlledLifetimeManager());
            Container.RegisterType<IMenuRegistry, MenuRegistry>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IAuthenticationService, AuthenticationService>();
            Container.RegisterType<IUserSettingsService, IsolatedStorageUserDataService>();
            Container.RegisterType<IUserSession, UserSession>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IScreenConductor, ScreenConductor>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IMenuViewModel, MenuViewModel>();
            Container.RegisterType<IMenuIndexService, MenuIndexService>();
            Container.RegisterType<IUserRepository, UserRepository>(new ContainerControlledLifetimeManager());
            
            Container.RegisterType<ITransitionService, TransitionService>();

            Container.RegisterType<IExceptionHandlerService, ExceptionHandlerService>();

            Container.RegisterType<TaskManagementFilters>(new ContainerControlledLifetimeManager());
        }
    }
}