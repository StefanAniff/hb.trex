using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Core.Services
{
    public interface IBusyService
    {
        bool IsBusy { get; }
        void ShowBusy(string key);
        void HideBusy(string key);
        IView BusyView { get; set; }

    }
}
