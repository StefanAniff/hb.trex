using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.Core.Interfaces;

namespace Trex.Administration.UserAdministrationScreen
{
    public class UserAdministrationScreenFactory : IScreenFactory
    {
        private readonly IUnityContainer _unityContainer;

        public UserAdministrationScreenFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        #region IScreenFactory Members

        public IScreen CreateScreen(IRegion region, Guid guid)
        {
            var screen = new UserAdministrationScreen(_unityContainer, guid);
            region.Add(screen.MasterView, screen.Guid.ToString());
            return screen;
        }

        #endregion
    }
}