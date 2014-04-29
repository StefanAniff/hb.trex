using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class MasterScreen
    {
        public Guid Id { get { return MenuInfo != null ? MenuInfo.ScreenGuid : Guid.Empty; } }
        public IScreen Screen { get; private set; }
        public IMenuInfo MenuInfo { get; private set; }

        public MasterScreen(IScreen screen, IMenuInfo menuInfo)
        {
            Screen = screen;
            MenuInfo = menuInfo;
        }
    }

    public class ScreenConductor : IScreenConductor
    {
        private readonly IScreenFactoryRegistry _factoryRegistry;
        private readonly IRegionManager _regionManager;
        private readonly IList<MasterScreen> _masterScreens = new List<MasterScreen>();
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
            ApplicationCommands.JumpToSubmenuCommand.RegisterCommand(new DelegateCommand<JumpToSubmenuParam>(JumpToSubmenuExecute));
        }

        /// <summary>
        /// Jumps to a window specified in JumpToSubmenuParam.
        /// NB! Atm only supports within the same module
        /// </summary>
        /// <param name="activateParam"></param>
        private void JumpToSubmenuExecute(JumpToSubmenuParam activateParam)
        {
            // Get active master screen
            var activeMaster = _masterScreens.SingleOrDefault(x => x.Screen.IsActive);
            if (activeMaster == null)
                return;

            var subMenuInfo = activeMaster.MenuInfo.SubMenuInfos.SingleOrDefault(x => x.SubMenuType == activateParam.ViewType);
            if (subMenuInfo == null)
            {
                ShowJumpToSubmenuError(activateParam, activeMaster);
                return;
            }

            // Get requested submenuView to be activated
            InactiveAllSubMenuInfos(activeMaster.MenuInfo);
            ActivateSubmenu(subMenuInfo, activeMaster.Screen);
        }

        private static void ShowJumpToSubmenuError(JumpToSubmenuParam activateParam, MasterScreen activeMaster)
        {
            MessageBox.Show(
                string.Format("{0} submenuitem is not a child of module {1}", activateParam.ViewType.Name,
                              activeMaster.MenuInfo.DisplayName)
                , "Internal error"
                , MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Change active screen by given subMenuInfo
        /// </summary>
        /// <param name="subMenuInfo"></param>
        private void ChangeSubmenuExecute(SubMenuInfo subMenuInfo)
        {
            var masterScreenInfo = subMenuInfo.Parent;
            InactiveAllSubMenuInfos(masterScreenInfo);
            ActivateSubmenu(subMenuInfo, _masterScreens.Single(x => x.Id.Equals(masterScreenInfo.ScreenGuid)).Screen);
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
            //if (_masterScreenDictionary.ContainsKey(menuInfo.ScreenGuid))
            var existingMasterScreen = _masterScreens.SingleOrDefault(x => x.Id.Equals(menuInfo.ScreenGuid));
            if (existingMasterScreen != null)
            {
                screen = existingMasterScreen.Screen;
                if (screen.IsActive)
                    return;

                if (screen is IDialogScreen)
                {
                    ((IDialogScreen)existingMasterScreen.Screen).Open();
                    return;
                }

                var mainRegion = _regionManager.Regions[_appSettings.RegionNames.MainRegion];
                RemoveActiveScreen();
                var view = mainRegion.GetView(existingMasterScreen.Id.ToString());
                mainRegion.Activate(view);
                screen.IsActive = true;
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
                    _masterScreens.Add(new MasterScreen(screen, menuInfo));
                    return;
                }

                RemoveActiveScreen();
                mainRegion.Activate(screen.MasterView);

                screen.IsActive = true;
                _masterScreens.Add(new MasterScreen(screen, menuInfo));
            }

            _transitionService.EnterViewAnimation(screen.MasterView);
        }

        private void RemoveActiveScreen()
        {
            var mainRegion = _regionManager.Regions[_appSettings.RegionNames.MainRegion];

            foreach (var masterScreen in _masterScreens)
            {
                if (!masterScreen.Screen.IsActive) 
                    continue;

                var screenAttribute = masterScreen.Screen.GetScreenAttribute();
                var view = mainRegion.GetView(masterScreen.Screen.Guid.ToString());

                if (screenAttribute.CanBeDeactivated)
                {
                    mainRegion.Deactivate(view);
                    masterScreen.Screen.IsActive = false;
                }
                else
                {
                    mainRegion.Remove(view);
                    _masterScreens.Remove(masterScreen);
                }
            }
        }
    }
}