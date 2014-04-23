using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class UserWlanSettingsService : IUserWlanSettingsService
    {
        private readonly IAppSettings _appSettings;
        private readonly IConnectivityService _connectivityService;

        private readonly DelegateCommand<object> _syncCompletedCommand;
        private readonly DelegateCommand<object> _connectivityChangedCommand;

        public bool BoundToWLan { get; set; }
        public int UserWLanTimeEntryTypeId { get; set; }

        public UserWlanSettingsService(IAppSettings appSettings, IConnectivityService connectivityService)
        {
            _appSettings = appSettings;
            _connectivityService = connectivityService;

            _syncCompletedCommand = new DelegateCommand<object>(ConnectivityChangedExecute);
            _connectivityChangedCommand = new DelegateCommand<object>(ConnectivityChangedExecute);

            ApplicationCommands.ConnectivityChanged.RegisterCommand(_connectivityChangedCommand);
            ApplicationCommands.SyncCompleted.RegisterCommand(_syncCompletedCommand);
            ApplicationCommands.BootCompleted.RegisterCommand(_syncCompletedCommand);
        }

        private async void ConnectivityChangedExecute(object obj)
        {
            var wLanIdentification = await _connectivityService.GetWLanIdentification();

            var userWLanTimeEntryType = WLansBoundToTimeEntryTypes.FirstOrDefault(x => x.WifiName.ToUpper() == wLanIdentification.ToUpper());
            if (userWLanTimeEntryType != null)
            {
                UserWLanTimeEntryTypeId = userWLanTimeEntryType.DefaultTimeEntryTypeId;
                BoundToWLan = true;
            }
            else
            {
                UserWLanTimeEntryTypeId = 0;
                BoundToWLan = false;
            }
            ApplicationCommands.PremiseSettingChanged.Execute(null);
        }

        public async Task<IEnumerable<IUserWLanTimeEntryType>> FindAnyUserWLanMatches()
        {
            var availableNetworkList = await _connectivityService.GetAvailableNetworkList();

            var list = new List<IUserWLanTimeEntryType>();
            foreach (var userWLanTimeEntryTypeItemViewmodel in WLansBoundToTimeEntryTypes)
            {
                if (availableNetworkList.Any(x => x == userWLanTimeEntryTypeItemViewmodel.WifiName))
                {
                    list.Add(userWLanTimeEntryTypeItemViewmodel); ;
                }
            }
            return list;
        }

        public List<IUserWLanTimeEntryType> WLansBoundToTimeEntryTypes
        {
            get
            {
                var wLansBoundToTimeEntryTypes = _appSettings.WLansBoundToTimeEntryTypes;
                return Convert(wLansBoundToTimeEntryTypes);
            }
            set
            {
                _appSettings.WLansBoundToTimeEntryTypes = Convert(value);
                ConnectivityChangedExecute(null);
            }
        }

        private string Convert(IEnumerable<IUserWLanTimeEntryType> wLansBoundToTimeEntryTypes)
        {
            var sb = new StringBuilder();
            foreach (var wLansBoundToTimeEntryType in wLansBoundToTimeEntryTypes)
            {
                sb.AppendFormat("{0},{1};", wLansBoundToTimeEntryType.WifiName, wLansBoundToTimeEntryType.DefaultTimeEntryTypeId);
            }
            return sb.ToString();
        }

        private List<IUserWLanTimeEntryType> Convert(string wLansBoundToTimeEntryTypes)
        {
            var items = new List<IUserWLanTimeEntryType>();

            if (string.IsNullOrEmpty(wLansBoundToTimeEntryTypes))
            {
                return items;
            }

            var array = wLansBoundToTimeEntryTypes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var value in array)
            {
                var tuple = value.Split(',');
                var defaultTimeEntryTypeId = tuple[1];
                var wifiName = tuple[0];
                items.Add(new UserWLanTimeEntryType(int.Parse(defaultTimeEntryTypeId), wifiName));
            }
            return items;
        }
    }

    public class UserWLanTimeEntryType : IUserWLanTimeEntryType
    {
        public string WifiName { get; set; }
        public int DefaultTimeEntryTypeId { get; set; }
        public bool Connected { get; set; }

        public UserWLanTimeEntryType(int defaultTimeEntryTypeId, string wifiName)
        {
            DefaultTimeEntryTypeId = defaultTimeEntryTypeId;
            WifiName = wifiName;
        }
    }
}