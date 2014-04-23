using System.Configuration;
using System.Linq;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;

namespace Trex.Server.Infrastructure.ServiceBehavior
{
    public class RequestConnectionStringProvider : IConnectionStringProvider
    {
        public string ConnectionString
        {
            get
            {
                var customerId = System.Web.HttpContext.Current.Request["customerId"];
                return GetConnectionString(customerId);
            }
        }

        public static string GetConnectionString(string customerID)
        {
            var trexBaseEntities = new TrexBaseEntities();

            var customer = trexBaseEntities.TrexCustomers.SingleOrDefault(c => c.CustomerId == customerID);

            var preString = ConfigurationManager.AppSettings["defaultEFConnectionString"];
            var tmp = string.Format(preString, customer.ConnectionString);
            return tmp;
        }
    }
}
