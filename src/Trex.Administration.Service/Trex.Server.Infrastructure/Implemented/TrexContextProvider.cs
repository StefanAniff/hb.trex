using System;
using System.Data.EntityClient;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.BaseClasses;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TrexContextProvider : LogableBase, ITrexContextProvider
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly EntityConnection _entityConnection;

        public TrexContextProvider(IConnectionStringProvider connectionStringProvider)
        {
            try
            {
                _connectionStringProvider = connectionStringProvider;

                var entityConnectionStringBuilder = new EntityConnectionStringBuilder(_connectionStringProvider.ConnectionString);

                _entityConnection = new EntityConnection(entityConnectionStringBuilder.ToString());
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public TrexEntities TrexEntityContext
        {
            get
            {
                //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
                return new TrexEntities(_entityConnection);
            }
        }
    }
}