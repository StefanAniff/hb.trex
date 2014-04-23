using Trex.Server.DataAccess;

namespace Trex.Server.Core.Services
{
    public interface ITrexBaseContextProvider
    {
        TrexBaseEntities TrexBaseEntityContext { get; }
    }
}
