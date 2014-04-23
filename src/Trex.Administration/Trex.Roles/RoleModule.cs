using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Trex.Core.Implemented;
using Trex.Core.Interfaces;
using Trex.Roles.Implemented;
using Trex.Roles.Interfaces;
using Trex.Roles.RoleScreen;
using Trex.ServiceContracts;

namespace Trex.Roles
{
    public class RoleModule : IModule
    {
        private readonly IMenuRegistry _menuRegistry;
        private readonly IScreenFactoryRegistry _screenFactoryRegistry;
        private readonly IUnityContainer _unityContainer;

        public RoleModule(IUnityContainer container, IScreenFactoryRegistry screenFactoryRegistry, IMenuRegistry menuRegistry)
        {
            _unityContainer = container;
            _screenFactoryRegistry = screenFactoryRegistry;
            _menuRegistry = menuRegistry;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewModels();

            var roleScreenInfo = MenuInfo.CreateMenuItem("Role administration", true, false, Permissions.UserAdministrationPermission);
            _menuRegistry.RegisterMenuInfo(roleScreenInfo, "Administration", 2);
            _screenFactoryRegistry.RegisterFactory(roleScreenInfo.ScreenGuid, new RoleScreenFactory(_unityContainer));

            _unityContainer.Resolve<IDialogService>();
            
        }

        #endregion

        private void RegisterViewModels()
        {
            _unityContainer.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());

           
        }
    }
}