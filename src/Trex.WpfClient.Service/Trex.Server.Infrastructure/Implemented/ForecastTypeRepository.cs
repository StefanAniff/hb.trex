using NHibernate;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;

namespace Trex.Server.Infrastructure.Implemented
{
    public class ForecastTypeRepository : GenericRepository<ForecastType>, IForecastTypeRepository
    {
        public ForecastTypeRepository(ISession openSession) : base(openSession)
        {
        }

        public ForecastTypeRepository(IActiveSessionManager activeSessionManager) : base(activeSessionManager)
        {
        }
    }
}