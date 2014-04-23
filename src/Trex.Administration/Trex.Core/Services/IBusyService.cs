using Trex.Core.Interfaces;

namespace Trex.Core.Services
{
    public interface IBusyService
    {
        bool IsBusy { get; set; }
        IBusyView BusyView { get; set; }
        void ShowBusy(string message);
        void HideBusy(object nullObject);
    }
}