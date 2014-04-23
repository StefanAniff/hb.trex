using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.TaskModule.SettingsScreen
{
    public class SettingsScreenFactory : IScreenFactory
    {
        private readonly IUnityContainer _unityContainer;

        public SettingsScreenFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public IScreen CreateScreen(IRegion region, Guid guid)
        {
            return new SettingsScreen(guid, _unityContainer);

        }
    }
}
