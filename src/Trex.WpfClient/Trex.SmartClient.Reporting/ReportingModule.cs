using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Reporting
{
    [Module(ModuleName = "ReportModule")]
    public class ReportingModule : IModule
    {

        private readonly IMenuRegistry _menuRegistry;
        private readonly IScreenFactoryRegistry _screenFactoryRegistry;
        private readonly IUnityContainer _unityContainer;

        public ReportingModule(IUnityContainer unityContainer, IMenuRegistry menuRegistry, IScreenFactoryRegistry screenFactoryRegistry)
        {
            _menuRegistry = menuRegistry;
            _screenFactoryRegistry = screenFactoryRegistry;
            _unityContainer = unityContainer;
        }

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            // Disabled for H&B
            //var taskAdminScreenInfo = MenuInfo.Create(1, "Reports", true, false, false, true);
            //_screenFactoryRegistry.RegisterFactory(taskAdminScreenInfo.ScreenGuid, new ReportScreen.ReportScreenFactory(_unityContainer));
            //_menuRegistry.RegisterMenuInfo(taskAdminScreenInfo);
        }
    }
}
