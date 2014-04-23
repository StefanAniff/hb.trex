using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.TaskModule.SettingsScreen.WlanBinding;

namespace Trex.SmartClient.TaskModule.Interfaces
{
    public interface ISettingsViewModel : IViewModel
    {
        IWlanBindingViewmodel WlanBindingViewModel { get; }
    }
}
