using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.Core.Interfaces;

namespace Trex.Roles.RoleScreen
{
    public class RoleScreenFactory : IScreenFactory
    {
        private readonly IUnityContainer _unityContainer;

        public RoleScreenFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        #region IScreenFactory Members

        public IScreen CreateScreen(IRegion region, Guid guid)
        {
            var roleScreen = new RoleScreen(guid, _unityContainer);
            region.Add(roleScreen.MasterView, roleScreen.Guid.ToString());
            return roleScreen;
        }

        #endregion
    }
}