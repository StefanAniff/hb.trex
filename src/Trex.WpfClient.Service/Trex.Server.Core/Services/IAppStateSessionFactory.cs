
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IAppStateSessionFactory
    {
        ApplicationStateSession AppState { get; }
    }
}
