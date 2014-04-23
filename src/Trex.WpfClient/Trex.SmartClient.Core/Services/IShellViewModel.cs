using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Core.Services
{
    public interface IShellViewModel : IViewModel
    {
        bool IsRunning { get; }
    }
}
