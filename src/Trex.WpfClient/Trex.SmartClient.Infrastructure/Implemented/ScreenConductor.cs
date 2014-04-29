using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class ScreenConductor : IScreenConductor
    {
        private readonly IScreenFactoryRegistry _factoryRegistry;
        private readonly IRegionManager _regionManager;
        private readonly Dictionary<Guid, IScreen> _screenDictionary;
        private readonly IAppSettings _appSettings;
        private readonly IUserSession _userSession;
        private readonly IBusyService _busyService;
        private readonly ITransitionService _transitionService;
        private readonly IRegionNames _regionNames;

        public ScreenConductor(IScreenFactoryRegistry factoryRegistry
            , IRegionManager regionManager
            , IAppSettings appSettings
            , IUserSession userSession
            , IBusyService busyService
            , ITransitionService transitionService
            , IRegionNames regionNames)
        {
            _factoryRegistry = factoryRegistry;
            _regionManager = regionManager;
            _appSettings = appSettings;
            // _busyService = busyService;
            _screenDictionary = new Dictionary<Guid, IScreen>();
            _userSession = userSession;
            _busyService = busyService;
            _transitionService = transitionService;
            _regionNames = regionNames;

            WireUpEvents();
        }

        public void InitializeMenu()
        {

        }

        private void WireUpEvents()
        {
            ApplicationCommands.UserLoggedOut.RegisterCommand(new DelegateCommand<object>(UserLoggedOut));
            ApplicationCommands.ChangeScreenCommand.RegisterCommand(new DelegateCommand<IMenuInfo>(SwitchScreen));
            ApplicationCommands.ChangeSubmenuCommand.RegisterCommand(new DelegateCommand<SubMenuInfo>(ChangeSubmenuExecute));
            ApplicationCommands.JumpToSubmenuCommand.RegisterCommand(new DelegateCommand<Type>(JumpToSubMenuExecute));
        }

        /// <summary>
        /// Change active screen by given type
        /// </summary>
        /// <param name="viewType"></param>
        private void JumpToSubMenuExecute(Type viewType)
        {
            
        }

        /// <summary>
        /// Change active screen by given subMenuInfo
        /// </summary>
        /// <param name="subMenuInfo"></param>
        private void ChangeSubmenuExecute(SubMenuInfo subMenuInfo)
        {
            var masterScreenInfo = subMenuInfo.Parent;
            InactiveAllSubMenuInfos(masterScreenInfo);
            ActivateSubmenu(subMenuInfo, _screenDictionary[masterScreenInfo.ScreenGuid]);
        }

        private static void InactiveAllSubMenuInfos(IMenuInfo masterScreenInfo)
        {
            foreach (var submenu in masterScreenInfo.SubMenuInfos)
            {
                submenu.IsActive = false;
            }
        }

        private void ActivateSubmenu(SubMenuInfo subMenuInfo, IScreen screen)
        {
            subMenuInfo.IsActive = true;
            screen.ActivateView(subMenuInfo.SubMenuName, _regionNames.SubmenuRegion);
        }

        private void UserLoggedOut(object obj)
        {
            RemoveActiveScreen();
        }

        private void SwitchScreen(IMenuInfo menuInfo)
        {
            IScreen screen;
            if (menuInfo == null)
            {
                return;
            }

            if (!_userSession.IsLoggedIn)
            {
                return;
            }

            //If screen already exists, activate it
            if (_screenDictionary.ContainsKey(menuInfo.ScreenGuid))
            {
                screen = _screenDictionary[menuInfo.ScreenGuid];

                if (screen.IsActive)
                    return;

                if (screen is IDialogScreen)
                {
                    ((IDialogScreen)screen).Open();
                    return;
                }

                var mainRegion = _regionManager.Regions[_appSettings.RegionNames.MainRegion];
                RemoveActiveScreen();
                var view = mainRegion.GetView(screen.Guid.ToString());
                mainRegion.Activate(view);
                _screenDictionary[menuInfo.ScreenGuid].IsActive = true;
            }
            else
            {
                var factory = _factoryRegistry.GetFactory(menuInfo.ScreenGuid);
                var mainRegion = _regionManager.Regions[_appSettings.RegionNames.MainRegion];

                screen = factory.CreateScreen(mainRegion, menuInfo.ScreenGuid);
                if (screen is IDialogScreen)
                {
                    ((IDialogScreen)screen).Open();
                    _busyService.HideBusy("SwitchScreen");
                    _screenDictionary.Add(menuInfo.ScreenGuid, screen);
                    return;
                }

                RemoveActiveScreen();
                mainRegion.Activate(screen.MasterView);

                screen.IsActive = true;
                _screenDictionary.Add(menuInfo.ScreenGuid, screen);
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
                    var screenAttribute = screen.GetScreenAttribute();
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
    }
}