namespace Trex.Server.Core.Services
{
    public interface IPriceService
    {
        double GetPrice(double? price, int taskId, int userId);
    }
}