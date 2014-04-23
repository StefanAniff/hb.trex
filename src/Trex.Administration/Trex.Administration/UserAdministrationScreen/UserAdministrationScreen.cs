using System;
using Microsoft.Practices.Unity;
using Trex.Administration.UserAdministrationScreen.MainView;
using Trex.Core.Attributes;
using Trex.Core.Implemented;
using Trex.Core.Services;

namespace Trex.Administration.UserAdministrationScreen
{
    [Screen(Name = "UserAdministrationScreen", CanBeDeactivated = true)]
    public class UserAdministrationScreen : ScreenBase
    {
        public UserAdministrationScreen(IUnityContainer unityContainer, Guid guid) : base(guid, unityContainer) {}

        protected override void Initialize()
        {
            var dataService = Container.Resolve<IDataService>();
            var exceptionHandler = Container.Resolve<IExceptionHandlerService>();
            var userRepository = Container.Resolve<IUserRepository>();
            var mainView = new MainView.MainView();
            mainView.ViewModel = new MainViewModel(dataService, exceptionHandler, userRepository);

            

            MasterView = mainView;
        }
    }
}