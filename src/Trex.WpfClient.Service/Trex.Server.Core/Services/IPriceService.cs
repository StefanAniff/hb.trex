using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IPriceService
    {
        double GetPrice(double? price, User getByUserID, Task task);
    }
}
