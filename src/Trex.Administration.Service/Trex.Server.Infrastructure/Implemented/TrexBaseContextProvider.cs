using Trex.Server.Core.Services;
using Trex.Server.DataAccess;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TrexBaseContextProvider:ITrexBaseContextProvider
    {
        public TrexBaseEntities TrexBaseEntityContext
        {
            get { return  new TrexBaseEntities();}
        }
    }
}
