using Trex.Core.Model;

namespace Trex.Core.Services
{
    public interface IEstimateService
    {
        double EstimateCalculated { get; }

        EstimateStatus Status { get; }
        void Calculate(double pessimisticEstimate, double optimisticEstimate, double realisticEstimate);
    }
}