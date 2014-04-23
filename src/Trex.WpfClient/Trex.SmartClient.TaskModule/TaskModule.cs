using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.TaskModule.Implemented;
using Trex.SmartClient.TaskModule.Interfaces;
using Trex.SmartClient.TaskModule.TaskScreen;
using Trex.SmartClient.TaskModule.TaskScreen.HistoryFeedView;
using Trex.SmartClient.TaskModule.TaskScreen.TaskScreenMasterView;

namespace Trex.SmartClient.TaskModule
{
    [Module(ModuleName = "TaskModule")]
    public class TaskModule : IModule
    {
        private readonly IMenuRegistry _menuRegistry;
        private readonly IScreenFactoryRegistry _screenFactoryRegistry;
        private readonly IUnityContainer _unityContainer;
        private readonly IUserSession _userSession;
        private readonly IAppSettings _appSettings;

        public TaskModule(IUnityContainer unityContainer, IMenuRegistry menuRegistry, IScreenFactoryRegistry screenFactoryRegistry,
            IUserSession userSession, IAppSettings appSettings)
        {
            _menuRegistry = menuRegistry;
            _userSession = userSession;
            _appSettings = appSettings;

            _screenFactoryRegistry = screenFactoryRegistry;
            _unityContainer = unityContainer;
        }

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            RegisterViewModels();
            var taskAdminScreenInfo = MenuInfo.Create(0, "Registration", true, _appSettings.StartScreenIsRegistration, false, true);

            _screenFactoryRegistry.RegisterFactory(taskAdminScreenInfo.ScreenGuid, new TaskScreenFactory(_unityContainer, _userSession));
            _menuRegistry.RegisterMenuInfo(taskAdminScreenInfo);

            var settingsScreenInfo = MenuInfo.Create(2, "Settings", true, false, true, true);
            _screenFactoryRegistry.RegisterFactory(settingsScreenInfo.ScreenGuid, new SettingsScreen.SettingsScreenFactory(_unityContainer));
            _menuRegistry.RegisterMenuInfo(settingsScreenInfo);


            _unityContainer.Resolve<IDialogService>();

        }



        private void RegisterViewModels()
        {
            _unityContainer.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<IHistoryFeedViewModel, HistoryFeedViewModel>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<ITaskScreenMasterViewModel, TaskScreenMasterViewModel>();
            _unityContainer.RegisterType<ISettingsViewModel, SettingsScreen.SettingsView.SettingsViewModel>();
        }
    }
}
