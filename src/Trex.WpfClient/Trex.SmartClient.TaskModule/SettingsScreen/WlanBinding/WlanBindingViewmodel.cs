using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Data;
using Trex.SmartClient.Core.Implemented;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Core.Utils;
using Trex.SmartClient.Infrastructure.Commands;
using Trex.SmartClient.Infrastructure.Implemented;

namespace Trex.SmartClient.TaskModule.SettingsScreen.WlanBinding
{
    public class WlanBindingViewmodel : ViewModelBase, IWlanBindingViewmodel
    {
        private readonly IUserWlanSettingsService _userWlanSettingsService;
        private readonly IConnectivityService _connectivityService;

        public DelegateCommand<object> DeleteBinding { get; set; }


        public WlanBindingViewmodel(IUserWlanSettingsService userWlanSettingsService, List<TimeEntryType> timeEntryTypes,
                                    IConnectivityService connectivityService)
        {
            _userWlanSettingsService = userWlanSettingsService;
            _connectivityService = connectivityService;
            TimeEntryTypes = timeEntryTypes;

            WLansBoundToTimeEntryTypes = new ObservableItemCollection<UserWLanTimeEntryTypeItemViewmodel>();

            DeleteBinding = new DelegateCommand<object>(DeleteBindingExecute);
            PremiseSettingChangedExecute(null);

            ApplicationCommands.PremiseSettingChanged.RegisterCommand(new DelegateCommand<object>(PremiseSettingChangedExecute));
        }

        private void WLansBoundToTimeEntryTypesOnCollectionChanged(object sender, ItemChangedEventArgs<UserWLanTimeEntryTypeItemViewmodel> itemChangedEventArgs)
        {
            _userWlanSettingsService.WLansBoundToTimeEntryTypes = WLansBoundToTimeEntryTypes.Select(x => x.UserWLanTimeEntryType)
                                                                                            .Where(x => x.DefaultTimeEntryTypeId != 0)
                                                                                            .ToList();
        }

        private async void PremiseSettingChangedExecute(object obj)
        {
            var list = new List<IUserWLanTimeEntryType>();
            list.AddRange(_userWlanSettingsService.WLansBoundToTimeEntryTypes);
            var availableNetworkList = await _connectivityService.GetAvailableNetworkList();
            var unlistedWLanTimeEntryTypes = availableNetworkList
                                                                 .Select(x => new UserWLanTimeEntryType(0, x))
                                                                 .Where(newWlans => list.All(x => x.WifiName != newWlans.WifiName));
            list.AddRange(unlistedWLanTimeEntryTypes);

            foreach (var userWLanTimeEntryType in list)
            {
                userWLanTimeEntryType.Connected = userWLanTimeEntryType.WifiName == await _connectivityService.GetWLanIdentification();
            }

            WLansBoundToTimeEntryTypes.Clear();
            WLansBoundToTimeEntryTypes.AddRange(list.Select(x =>
                {
                    var timeEntryTypes = TimeEntryTypes.SingleOrDefault(t => t.Id == x.DefaultTimeEntryTypeId);
                    return new UserWLanTimeEntryTypeItemViewmodel(timeEntryTypes, x);
                }));
            WLansBoundToTimeEntryTypes.ItemChanged += WLansBoundToTimeEntryTypesOnCollectionChanged;
        }



        private void DeleteBindingExecute(object obj)
        {
            var wifiName = obj.ToString();
            var toUnbind = WLansBoundToTimeEntryTypes.SingleOrDefault(x => x.WifiName == wifiName);
            if (toUnbind != null) toUnbind.SelectedTimeEntryType = null;

            OnPropertyChanged(() => WLansBoundToTimeEntryTypes);
        }

        public List<TimeEntryType> TimeEntryTypes { get; private set; }

        private ObservableItemCollection<UserWLanTimeEntryTypeItemViewmodel> _wLansBoundToTimeEntryTypes;

        public ObservableItemCollection<UserWLanTimeEntryTypeItemViewmodel> WLansBoundToTimeEntryTypes
        {
            get { return _wLansBoundToTimeEntryTypes; }
            set
            {
                _wLansBoundToTimeEntryTypes = value;
                OnPropertyChanged(() => WLansBoundToTimeEntryTypes);
            }
        }

    }
}