namespace Trex.Core.Services
{
    public interface IEstimateSettings
    {
        double PessimisticWeight { get; }
        double OptimisticWeight { get; }
        double RealisticWeight { get; }
        double MinimumThreshold { get; }
        double MaximumThreshold { get; }
    }
}