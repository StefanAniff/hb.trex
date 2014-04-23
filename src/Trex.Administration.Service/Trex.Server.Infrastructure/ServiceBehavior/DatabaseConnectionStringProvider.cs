using System.ServiceModel;
using Trex.Server.Core.Interfaces;

namespace Trex.Server.Infrastructure.ServiceBehavior
{
    public class DatabaseConnectionStringProvider : IDatabaseConnectionStringProvider
    {
        public string DatabaseConnectionString
        {
            get { return OperationContext.Current.Extensions.Find<ConnectionContext>().ConnectionString; }
        }
    }
}