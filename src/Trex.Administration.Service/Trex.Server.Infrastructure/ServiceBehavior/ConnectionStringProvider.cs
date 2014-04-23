using System.Configuration;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.ServiceBehavior
{
    public class EFConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IDatabaseConnectionStringProvider _databaseConnectionStringProvider;

        public EFConnectionStringProvider(IDatabaseConnectionStringProvider databaseConnectionStringProvider)
        {
            _databaseConnectionStringProvider = databaseConnectionStringProvider;
        }

        public string ConnectionString
        {
            get
            {
                var context = ConfigurationManager.AppSettings["defaultEFConnectionString"];
                var dbConnStr = _databaseConnectionStringProvider.DatabaseConnectionString;
                return string.Format(context, dbConnStr);
            }
        }
    }
}