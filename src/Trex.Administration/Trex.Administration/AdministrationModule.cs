using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Trex.Administration.Implemented;
using Trex.Administration.Interfaces;
using Trex.Administration.UserAdministrationScreen;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.ServiceContracts;

namespace Trex.Administration
{
    public class AdministrationModule : IModule
    {
        private readonly IMenuRegistry _menuRegistry;
        private readonly IScreenFactoryRegistry _screenFactoryRegistry;
        private readonly IUnityContainer _unityContainer;

        public AdministrationModule(IUnityContainer container, IScreenFactoryRegistry screenFactoryRegistry, IMenuRegistry menuRegistry)
        {
            _unityContainer = container;
            _screenFactoryRegistry = screenFactoryRegistry;
            _menuRegistry = menuRegistry;
        }

        #region IModule Members

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            RegisterViewModels();

            var userAdminScreenInfo = MenuInfo.CreateMenuItem("User administration", true, false, Permissions.UserAdministrationPermission);
            _menuRegistry.RegisterMenuInfo(userAdminScreenInfo, "Administration", 1);
            _screenFactoryRegistry.RegisterFactory(userAdminScreenInfo.ScreenGuid, new UserAdministrationScreenFactory(_unityContainer));
            _unityContainer.Resolve<IDialogService>();
        }

        #endregion

        private void RegisterViewModels()
        {
            _unityContainer.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
        }
    }
}