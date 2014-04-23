using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;
using NativeWifi;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;
using log4net;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class ConnectivityService : ViewModelBase, IConnectivityService
    {
        private readonly DelegateCommand _syncCompletedCommand;
        private readonly DelegateCommand _syncFailedCommand;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ConnectivityService()
        {
            _syncCompletedCommand = new DelegateCommand(SyncCompletedExecute);
            _syncFailedCommand = new DelegateCommand(SyncFailedExecute);

            ApplicationCommands.SyncCompleted.RegisterCommand(_syncCompletedCommand);
            ApplicationCommands.SyncFailed.RegisterCommand(_syncFailedCommand);

            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            SystemEvents.PowerModeChanged += OnPowerChange;
            SystemEvents.SessionSwitch += SystemEventsOnSessionSwitch;
            SystemEvents.DisplaySettingsChanging += SystemEventsOnDisplaySettingsChanging;
        }

        private void SystemEventsOnDisplaySettingsChanging(object sender, EventArgs eventArgs)
        {
            Logger.Debug("DisplaySettingsChanging");
        }

        private void SystemEventsOnSessionSwitch(object sender, SessionSwitchEventArgs sessionSwitchEventArgs)
        {
            Logger.Debug("OnSessionSwitch: " + sessionSwitchEventArgs.Reason);
        }

        private void OnPowerChange(Object sender, PowerModeChangedEventArgs e)
        {
            Logger.Debug("Received Powermode change: " + e.Mode);
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    ApplicationCommands.ConnectivityChanged.Execute(IsOnline);
                    ApplicationCommands.ResumedPC.Execute(null);
                    break;
            }
        }

        private void SyncFailedExecute()
        {
            IsUnstable = true;
        }

        private void SyncCompletedExecute()
        {
            IsUnstable = false;
        }

        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            Logger.Debug("NetworkAddressChanged");
            ApplicationCommands.ConnectivityChanged.Execute(IsOnline);
        }

        private bool _isUnstable;

        public bool IsUnstable
        {
            get { return _isUnstable; }
            set
            {
                _isUnstable = value;
                OnPropertyChanged(() => IsUnstable);
                ApplicationCommands.ConnectivityUnstable.Execute(null);
            }
        }

        public bool IsOnline
        {
            get { return NetworkInterface.GetIsNetworkAvailable(); }
        }

        private static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
        }

        private async Task<List<Wlan.WlanAvailableNetwork>> PrivateGetAvailableNetworkList()
        {
            var list = new List<Wlan.WlanAvailableNetwork>();

            await Task.Run(() =>
                   {
                       try
                       {
                           using (var wlan = new WlanClient())
                           {
                               foreach (var wlanIface in wlan.Interfaces)
                               {
                                   var networks = wlanIface.GetAvailableNetworkList(0);
                                   list.AddRange(networks);
                               }
                           }
                       }
                       catch (Exception ex)
                       {
                           Logger.Info(ex);
                       }
                   });
            return list;
        }

        public async Task<IEnumerable<string>> GetAvailableNetworkList()
        {
            var privateGetAvailableNetworkList = await PrivateGetAvailableNetworkList();

            return privateGetAvailableNetworkList.Select(network => GetStringForSSID(network.dot11Ssid));
        }

        public async Task<string> GetWLanIdentification()
        {
            var privateGetAvailableNetworkList = await PrivateGetAvailableNetworkList();
            foreach (var network in privateGetAvailableNetworkList)
            {
                if ((network.flags & Wlan.WlanAvailableNetworkFlags.Connected) == Wlan.WlanAvailableNetworkFlags.Connected)
                {
                    return GetStringForSSID(network.dot11Ssid);
                }
            }
            return string.Empty;
        }

        public new void Dispose()
        {
            NetworkChange.NetworkAddressChanged -= NetworkChange_NetworkAddressChanged;
            ApplicationCommands.SyncCompleted.UnregisterCommand(_syncCompletedCommand);
            ApplicationCommands.SyncFailed.UnregisterCommand(_syncFailedCommand);
            SystemEvents.PowerModeChanged -= OnPowerChange;
            base.Dispose();
        }
    }
}