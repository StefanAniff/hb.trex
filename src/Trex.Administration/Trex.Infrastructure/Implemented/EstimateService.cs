using Trex.Core.Model;
using Trex.Core.Services;

namespace Trex.Infrastructure.Implemented
{
    public class EstimateService : IEstimateService
    {
        private readonly IEstimateCalculator _estimateCalculator;

        public EstimateService(IEstimateSettings estimateSettings)
        {
            _estimateCalculator = new EstimateCalculator(estimateSettings);
        }

        #region IEstimateService Members

        public void Calculate(double pessimisticEstimate, double optimisticEstimate, double realisticEstimate)
        {
            _estimateCalculator.Calculate(pessimisticEstimate, optimisticEstimate, realisticEstimate);
        }

        public double EstimateCalculated
        {
            get { return _estimateCalculator.EstimateCalculated; }
        }

        public EstimateStatus Status
        {
            get { return _estimateCalculator.Status; }
        }

        #endregion
    }
}