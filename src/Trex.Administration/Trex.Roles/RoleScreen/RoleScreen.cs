using System;
using Microsoft.Practices.Unity;
using Trex.Core.Attributes;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Roles.RoleScreen.MainView;

namespace Trex.Roles.RoleScreen
{
    [Screen(CanBeDeactivated = true, AllowedRoles = "Administrator")]
    public class RoleScreen : ScreenBase
    {
        public RoleScreen(Guid guid, IUnityContainer unityContainer)
            : base(guid, unityContainer) {}

        protected override void Initialize()
        {
            var userSession = Container.Resolve<IUserSession>();

            var mainView = new MainView.MainView();
            var mainViewModel = new MainViewModel(userSession,Container.Resolve<IDataService>(),Container.Resolve<IExceptionHandlerService>());
            mainView.ViewModel = mainViewModel;
            MasterView = mainView;
        }
    }
}