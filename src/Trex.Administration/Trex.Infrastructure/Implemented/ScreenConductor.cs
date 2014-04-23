using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Trex.Core.Attributes;
using Trex.Core.Interfaces;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;

namespace Trex.Infrastructure.Implemented
{
    public class ScreenConductor : IScreenConductor
    {
        private readonly IAppSettings _appSettings;
        private readonly IScreenFactoryRegistry _factoryRegistry;
        private readonly IRegionManager _regionManager;
        private readonly Dictionary<Guid, IScreen> _screenDictionary;
        private readonly ITransitionService _transitionService;
        private readonly IUserSession _userSession;

        public ScreenConductor(IScreenFactoryRegistry factoryRegistry, IRegionManager regionManager, IAppSettings appSettings, IUserSession userSession, ITransitionService transitionService)
        {
            _factoryRegistry = factoryRegistry;
            _regionManager = regionManager;
            _appSettings = appSettings;
            _screenDictionary = new Dictionary<Guid, IScreen>();
            _userSession = userSession;
            _transitionService = transitionService;

            WireUpEvents();
        }

        #region IScreenConductor Members

        public void InitializeMenu() {}

        #endregion

        private void WireUpEvents()
        {
            ApplicationCommands.UserLoggedOut.RegisterCommand(new DelegateCommand<object>(UserLoggedOut));
            ApplicationCommands.ChangeScreenCommand.RegisterCommand(new DelegateCommand<IMenuInfo>(SwitchScreen));
        }

        private void UserLoggedOut(object obj)
        {
            RemoveActiveScreen();
            ApplicationCommands.GoToLogin.Execute(null);
        }

        public void SwitchScreen(IMenuInfo menuInfo)
        {
            IScreen screen;

            //If screen already exists, activate it
            if (_screenDictionary.ContainsKey(menuInfo.ScreenGuid))
            {
                screen = _screenDictionary[menuInfo.ScreenGuid];
                if (screen.IsActive)
                {
                    return;
                }

                var mainRegion = _regionManager.Regions[_appSettings.RegionNames.MainRegion];
                RemoveActiveScreen();
                var view = mainRegion.GetView(screen.Guid.ToString());
                mainRegion.Activate(view);
                _screenDictionary[menuInfo.ScreenGuid].IsActive = true;
            }
                //Else create new
            else
            {
                RemoveActiveScreen();
                var factory = _factoryRegistry.GetFactory(menuInfo.ScreenGuid);
                var mainRegion = _regionManager.Regions[_appSettings.RegionNames.MainRegion];

                screen = factory.CreateScreen(mainRegion, menuInfo.ScreenGuid);

                if (screen != null)
                {
                    screen.IsActive = true;
                    _screenDictionary.Add(menuInfo.ScreenGuid, screen);
                }
            }
            _transitionService.EnterViewAnimation(screen.MasterView);
        }

        private void RemoveActiveScreen()
        {
            var mainRegion = _regionManager.Regions[_appSettings.RegionNames.MainRegion];

            foreach (var screen in _screenDictionary.Values.ToList())
            {
                if (screen.IsActive)
                {
                    var screenAttribute = GetScreenAttribute(screen);
                    var view = mainRegion.GetView(screen.Guid.ToString());
                    if (screenAttribute.CanBeDeactivated)
                    {
                        mainRegion.Deactivate(view);
                        _screenDictionary[screen.Guid].IsActive = false;
                    }
                    else
                    {
                        mainRegion.Remove(view);
                        _screenDictionary.Remove(screen.Guid);
                    }
                }
            }
        }

        private ScreenAttribute GetScreenAttribute(IScreen screen)
        {
            ScreenAttribute attribute = null;
            foreach (var attr in screen.GetType().GetCustomAttributes(typeof (ScreenAttribute), true))
            {
                attribute = attr as ScreenAttribute;
                break;
            }
            return attribute;
        }
    }
}