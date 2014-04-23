using Trex.Server.DataAccess;

namespace Trex.Server.Core.Services
{
    public interface ITrexContextProvider
    {
        TrexEntities TrexEntityContext { get; }
       
    }
}
