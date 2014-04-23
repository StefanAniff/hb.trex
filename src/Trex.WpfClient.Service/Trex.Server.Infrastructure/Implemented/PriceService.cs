using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using System.Linq;

namespace Trex.Server.Infrastructure.Implemented
{
    public class PriceService : IPriceService
    {
        public PriceService()
        {

        }

        public double GetPrice(double? price, User user, Task task)
        {
            if (price.HasValue && price > 0)
            {
                return price.Value;
            }

            var pricePrHour = user.Price;
            var company = task.Project.Company;

            if (user.CustomerInfo.Count(uc => uc.CustomerID == company.CustomerID) > 0)
            {
                var info = user.CustomerInfo.Single(uc => uc.CustomerID == company.CustomerID);
                pricePrHour = info.PricePrHour;
            }
            return pricePrHour;
        }
    }
}